using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EmuLater
{
    public class Chip8Core : IEmulatorCore
    {
        const int width = 64;
        public int Width
        {
            get { return width; }
        }

        const int height = 32;
        public int Height
        {
            get { return height; }
        }

        const int START_ADDRESS = 0x200;
        Random rand = new Random();
        Stopwatch stopwatch = new Stopwatch();
        Stopwatch t = new Stopwatch();
        FrameCounter UPS = new FrameCounter();

        #region Hardware
        bool[,] screen = new bool[width, height];
        byte[] RAM = new byte[0xFFF];
        byte[] V = new byte[16]; //CPU registers
        ushort I; //address used to access RAM
        ushort PC; //Program Counter;
        Stack<ushort> stack = new Stack<ushort>();
        byte delay_timer = 0;
        byte sound_timer = 0;



        byte[] keyboardMap = { 88, 49, 50, 51, 81, 87, 69, 65, 83, 68, 90, 67, 52, 82, 70, 86 };
        #endregion

        #region Fontset

        byte fontset_address = 0x50;
        byte[] chip8_fontset =
        {
            0b11110000,
            0b10010000,
            0b10010000,
            0b10010000,
            0b11110000, //0

            0b00100000,
            0b01100000,
            0b00100000,
            0b00100000,
            0b01110000, //1

            0b11110000,
            0b00010000,
            0b11110000,
            0b10000000,
            0b11110000, //2

            0b11110000,
            0b00010000,
            0b11110000,
            0b00010000,
            0b11110000, //3

            0b10010000,
            0b10010000,
            0b11110000,
            0b00010000,
            0b00010000, //4

            0b11110000,
            0b10000000,
            0b11110000,
            0b00010000,
            0b11110000, //5

            0b11110000,
            0b10000000,
            0b11110000,
            0b10010000,
            0b11110000, //6

            0b11110000,
            0b00010000,
            0b00100000,
            0b01000000,
            0b01000000, //7

            0b11110000,
            0b10010000,
            0b11110000,
            0b10010000,
            0b11110000, //8

            0b11110000,
            0b10010000,
            0b11110000,
            0b00010000,
            0b11110000, //9

            0b11110000,
            0b10010000,
            0b11110000,
            0b10010000,
            0b10010000, //A

            0b11100000,
            0b10010000,
            0b11100000,
            0b10010000,
            0b11100000, //B

            0b11110000,
            0b10000000,
            0b10000000,
            0b10000000,
            0b11110000, //C

            0b11100000,
            0b10010000,
            0b10010000,
            0b10010000,
            0b11100000, //D

            0b11110000,
            0b10000000,
            0b11110000,
            0b10000000,
            0b11110000, //E

            0b11110000,
            0b10000000,
            0b11110000,
            0b10000000,
            0b10000000  //F
        };
        #endregion

        #region Instruction Shorthands

        ushort instruction;
        ushort lastInstruction;
        internal double delay = .00325;

        ushort Instruction
        {
            get { return instruction; }
            set { lastInstruction = instruction; instruction = value; }
        }

        int X
        {
            get { return (Instruction & 0x0F00) >> 8; }
        }

        int Y
        {
            get { return (Instruction & 0x00F0) >> 4; }
        }

        byte N
        {
            get { return (byte)(Instruction & 0x000F); }
        }

        byte NN
        {
            get { return (byte)(Instruction & 0x00FF); }
        }

        ushort NNN
        {
            get { return (ushort)(Instruction & 0x0FFF); }
        }

        #endregion

        public Chip8Core()
        {
            ResetHardware();
        }

        void ResetHardware()
        {
            for (int i = 0; i < RAM.Length; i++)
            {
                RAM[i] = 0;
            }

            LoadFontSet();
            for (int i = 0; i < V.Length; i++)
            {
                V[i] = 0;
            }

            I = 0;
            Instruction = 0;
            stack.Clear();
            PC = START_ADDRESS;
        }

        
        // Load font set into memory
        void LoadFontSet()
        {
            for (int i = 0; i < chip8_fontset.Length; ++i)
            {
                RAM[i + fontset_address] = chip8_fontset[i];
            }
        }

        public void LoadROM(string path)
        {
            ResetHardware();
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    var offset = START_ADDRESS;
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        if (offset < RAM.Length)
                        {
                            RAM[offset++] = reader.ReadByte();
                        }
                        else break;
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("That ROM does not exist");
            }

            UPS.Start();
            t.Start();
        }

        public void DumpRAM()
        {
            using (var stream = new FileStream("chip8.ram", FileMode.Create))
            {
                stream.Write(RAM, fontset_address, RAM.Length - fontset_address);
            }
        }

        public void Run()
        {
            Input.Poll();
            Tick();
            UPS.Update();
            Wait(delay);
        }


        public void Render(Graphics g)
        {
            g.Clear(Color.Black);
            for (int x = 0; x < screen.GetLength(0); x++)
            {
                for (int y = 0; y < screen.GetLength(1); y++)
                {
                    if (screen[x, y])
                        g.FillRectangle(Brushes.White, x * GameForm.SCALE, y * GameForm.SCALE, GameForm.SCALE, GameForm.SCALE);
                }
            }
            //g.DrawString(String.Format("{0:F2} UPS", UPS.Get()), SystemFonts.DefaultFont, Brushes.Gray, 0, 32);
        }

        void Tick()
        {
            Fetch();
            Decode();

            if (t.Elapsed.TotalSeconds > .0166667)
            {
                if (delay_timer > 0)
                    delay_timer--;
                if (sound_timer > 0)
                {
                    //TODO Play Buzzer continous while sound_timer is active
                    if (sound_timer == 1)
                        Console.Beep();
                    sound_timer--;
                }
                t.Restart();
            }
        }

        void IncrementPC()
        {
            PC += 2;
        }

        void Fetch()
        {
            Instruction = (ushort)((RAM[PC] << 8) | (RAM[PC + 1]));
            IncrementPC();
        }

        void Decode()
        {
            switch (Instruction & 0xF000)
            {
                case 0x0000:
                    switch (Instruction & 0x00FF)
                    {
                        case 0x00E0:
                            _00E0();
                            break;
                        case 0x00EE:
                            _00EE();
                            break;
                        default:
                            _0NNN();
                            break;
                    }
                    break;
                case 0x1000:
                    _1NNN();
                    break;
                case 0x2000:
                    _2NNN();
                    break;
                case 0x3000:
                    _3XNN();
                    break;
                case 0x4000:
                    _4XNN();
                    break;
                case 0x5000:
                    _5XY0();
                    break;
                case 0x6000:
                    _6XNN();
                    break;
                case 0x7000:
                    _7XNN();
                    break;
                case 0x8000:
                    switch (Instruction & 0x000F)
                    {
                        case 0x0000:
                            _8XY0();
                            break;
                        case 0x0001:
                            _8XY1();
                            break;
                        case 0x0002:
                            _8XY2();
                            break;
                        case 0x0003:
                            _8XY3();
                            break;
                        case 0x0004:
                            _8XY4();
                            break;
                        case 0x0005:
                            _8XY5();
                            break;
                        case 0x0006:
                            _8XY6();
                            break;
                        case 0x0007:
                            _8XY7();
                            break;
                        case 0x000E:
                            _8XYE();
                            break;
                        default:
                            InvalidOpcode();
                            break;
                    }
                    break;
                case 0x9000:
                    _9XY0();
                    break;
                case 0xA000:
                    _ANNN();
                    break;
                case 0xB000:
                    _BNNN();
                    break;
                case 0xC000:
                    _CXNN();
                    break;
                case 0xD000:
                    _DXYN();
                    break;
                case 0xE000:
                    switch (Instruction & 0x00FF)
                    {
                        case 0x009E:
                            _EX9E();
                            break;
                        case 0x00A1:
                            _EXA1();
                            break;
                        default:
                            InvalidOpcode();
                            break;
                    }
                    break;
                case 0xF000:
                    switch (Instruction & 0x00FF)
                    {
                        case 0x0007:
                            _FX07();
                            break;
                        case 0x000A:
                            _FX0A();
                            break;
                        case 0x0015:
                            _FX15();
                            break;
                        case 0x0018:
                            _FX18();
                            break;
                        case 0x001E:
                            _FX1E();
                            break;
                        case 0x0029:
                            _FX29();
                            break;
                        case 0x0033:
                            _FX33();
                            break;
                        case 0x0055:
                            _FX55();
                            break;
                        case 0x0065:
                            _FX65();
                            break;
                        default:
                            InvalidOpcode();
                            break;
                    }
                    break;
                default:
                    InvalidOpcode();
                    break;
            }
        }

        #region Opcodes 

        
        //clear the screen
        void _00E0()
        {
            for (int x = 0; x < screen.GetLength(0); x++)
            {
                for (int y = 0; y < screen.GetLength(1); y++)
                {
                    screen[x, y] = false;
                }
            }
        }

        
        //return from a subroutine
        void _00EE()
        {
            PC = stack.Pop();
        }

        
        //unused on modern devices, leave as nop
        void _0NNN()
        {

        }

        
        //Jump the program counter to the specified address
        void _1NNN()
        {
            PC = NNN;
        }

        
        //call subroutine at NNN
        void _2NNN()
        {
            stack.Push(PC);
            PC = NNN;
        }

        
        //skip the next instruction if register VX equals NN
        void _3XNN()
        {
            if (V[X] == NN)
                IncrementPC();
        }


        
        //skip the next instruction if register VX doesn't equal NN
        void _4XNN()
        {
            if (V[X] != NN)
                IncrementPC();
        }

        
        //skip the next instruction if register VX equals VY
        void _5XY0()
        {
            if (V[X] == V[Y])
                IncrementPC();
        }

        void _6XNN()
        {
            V[X] = NN;
        }

        void _7XNN()
        {
            V[X] += NN;
        }

        
        //Store value VY in VX
        void _8XY0()
        {
            V[X] = V[Y];
        }

        
        //store VX OR VY in VX
        void _8XY1()
        {
            V[X] = (byte)(V[X] | V[Y]);
        }

        
        //store VX AND VY in VX
        void _8XY2()
        {
            V[X] = (byte)(V[X] & V[Y]);
        }

        
        //store VX XOR VY in VX
        void _8XY3()
        {
            V[X] = (byte)(V[X] ^ V[Y]);
        }

        
        //add VY to VX, set flag if carry occurs
        void _8XY4()
        {
            int result = V[X] + V[Y];

            if (result > 255)
                V[0xF] = 1;
            else
                V[0xF] = 0;

            V[X] = (byte)result;
        }

        
        //subtract VY from VX, store in VX, set flag if there is no borrow
        void _8XY5()
        {
            int result = V[X] - V[Y];

            if (V[X] > V[Y])
                V[0xF] = 1;
            else
                V[0xF] = 0;

            V[X] = (byte)result;
        }

        void _8XY6()
        {
            V[0xF] = (byte)(V[X] & 1);
            V[X] >>= 1;
        }

        
        //subtract VX from VY, store in VX, set flag if there is no borrow
        void _8XY7()
        {
            var result = V[Y] - V[X];

            if (V[Y] > V[X])
                V[0xF] = 1;
            else
                V[0xF] = 0;

            V[X] = (byte)result;
        }

        
        //set VF to most significant bit, then shift left
        void _8XYE()
        {
            V[0xF] = (byte)(V[X] & 128);
            V[X] <<= 1;
        }

        
        //If VX doesn't equal VY, skip next instruction
        void _9XY0()
        {
            if (V[X] != V[Y])
                IncrementPC();
        }

        
        //Set register I to NNN
        void _ANNN()
        {
            I = NNN;
        }

        void _BNNN()
        {
            PC = (ushort)(NNN + V[0]);
        }

        void _CXNN()
        {
            V[X] = (byte)(rand.Next(256) & NN);
        }

        
        //Draw sprite TODO allow ship to go all the way to edge of screen
        void _DXYN()
        {
            var x = V[X];
            var y = V[Y];
            V[0xF] = 0;

            for (int line = 0; line < N; line++)
            {
                byte pixelLine = RAM[I + line];
                for (int i = 0; i < 8; i++)
                {
                    bool toggle = ((pixelLine & 128) == 128);
                    pixelLine <<= 1;
                    var p = (x + i);
                    var q = (y + line);
                    var was = screen[p, q];

                    screen[p, q] ^= toggle;
                    var now = screen[p, q];

                    if (was & !now)
                        V[0xF] = 1;
                }
            }
        }

        
        //If key VX is pressed, skip next instruction
        void _EX9E()
        {
            if (Input.GetKey((Keys)keyboardMap[V[X]]))
                IncrementPC();
        }
        
        //If key VX is not pressed, skip next instruction
        void _EXA1()
        {
            if (!(Input.GetKey((Keys)keyboardMap[V[X]])))
                IncrementPC();
        }

        void _FX07()
        {
            V[X] = delay_timer;
        }

        
        //wait for keypress, then store key in  VX 
        void _FX0A()
        {
            V[X] = WaitForKeyPress();
        }

        void _FX15()
        {
            delay_timer = V[X];
        }

        void _FX18()
        {
            sound_timer = V[X];
        }

        void _FX1E()
        {
            I += V[X];
        }

        void _FX29()
        {
            throw new NotImplementedException();
        }

        
        //Convert the value of V[X] to BCD and store at addresses I through I+2
        void _FX33()
        {
            throw new NotImplementedException();
        }

        
        //Load registers 0-X into memory starting at address I. Increase I by X+1
        void _FX55()
        {
            for (int i = 0; i < X; i++)
            {
                RAM[I + i] = V[i];
            }
            I = (ushort)(I + X + 1);
        }

        void _FX65()
        {
            for (int i = 0; i <= X; i++)
            {
                V[i] = RAM[I + i];
            }
            I = (ushort)(I + X + 1);
        }
        #endregion

        void InvalidOpcode()
        {
            string message = String.Format("instruction 0x{0:X} is not valid.\nthe last valid instruction executed was : 0x{1:X}", Instruction, lastInstruction);
            Console.WriteLine(message);
            MessageBox.Show(message);
            Environment.Exit(-1);
        }

        byte WaitForKeyPress()
        {
            while (KeyPressed() == 0)
            {
                Input.Poll();
                Application.DoEvents();
            }
            return KeyPressed();
        }

        byte KeyPressed()
        {
            for (int i = 0; i < keyboardMap.Length; i++)
            {
                if (Input.GetKey((Keys)keyboardMap[i]))
                    return keyboardMap[i];
            }
            return 0;
        }

        void Wait(double delay)
        {
            stopwatch.Restart();
            while (stopwatch.Elapsed.TotalSeconds < delay)
            {
                //do nothing
            }
            stopwatch.Restart();
        }
    }
}

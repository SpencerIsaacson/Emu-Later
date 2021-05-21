using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp
{
    class Chip8Assembler
    {
        public static int Assemble(string fileName)
        {
            var lines = new List<string>();
            var bytes = new List<byte>();

            using (TextReader file = new StreamReader(new FileStream(fileName, FileMode.Open)))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                    lines.Add(line);
            }

            for (int i = 0; i < lines.Count; i++)
            {
                string[] tokens = lines[i].Split(' '); //TODO rename, doesn't represent "tokens" strictly speaking
                string mneumonic = tokens[0].ToLower();
                string[] arguments = { };

                if (tokens.Length > 1)
                    arguments = tokens[1].Split(',');

                switch (mneumonic)
                {
                    case "":
                        break;
                    case "rca":
                        bytes.Add((byte)(0x0 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[0][1]) << 4) | HexCharToByte(arguments[0][2])));
                        break;
                    case "return":
                        bytes.Add(0x00);
                        bytes.Add(0xEE);
                        break;
                    case "cls":
                        bytes.Add(0x00);
                        bytes.Add(0xE0);
                        break;
                    case "jump":
                        bytes.Add((byte)(0x10 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[0][1]) << 4) | HexCharToByte(arguments[0][2])));
                        break;
                    case "call":
                        bytes.Add((byte)(0x20 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[0][1]) << 4) | HexCharToByte(arguments[0][2])));
                        break;
                    case "sei":
                        bytes.Add((byte)(0x30 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | HexCharToByte(arguments[1][1])));
                        break;
                    case "snei":
                        bytes.Add((byte)(0x40 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | HexCharToByte(arguments[1][1])));
                        break;
                    case "ser":
                        bytes.Add((byte)(0x50 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4)));
                        break;
                    case "movi":
                        bytes.Add((byte)(0x60 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | HexCharToByte(arguments[1][1])));
                        break;
                    case "addi":
                        bytes.Add((byte)(0x70 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | HexCharToByte(arguments[1][1])));
                        break;
                    case "movr":
                        bytes.Add((byte)(0x80 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4)));
                        break;
                    case "or":
                        bytes.Add((byte)(0x80 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | 1));
                        break;
                    case "and":
                        bytes.Add((byte)(0x80 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | 2));
                        break;
                    case "xor":
                        bytes.Add((byte)(0x80 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | 3));
                        break;
                    case "addr":
                        bytes.Add((byte)(0x80 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | 4));
                        break;
                    case "x_min_y":
                        bytes.Add((byte)(0x80 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | 5));
                        break;
                    case "right_shift":
                        bytes.Add((byte)(0x80 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | 6));
                        break;
                    case "y_min_x":
                        bytes.Add((byte)(0x80 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | 7));
                        break;
                    case "left_shift":
                        bytes.Add((byte)(0x80 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | 0xE));
                        break;
                    case "sner":
                        bytes.Add((byte)(0x90 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4)));
                        break;
                    case "set_i":
                        bytes.Add((byte)(0xA0 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[0][1]) << 4) | HexCharToByte(arguments[0][2])));
                        break;
                    case "jump_offset":
                        bytes.Add((byte)(0xB0 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[0][1]) << 4) | HexCharToByte(arguments[0][2])));
                        break;
                    case "rand":
                        bytes.Add((byte)(0xC0 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | HexCharToByte(arguments[1][1])));
                        break;
                    case "draw":
                        bytes.Add((byte)(0xD0 | HexCharToByte(arguments[0][0])));
                        bytes.Add((byte)((HexCharToByte(arguments[1][0]) << 4) | HexCharToByte(arguments[2][0])));
                        break;
                    case "skr":
                        bytes.Add((byte)(0xE0 | HexCharToByte(arguments[0][0])));
                        bytes.Add(0x9E);
                        break;
                    case "snkr":
                        bytes.Add((byte)(0xE0 | HexCharToByte(arguments[0][0])));
                        bytes.Add(0xA1);
                        break;
                    case "get_delay":
                        bytes.Add((byte)(0xF0 | HexCharToByte(arguments[0][0])));
                        bytes.Add(0x07);
                        break;
                    case "await_key":
                        bytes.Add((byte)(0xF0 | HexCharToByte(arguments[0][0])));
                        bytes.Add(0x0A);
                        break;
                    case "delay_timer":
                        bytes.Add((byte)(0xF0 | HexCharToByte(arguments[0][0])));
                        bytes.Add(0x15);
                        break;
                    case "sound_timer":
                        bytes.Add((byte)(0xF0 | HexCharToByte(arguments[0][0])));
                        bytes.Add(0x18);
                        break;
                    case "advance_i":
                        bytes.Add((byte)(0xF0 | HexCharToByte(arguments[0][0])));
                        bytes.Add(0x1E);
                        break;
                    case "bcd":
                        bytes.Add((byte)(0xF0 | HexCharToByte(arguments[0][0])));
                        bytes.Add(0x33);
                        break;
                    case "reg_dump":
                        bytes.Add((byte)(0xF0 | HexCharToByte(arguments[0][0])));
                        bytes.Add(0x55);
                        break;
                    case "reg_load":
                        bytes.Add((byte)(0xF0 | HexCharToByte(arguments[0][0])));
                        bytes.Add(0x65);
                        break;
                    case "#begin_data":
                        while (lines[++i] != "#end_data")
                        {
                            bytes.Add(byte.Parse(lines[i], System.Globalization.NumberStyles.HexNumber));
                        }
                        break;
                    case "#begin_ascii":
                        while (lines[++i] != "#end_ascii")
                        {
                            //TODO
                        }
                        break;
                    case "invalid": //TODO replace "invalids" with data blocks
                        bytes.Add(0);
                        bytes.Add(0);
                        break;
                    default:
                        return -1; //Unsupported Operation
                }

            }

            using (var stream = new FileStream("MyAssembly.ch8", FileMode.Create))
            {
                stream.Write(bytes.ToArray(), 0, bytes.Count);
            }

            return 0;
        }


        public static void Disassemble_LinearSweep(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream(fileName, FileMode.Open)))
            using (TextWriter writer = new StreamWriter(new FileStream("disassembly.asm", FileMode.Create)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length - 1)
                {
                    byte highByte = reader.ReadByte();
                    byte lowByte = reader.ReadByte();
                    ushort instruction = (ushort)((highByte << 8) | (lowByte));

                    int Op = (highByte & 0xF0) >> 4;
                    int X = (instruction & 0x0F00) >> 8;
                    int Y = (instruction & 0x00F0) >> 4;
                    int N = instruction & 0x000F;
                    int NN = instruction & 0x00FF;
                    int NNN = instruction & 0x0FFF;

                    string address = string.Format("0x{0:X3}", NNN);
                    string x_and_byte_constant = string.Format("v{0:X1} {1:X2}", X, NN);
                    string x_and_y = string.Format("v{0:X1} v{1:X1}", X, Y);
                    string x_y_and_nibble_constant = string.Format("v{0:X1} v{1:X1} {2:X1}", X, Y, N);


                    string line = new Func<string>(() =>
                    {
                        switch (Op)
                        {
                            case 0:
                                if (NNN == 0x0EE)
                                    return "return";
                                if ((NNN & 0xF0) == 0x0E0)
                                    return "cls";
                                return "RCA " + address;
                            case 1: return "jump " + address;
                            case 2: return "call " + address;
                            case 3: return "sei " + x_and_byte_constant;
                            case 4: return "snei " + x_and_byte_constant;
                            case 5:
                                return (N != 0) ? "invalid" : "ser " + x_and_y;
                            case 6: return "movi " + x_and_byte_constant;
                            case 7: return "addi " + x_and_byte_constant;
                            case 8:
                                switch (N)
                                {
                                    case 0x0: return "movr " + x_and_y;
                                    case 0x1: return "or " + x_and_y;
                                    case 0x2: return "and " + x_and_y;
                                    case 0x3: return "xor " + x_and_y;
                                    case 0x4: return "addr " + x_and_y;
                                    case 0x5: return "x_min_y " + x_and_y;
                                    case 0x6: return "right_shift " + x_and_y;
                                    case 0x7: return "y_min_x " + x_and_y;
                                    case 0xE: return "left_shift " + x_and_y;
                                    default: return "invalid";
                                }
                            case 9:
                                return (N != 0) ? "invalid" : "sner " + x_and_y;
                            case 0xA: return "set_i " + address;
                            case 0xB: return "jump_offset " + address;
                            case 0xC: return "rand " + x_and_byte_constant;
                            case 0xD: return "draw " + x_y_and_nibble_constant;
                            case 0xE:
                                switch (NN)
                                {
                                    case 0x9E:return $"skr {X:X}";
                                    case 0xA1:return $"snkr {X:X}";
                                    default: return "invalid";
                                }
                            case 0xF:
                                switch (NN)
                                {
                                    case 0x07: return $"get_delay {X:X}";
                                    case 0x0A: return $"await_key {X:X}";
                                    case 0x15: return $"delay_timer {X:X}";
                                    case 0x18: return $"sound_timer {X:X}";
                                    case 0x1E: return $"advance_i {X:X}";
                                    case 0x29: return $"sprite_addr {X:X}";
                                    case 0x33: return $"BCD {X:X}";
                                    case 0x55: return $"reg_dump {X:X}";
                                    case 0x65: return $"reg_load {X:X}";
                                    default: return "invalid";
                                }
                            default: return string.Format("Stopped at unrecognized instruction {0:X4} at address {1:X4}", instruction, reader.BaseStream.Position - 2);
                        }
                    })();

                    writer.WriteLine(line);
                }
            }
        }

        public static int Disassemble_LinearSweep_BasicSyntax(string fileName)
        {
            int return_code = 0;

            using (BinaryReader reader = new BinaryReader(new FileStream(fileName, FileMode.Open)))
            using (TextWriter writer = new StreamWriter(new FileStream("disassembly.asm", FileMode.Create)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length - 1)
                {
                    byte highByte = reader.ReadByte();
                    byte lowByte = reader.ReadByte();
                    ushort instruction = (ushort)((highByte << 8) | (lowByte));

                    int Op = (highByte & 0xF0) >> 4;
                    int X = (instruction & 0x0F00) >> 8;
                    int Y = (instruction & 0x00F0) >> 4;
                    int N = instruction & 0x000F;
                    int NN = instruction & 0x00FF;
                    int NNN = instruction & 0x0FFF;

                    string address = $"{NNN:X3}";
                    string x_and_byte_constant = $"{X:X1},{NN:X2}";
                    string x_and_y = $"{X:X1},{Y:X1}";
                    string x_y_and_nibble_constant = $"{X:X1},{Y:X1},{N:X1}";


                    string line = new Func<string>(() =>
                    {
                        switch (Op)
                        {
                            case 0:
                                if (NNN == 0x0EE)
                                    return "return";
                                if ((NNN & 0xF0) == 0x0E0)
                                    return "cls";
                                return "RCA " + address;
                            case 1: return "jump " + address;
                            case 2: return "call " + address;
                            case 3: return "sei " + x_and_byte_constant;
                            case 4: return "snei " + x_and_byte_constant;
                            case 5:
                                return (N != 0) ? "invalid" : "ser " + x_and_y;
                            case 6: return "movi " + x_and_byte_constant;
                            case 7: return "addi " + x_and_byte_constant;
                            case 8:
                                switch (N)
                                {
                                    case 0x0: return "movr " + x_and_y;
                                    case 0x1: return "or " + x_and_y;
                                    case 0x2: return "and " + x_and_y;
                                    case 0x3: return "xor " + x_and_y;
                                    case 0x4: return "addr " + x_and_y;
                                    case 0x5: return "x_min_y " + x_and_y;
                                    case 0x6: return "right_shift " + x_and_y;
                                    case 0x7: return "y_min_x " + x_and_y;
                                    case 0xE: return "left_shift " + x_and_y;
                                    default: return "invalid";
                                }
                            case 9:
                                return (N != 0) ? "invalid" : "sner " + x_and_y;
                            case 0xA: return "set_i " + address;
                            case 0xB: return "jump_offset " + address;
                            case 0xC: return "rand " + x_and_byte_constant;
                            case 0xD: return "draw " + x_y_and_nibble_constant;
                            case 0xE:
                                switch (NN)
                                {
                                    case 0x9E: return $"skr {X:X}";
                                    case 0xA1: return $"snkr {X:X}";
                                    default: return "invalid";
                                }
                            case 0xF:
                                switch (NN)
                                {
                                    case 0x07: return $"get_delay {X:X}";
                                    case 0x0A: return $"await_key {X:X}";
                                    case 0x15: return $"delay_timer {X:X}";
                                    case 0x18: return $"sound_timer {X:X}";
                                    case 0x1E: return $"advance_i {X:X}";
                                    case 0x29: return $"sprite_addr {X:X}";
                                    case 0x33: return $"BCD {X:X}";
                                    case 0x55: return $"reg_dump {X:X}";
                                    case 0x65: return $"reg_load {X:X}";
                                    default: return "invalid";
                                }
                            default: return_code = -1; return string.Format("Stopped at unrecognized instruction {0:X4} at address {1:X4}", instruction, reader.BaseStream.Position - 2);
                        }
                    })();

                    writer.WriteLine(line);

                }
            }

            return return_code;
        }


        /// <summary>
        /// Currently not implemented
        /// </summary>
        /// <param name="fileName"></param>
        public static void Disassemble_RecursiveSweep(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream(fileName, FileMode.Open)))
            using (TextWriter writer = new StreamWriter(new FileStream("game.asm", FileMode.Create)))
            {
                byte[] bytes = new byte[reader.BaseStream.Length];
                bool[] visited = new bool[bytes.Length];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = reader.ReadByte();
                    Console.WriteLine(bytes[i]);
                }

            }
        }

        private static void Recurse(byte[] bytes, bool[] visited, int addr)
        {
            for (int i = addr; i < bytes.Length; i += 2)
            {
                if (visited[i])
                    return;
                //decode
                //if branch, recurse for each target
            }
        }



        static byte HexCharToByte(char digit)
        {
            if (lookup.ContainsKey(digit))
                return lookup[digit];
            else return 255; //invalid character
        }

        static Dictionary<char, byte> lookup = new Dictionary<char, byte>()
        {
            {'0', 0 },
            {'1', 1 },
            {'2', 2 },
            {'3', 3 },
            {'4', 4 },
            {'5', 5 },
            {'6', 6 },
            {'7', 7 },
            {'8', 8 },
            {'9', 9 },
            {'A', 0xA },
            {'B', 0xB },
            {'C', 0xC },
            {'D', 0xD },
            {'E', 0xE },
            {'F', 0xF },
            {'a', 0xA },
            {'b', 0xB },
            {'c', 0xC },
            {'d', 0xD },
            {'e', 0xE },
            {'f', 0xF },
        };
    }
}
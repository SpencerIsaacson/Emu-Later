using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                if (args[0] == "asm")
                {
                    int result_code = Chip8Assembler.Assemble(args[1]);
                    if (result_code == 0)
                        Console.WriteLine("Successfully assembled " + args[1]);
                    else
                        Console.WriteLine("Failed to assemble" + args[1]);
                }
                else if (args[0] == "dis")
                {
                    int result_code = Chip8Assembler.Disassemble_LinearSweep_BasicSyntax(args[1]);
                    if (result_code == 0)
                        Console.WriteLine("Successfully disassembled " + args[1]);
                    else
                        Console.WriteLine("Failed to disassemble" + args[1]);

                }
                else
                    Console.WriteLine($"Unrecognized assembler command '{args[0]}'");
            }
            else
            {
                Console.WriteLine("You must provide a command-line argument for the path of the file to assemble");
            }
            Console.ReadLine();
        }

        public static bool BinariesMatch()
        {
            using (BinaryReader mine = new BinaryReader(new FileStream("INVADERS", FileMode.Open)))
            using (BinaryReader yours = new BinaryReader(new FileStream("MyAssembly.ch8", FileMode.Open)))
            {
                int i = 0;
                while (!(mine.BaseStream.EndOfStream() || yours.BaseStream.EndOfStream()))
                {
                    var myByte = mine.ReadByte();
                    var yourByte = yours.ReadByte();
                    Console.WriteLine($"{i++:X} {myByte}:{yourByte}");
                    if (myByte != yourByte)
                        return false;

                }
            }
            return true;
        }
    }

    public static class Extensions
    {
        public static bool EndOfStream(this Stream stream)
        {
            return stream.Position == stream.Length;
        }
    }
}

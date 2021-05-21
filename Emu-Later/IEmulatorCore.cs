using System.Drawing;

namespace EmuLater
{
    interface IEmulatorCore
    {
        void LoadROM(string path);
        void Render(Graphics g);
        void Run();
        void DumpRAM();
        int Width { get; }
        int Height { get; }
    }
}

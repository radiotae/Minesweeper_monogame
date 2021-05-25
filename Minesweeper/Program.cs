using System;

namespace Minesweeper
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Minesweeper())
                game.Run();
        }
    }
}

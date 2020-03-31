using System;

namespace KalahaBot
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Board testboard = new Board(7, 7);
            GameControl game = new GameControl(testboard);
            game.init();
        }
    }
}

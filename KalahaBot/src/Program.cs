using System;

namespace KalahaBot
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            GameControl game = new GameControl(new Board(6, 6));
            game.init();
        }
    }
}

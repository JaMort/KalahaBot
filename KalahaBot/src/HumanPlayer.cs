using System;
using System.Collections.Generic;
using System.Text;

namespace KalahaBot
{
    class HumanPlayer : IPlayer
    {
        public string name { get; set; }
        private Side side { get; set; } 

        /// <summary>
        /// Constructor for HumanPlayer.
        /// </summary>
        /// <param name="side">The players side</param>
        public HumanPlayer(Side side)
        {
            this.side = side;
        }

        /// <summary>
        /// Initializes the human player, asks for a name and saves it
        /// </summary>
        public void init()
        {
            Console.WriteLine("Please give me a name:");
            name = Console.ReadLine();
            Console.WriteLine("Username is: " + name);
        }

        /// <summary>
        /// Makes a move on a given board.
        /// </summary>
        /// <param name="board">The actual board which is used to decide state</param>
        public void makeMove(Board board)
        {
            Console.WriteLine(name + ", which pit do you want to take from? (1-" + board.getPitCount() + ")");

            if (board.move(side, Convert.ToInt32(Console.ReadLine()) - 1))
            {
                Console.WriteLine(board);
                makeMove(board);
            }
        }

        public override string ToString()
        {
            return "Player name is: " + this.name + ", Player is: " + this.side.ToString();
        }
    }
}

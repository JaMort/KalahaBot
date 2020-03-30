using System;
using System.Collections.Generic;
using System.Text;

namespace KalahaBot
{
    class HumanPlayer : IPlayer
    {


        public string name { get; set; }
        private bool isNorth { get; set; } 

        /// <summary>
        /// Constructor for HumanPlayer.
        /// </summary>
        /// <param name="isNorth">a boolean specifying if the player is north side or not.</param>
        public HumanPlayer(bool isNorth)
        {
            this.isNorth = isNorth;
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
        /// Makes a move on a given board
        /// </summary>
        /// <param name="board">Returns the given board</param>
        public void makeMove(Board board)
        {
            Console.WriteLine(name + ", which pit do you want to take from? (1-" + board.PitCount + ")");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (isNorth)
            {
                if (board.move(Side.NORTH, choice - 1))
                {
                    Console.WriteLine(board);
                    makeMove(board);
                };
            }
            else
            {
                if (board.move(Side.SOUTH, choice - 1))
                {
                    Console.WriteLine(board);
                    makeMove(board);
                };
            }
            
        }

        public override string ToString()
        {
            return "Player name is: " + this.name + ", Player is north: " + this.isNorth;
        }
    }
}

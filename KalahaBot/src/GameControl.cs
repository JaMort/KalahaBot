using System;

namespace KalahaBot
{
    public enum Side
    {
        NORTH, SOUTH, NONE
    }

    public class GameControl
    {
        IPlayer player1, player2;
        private Board board;
        public GameControl(Board board) 
        {
            this.board = board;
        }

        /// <summary>
        /// Initializes and starts the game
        /// </summary>
        public void init()
        {
            Console.WriteLine("Is the north player a human? (y/n)");
            string northPlayerPrompt = Console.ReadLine();
            while (true) {
                if (northPlayerPrompt.Equals("y"))
                {
                    player2 = new HumanPlayer(true);
                    break;
                }
                else if (northPlayerPrompt.Equals("n"))
                {
                    player2 = new AIPlayer(board, true);
                    break;
                }
                else
                {
                    Console.WriteLine("Please write either y or n");
                }
            }
            player2.init();
            Console.WriteLine("Is the south player a human? (y/n)");
            string southPlayerPrompt = Console.ReadLine();
            while (true) {
                if (southPlayerPrompt.Equals("y"))
                {
                    player1 = new HumanPlayer(false);
                    break;
                }
                else if (southPlayerPrompt.Equals("n"))
                {
                    player1 = new AIPlayer(board, false);
                    break;
                }
                else
                {
                    Console.WriteLine("Please write either y or n");
                }
            }
            player1.init();
            IPlayer currentPlayer = player1;
            while (true)
            {
                Console.WriteLine("Current player: " + currentPlayer.name);
                Console.WriteLine(this.board);
                currentPlayer.makeMove(this.board);
                if(board.getKalaha(Side.NORTH) > (board.PitCount * board.getInitialBalls()))
                {
                    Console.WriteLine(player2.name + " wins");
                    break;
                }
                if (board.getKalaha(Side.SOUTH) > (board.PitCount * board.getInitialBalls()))
                {
                    Console.WriteLine(player1.name + " wins");
                    break;
                }
                if(board.getKalaha(Side.SOUTH) == (board.PitCount*board.getInitialBalls()) && board.getKalaha(Side.NORTH) == (board.PitCount * board.getInitialBalls()))
                {
                    Console.WriteLine("issa tie");
                    break;
                }
                if (currentPlayer.Equals(player1))
                    currentPlayer = player2;
                else
                    currentPlayer = player1;
            }
        }
    }
}
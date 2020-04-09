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

        public void init()
        {
            Console.WriteLine("Is the north player a human? (y/n)");
            string northPlayerPrompt = Console.ReadLine();
            while (true) {
                if (northPlayerPrompt.Equals("y"))
                {
                    player2 = new HumanPlayer(Side.NORTH);
                    player2.init();
                
                    break;
                }
                else if (northPlayerPrompt.Equals("n"))
                {
                    player2 = new Agent(Side.NORTH);
                    player2.init();
                    break;
                }
                else
                {
                    Console.WriteLine("Please write either y or n");
                }
            }
            Console.WriteLine("Is the south player a human? (y/n)");
            string southPlayerPrompt = Console.ReadLine();
            while (true) {
                if (southPlayerPrompt.Equals("y"))
                {
                    player1 = new HumanPlayer(Side.SOUTH);
                    player1.init();
                    break;
                }
                else if (southPlayerPrompt.Equals("n"))
                {
                    player1 = new Agent(Side.SOUTH);
                    player1.init();
                    break;
                }
                else
                {
                    Console.WriteLine("Please write either y or n");
                }
            }
            
            IPlayer currentPlayer = player1;
            while (true)
            {
                Console.WriteLine("Current player: " + currentPlayer.name);
                Console.WriteLine(this.board);
                currentPlayer.makeMove(this.board);
                if(board.getKalaha(Side.NORTH) > (board.getPitCount() * board.getInitialBalls()))
                {
                    Console.WriteLine(player2.name + " wins");
                    break;
                }
                if (board.getKalaha(Side.SOUTH) > (board.getPitCount() * board.getInitialBalls()))
                {
                    Console.WriteLine(player1.name + " wins");
                    break;
                }
                if(board.getKalaha(Side.SOUTH) == (board.getPitCount()*board.getInitialBalls()) && board.getKalaha(Side.NORTH) == (board.getPitCount() * board.getInitialBalls()))
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
using System;
using System.Collections.Generic;
using System.Text;

namespace KalahaBot
{
    class AIPlayer : IPlayer
    {
        public string name { get; set; }
        private Board board;
        private Board boardCopy;
        private bool isNorth;
        private int states = 0;
        private int[] path = new int[13];

        public AIPlayer(Board board, bool isNorth)
        {
            this.board = board;
            this.isNorth = isNorth;
        }

        /// <summary>
        /// Initializes the AI player, gives it a name and creates a copy of the boardstate.
        /// </summary>
        public void init()
        {
            CopyBoard();
            name = "AIPlayer";
        }

        public void makeMove(Board board)
        {
            states = 0;
            int choice;
            childState choicestate = minimax(new childState(board, Int32.MaxValue, board), 12, Int32.MinValue, Int32.MaxValue, true, isNorth);
            choice = choicestate.index;
            //Console.WriteLine("path: " + string.Join(", ", path));
            if (choicestate.value == Int32.MaxValue - 1 || choicestate.value == Int32.MinValue + 1)
            {
                choice = 0;
            }
            Console.WriteLine("AI choice is: " + (choice + 1) + ", amount of states expanded: " + states + ", expected value: " + choicestate.value) ;
            if (isNorth)
            {
                if (board.move(Side.NORTH, choice))
                {
                    Console.WriteLine(board);
                    makeMove(board);
                };
            }
            else
            {
                if (board.move(Side.SOUTH, choice))
                {
                    Console.WriteLine(board);
                    makeMove(board);
                };
            }
        }

        /// <summary>
        /// creates a copy of the boardstate
        /// </summary>
        private void CopyBoard()
        {
            this.boardCopy = new Board(this.board);
        }

        private childState[] ExpandChildren(childState board, bool isNorth)
        {
            childState[] children = new childState[board.state.PitCount];
            states += board.state.PitCount;
            for (int i = 0; i < board.state.PitCount; i++)
            {
                children[i] = new childState(new Board(board.state), i, new Board(board.state));
            }
            foreach(childState child in children)
            {
                if (isNorth)
                {
                    child.state.move(Side.NORTH, child.index);
                }
                else
                {
                    child.state.move(Side.SOUTH, child.index);
                }
            }
            return children;
        }

        private int ValueState(Board state, bool isNorth)
        {
            int value = 0;
            if (isNorth)
            {
                value = state.getKalaha(Side.NORTH) - state.getKalaha(Side.SOUTH);
            }
            else
            {
                value = state.getKalaha(Side.SOUTH) - state.getKalaha(Side.NORTH);
            }
            return value;
        }

        public childState minimax(childState position, int depth, int alpha, int beta, bool maximizingPlayer, bool isNorth)
        {
            if (depth == 0)
            {
                path[depth] = position.index;
                return position;
            }
            childState[] children = ExpandChildren(position, isNorth);
            if (maximizingPlayer)
            {
                childState returnPos = new childState(board, 0, board);
                int maxEval = Int32.MinValue;
                foreach (childState child in children)
                {
                    if (child.parent.getSide(isNorth)[child.index] != 0)
                    {
                        childState eval;
                        if (child.state.retry)
                        {
                            eval = minimax(child, depth - 1, alpha, beta, true, isNorth);
                        }
                        else
                        {
                            eval = minimax(child, depth - 1, alpha, beta, false, !isNorth);
                        }
                        eval.value = ValueState(eval.state, isNorth);
                        if (eval.value > maxEval)
                        {
                            returnPos = eval;
                            returnPos.index = child.index;
                            maxEval = eval.value;
                        }
                        alpha = Math.Max(alpha, eval.value);
                        if (beta <= alpha)
                        {
                            break;
                        }
                        
                    }                   
                }
                return returnPos;
            }
            else
            {
                childState returnPos = new childState(board, 0, board);
                int minEval = Int32.MaxValue;
                foreach (childState child in children)
                {
                    if (child.parent.getSide(isNorth)[child.index] != 0)
                    {
                        childState eval;
                        if (child.state.retry)
                        {
                            eval = minimax(child, depth - 1, alpha, beta, false, isNorth);
                        }
                        else
                        {
                            eval = minimax(child, depth - 1, alpha, beta, true, !isNorth);
                        }
                        eval.value = ValueState(eval.state, isNorth);
                        if (eval.value < minEval)
                        {
                            returnPos = eval;
                            returnPos.index = child.index;
                            minEval = eval.value;
                        }
                        beta = Math.Min(beta, eval.value);
                        if (beta <= alpha)
                        {
                            break;
                        }
                        
                    }   
                }
                return returnPos;
            }
        }
    }

    public struct childState {
        public childState(Board state, int index, Board parent)
        {
            this.state = state;
            this.index = index;
            this.value = Int32.MinValue;
            this.parent = parent;
        }

        public Board state { get; }
        public Board parent { get; }
        public int index { get; set; }
        public int value { get; set; }
    }
}

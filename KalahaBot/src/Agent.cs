using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KalahaBot
{
    public class Node
    {
        public int value;
        public int takePrev;
        public int takeNext;
        public Board board;
        public bool isMaximizing;
        public Side whoTurn;
        public Node parent = null;
        public List<Node> children;

        public Node() {}

        public Node(Board board, Side side)
        {
            this.board = new Board(board);
            this.whoTurn = side;
            if (side == Side.NORTH)
                this.isMaximizing = true;
            else
                this.isMaximizing = false;
        }

        public void expand()
        {
            // Create the List
            this.children = new List<Node>(this.board.getPitCount());

            // Make move for every pit
            for (int takePos=0; takePos < board.getPitCount(); takePos++)
            {
                if (isExpandable(board, takePos))
                {
                    // Make move
                    Board nBoard    = new Board(board);
                    bool retry      = nBoard.move(whoTurn, takePos);

                    // Create node
                    Node nNode      = new Node();
                    nNode.parent    = this;
                    nNode.takePrev  = takePos;
                    nNode.board     = nBoard;
                    nNode.isMaximizing  = (retry) ? this.isMaximizing : !this.isMaximizing;
                    switch (whoTurn)
                    {
                        case Side.NORTH:
                            nNode.whoTurn = (retry) ? Side.NORTH: Side.SOUTH;
                            break;

                        case Side.SOUTH:
                            nNode.whoTurn = (retry) ? Side.SOUTH: Side.NORTH;
                            break;
                    }
                    
                    // Add the new node
                    this.children.Add(nNode);
                }
            }
        }

        private bool isExpandable(Board board, int takePos)
        {
            return board.getPits(whoTurn)[takePos] > 0;
        }

        public override string ToString()
        {
            return string.Format("{0}\nValue: {1}, Prev Take Index: {2}, Next Take Index: {5}, Maximizing: {3}, Turn: {4}\n", board, this.value, this.takePrev, this.isMaximizing, this.whoTurn, this.takeNext);
        }
    }

    public class Agent : IPlayer
    {
        public string name { get; set; }
        private Side side;
        private long statesExpanded;

        public Agent(Side side) { this.side = side; }

        public void init()
        {
            Console.WriteLine("What should the AI be called?");
            this.name = Console.ReadLine();
        }

        public void makeMove(Board board)
        {
            Stopwatch sw = new Stopwatch();
            TimeSpan timeSpan;
            bool retry = false;
            do 
            {
                // Reset state counter
                statesExpanded = 0;

                // Create node/state
                Node currState = new Node(board, this.side);

                // Start stopwatch
                sw.Start();

                // Run minimax
                minimax(currState, 10, Int32.MinValue, Int32.MaxValue);

                // Stop stopwatch and retrieve TimeSpan
                sw.Stop();
                timeSpan = sw.Elapsed;
                sw.Reset();

                // Make move
                Double millis = timeSpan.TotalMilliseconds;
                Console.WriteLine(string.Format("{0} takes from pit {1} - Searched {2} states in {3} ms - {4} ms/state\n", this.name, currState.takeNext+1, this.statesExpanded, millis, millis/statesExpanded));
                retry = board.move(this.side, currState.takeNext);
                if (retry)
                    Console.WriteLine(board);
            }
            while(retry);

        }

        /// <summary>
        /// Minimax with pruning.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        private Node minimax(Node state, int depth, int alpha, int beta)
        {
            // Update states expanded
            statesExpanded++;

            // Terminating condition
            if (depth == 0 || state.board.isGameOver())
            {
                state.value = valueState(state);
                return state;
            }

            // Expand nodes
            state.expand();

            // Find best
            int bestValue; Node bestNode = new Node();
            if (state.isMaximizing)
            {
                bestValue = Int32.MinValue;
                foreach (Node node in state.children)
                {
                    Node eval = minimax(node, depth-1, alpha, beta);
                    if (eval.value > bestValue)
                    {
                        bestValue   = eval.value;
                        bestNode    = eval;
                    }
                    alpha = Math.Max(alpha, bestValue);

                    // Prune
                    if (beta <= alpha)
                        break;
                }
            }
            else
            {
                bestValue = Int32.MaxValue;
                foreach (Node node in state.children)
                {
                    Node eval = minimax(node, depth-1, alpha, beta);
                    if (eval.value < bestValue)
                    {
                        bestValue   = eval.value;
                        bestNode    = eval;
                    }
                    beta = Math.Min(beta, bestValue);

                    // Prune
                    if (beta <= alpha)
                        break;
                }
            }

            // Update value and return
            state.value = bestNode.value;
            state.takeNext = bestNode.takePrev;
            return state;
        }

        /// <summary>
        /// Minimax algorithm without pruning.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        private Node minimax(Node state, int depth)
        {
            // Update states expanded
            statesExpanded++;

            // Terminating condition
            if (depth == 0 || state.board.isGameOver())
            {
                state.value = valueState(state);
                return state;
            }

            // Expand nodes
            state.expand();

            // Find best
            int bestValue; Node bestNode = new Node();
            if (state.isMaximizing)
            {
                bestValue = Int32.MinValue;
                foreach (Node node in state.children)
                {
                    Node eval = minimax(node, depth-1);
                    if (eval.value > bestValue)
                    {
                        bestValue   = eval.value;
                        bestNode    = eval;
                    }
                }
            }
            else
            {
                bestValue = Int32.MaxValue;
                foreach (Node node in state.children)
                {
                    Node eval = minimax(node, depth-1);
                    if (eval.value < bestValue)
                    {
                        bestValue   = eval.value;
                        bestNode    = eval;
                    }
                }
            }

            // Update value and return
            state.value = bestNode.value;
            state.takeNext = bestNode.takePrev;
            return state;
        }

        private int valueState(Node node)
        {
            Board board = node.board;
            return board.getKalaha(Side.NORTH) - board.getKalaha(Side.SOUTH);
        }

        /////////////////////////////////// TEST SECTION! ///////////////////////////////////

        public Node testMinimax(Node state, int depth, int alpha, int beta)
        {
           return minimax(state, depth, alpha, beta);
        }
    }
}
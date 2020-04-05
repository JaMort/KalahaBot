using System;

namespace KalahaBot
{
    public struct State
    {
        public Board board;
        public int value;
        public int takePosition;
        public bool retry;

        public State(Board board)
        {
            this.board = board;
            this.value = Int32.MinValue;
            this.takePosition = -1;
            this.retry = false;
        }

        public State(Board board, int takePosition, bool retry)
        {
            this.board = board;
            this.takePosition = takePosition;
            this.value = Int32.MinValue;
            this.retry = retry;
        }
    }

    public class Agent : IPlayer
    {
        private const int RETRY_MULT = 10;
        private const int KALAHA_MULT = 1;
        public string name { get; set; }

        public void init()
        {

        }

        public void makeMove(Board board)
        {

        }

        private State minimax(State state, Side maximizingPlayer, int depth)
        {
            State[] states;
            State best = new State(state.board); // Dummy

            // Terminating condition
            if (depth == 0)
                return state;

            // Expand states
            states = expand(state, maximizingPlayer);

            // Maximize for correct player
            switch (maximizingPlayer)
            {
                case Side.NORTH:
                    if (state.retry)
                    {
                        foreach (State currState in states)
                        {
                            // Retrieve the best state
                            State nState = minimax(currState, maximizingPlayer, depth-1);

                            // Value the state
                            valueState(nState, maximizingPlayer);

                            // Check it
                            if (nState.value > best.value)
                                best = nState;
                        }
                    }
                    else
                    {
                        foreach (State currState in states)
                        {
                            // Retrieve the best state
                            State nState = minimax(currState, Side.SOUTH, depth-1);

                            // Value the state
                            valueState(nState, maximizingPlayer);

                            // Check it
                            if (nState.value > best.value)
                                best = nState;
                        }
                    }
                    break;

                case Side.SOUTH:
                    if (state.retry)
                        {
                            foreach (State currState in states)
                            {
                                // Retrieve the best state
                                State nState = minimax(currState, maximizingPlayer, depth-1);

                                // Value the state
                                valueState(nState, maximizingPlayer);

                                // Check it
                                if (nState.value > best.value)
                                    best = nState;
                            }
                        }
                        else
                        {
                            foreach (State currState in states)
                            {
                                // Retrieve the best state
                                State nState = minimax(currState, Side.NORTH, depth-1);

                                // Value the state
                                valueState(nState, maximizingPlayer);

                                // Check it
                                if (nState.value > best.value)
                                    best = nState;
                            }
                        }
                    break;

                default:
                    Console.WriteLine("ERROR: Hit default case in Minimax switch!");
                    break;
            }

            return best;
        }

        private State[] expand(State state, Side maximizingPlayer)
        {
            int pitCount = state.board.getPitCount();
            State[] nStates = new State[pitCount];

            // For every possible move make a new state
            bool retry;
            for (int takePos=0; takePos < pitCount; takePos++)
            {
                // Copy board
                Board nBoard = new Board(state.board);

                // Make move
                retry = nBoard.move(maximizingPlayer, takePos);

                // Create the new state
                nStates[takePos] = new State(nBoard, takePos, retry);
            }

            return nStates;
        }

        private void valueState(State state, Side maximizingPlayer)
        {
            state.value = state.board.getKalaha(maximizingPlayer) * KALAHA_MULT;
            /*
            if (state.retry)
                state.value *= RETRY_MULT;
                */
        }

        /////////////////////////////////// TEST SECTION! ///////////////////////////////////

        /// <summary>
        /// REMOVE!
        /// </summary>
        /// <param name="state"></param>
        /// <param name="maximizingPlayer"></param>
        /// <returns></returns>
        public State[] testExpand(State state, Side maximizingPlayer)
        {
            return expand(state, maximizingPlayer);
        }

        /// <summary>
        /// REMOVE!
        /// </summary>
        /// <param name="state"></param>
        /// <param name="maximizingPlayer"></param>
        /// <returns></returns>
        public State testValueState(State state, Side maximizingPlayer)
        {
            valueState(state, maximizingPlayer);
            return state;
        }

        /// <summary>
        /// REMOVE!
        /// </summary>
        /// <param name="state"></param>
        /// <param name="maximizingPlayer"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public State testMinimax(State state, Side maximizingPlayer, int depth)
        {
            return minimax(state, maximizingPlayer, depth);
        }
    }
}
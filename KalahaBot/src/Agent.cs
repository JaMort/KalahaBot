using System;

namespace KalahaBot
{
    public struct State
    {
        public Board board;
        public Side maximizingPlayer;
        public int value;
        public int takePosition;

        public State(Board board, Side maximizingPlayer)
        {
            this.board = board;
            this.maximizingPlayer = maximizingPlayer;
            this.value = Int32.MinValue;
            this.takePosition = -1;
        }

        public State(Board board, Side maximizingPlayer, int takePosition)
        {
            this.board = board;
            this.maximizingPlayer = maximizingPlayer;
            this.takePosition = takePosition;
            this.value = Int32.MinValue;
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

        private int minimax(Board board)
        {
            return -1;
        }

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

        private State[] expand(State state, Side maximizingPlayer)
        {
            int pitCount = state.board.getPitCount();
            State[] nStates = new State[pitCount];

            // For every possible move make a new state
            for (int takePos=0; takePos < pitCount; takePos++)
            {
                // Copy board
                Board nBoard = new Board(state.board);

                // Make move
                nBoard.move(maximizingPlayer, takePos);

                // Create the new state
                nStates[takePos] = new State(nBoard, maximizingPlayer, takePos);
            }

            return nStates;
        }

        private void valueState(State state)
        {
            state.value = state.board.getKalaha(state.maximizingPlayer) * KALAHA_MULT;
            /*
            if (state.retry)
                state.value *= RETRY_MULT;
                */
        }
    }
}
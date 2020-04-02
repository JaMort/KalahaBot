using System;
using System.Text;

namespace KalahaBot
{
    /// <summary>
    /// 
    /// </summary>
    public class Board
    {
        // The two sides, where last index is their kalaha and every other index is a pit on the given side.
        private int[] pits;
        private int indexNorthKalaha, indexSouthKalaha;
        private int pitCount, initialBalls;

        /// <summary>
        /// Creates a board with standard settings.
        /// </summary>
        public Board()
        {
            this.pitCount = 6; this.initialBalls = 6;
            initPits();
        }

        /// <summary>
        /// Makes the new Board a complete copy of "boardToCopy".
        /// </summary>
        /// <param name="boardToCopy">Board to make a copy of</param>
        public Board(Board boardToCopy)
        {
            // Copy variables
            this.pitCount           = boardToCopy.getNorthSide().Length;
            this.initialBalls       = boardToCopy.getInitialBalls();
            this.indexNorthKalaha   = this.pitCount;
            this.indexSouthKalaha   = this.pitCount*2+1;

            // Copy array
            this.pits = new int[this.pitCount * 2 + 2];
            int[] copyPits = boardToCopy.getPits();
            for (int i=0; i < this.pits.Length; i++)
                this.pits[i] = copyPits[i];
        }

        /// <summary>
        /// Creates a board with the given settings.
        /// </summary>
        /// <param name="pitCount">Amount of pits in each side</param>
        /// <param name="initialBalls">Amount of balls initially in the pits</param>
        public Board(int pitCount, int initialBalls)
        {
            this.pitCount = pitCount; this.initialBalls = initialBalls;
            initPits();
        }

        /// <summary>
        /// Makes a new Board with the state as specified by the parameters.
        /// </summary>
        /// <param name="north">int[] of all North pits</param>
        /// <param name="northKalaha">#Balls in North kalaha pit</param>
        /// <param name="south">nt[] of all South pits</param>
        /// <param name="southKalaha">#Balls in South kalaha pit</param>
        public Board(int[] north, int northKalaha, int[] south, int southKalaha)
        {
            int length;

            // Check that north and south is even length
            if (north.Length != south.Length)
                throw new System.Exception(string.Format("North and South isn't same length! North: {0}, South: {1}", north.Length, south.Length));

            length = north.Length;

            // Init pits
            this.pitCount = length; this.initialBalls = -1;
            this.pits = new int[this.pitCount * 2 + 2];

            // Copy board
            for (int curr0=0, curr1=length+1; curr0 <= length; curr0++, curr1++)
            {
                if (curr0 != length)
                {
                    this.pits[curr0] = north[curr0];
                    this.pits[curr1] = south[curr0];
                }

                else
                {
                    // Set kalaha and indexes
                    this.pits[curr0] = northKalaha;
                    this.indexNorthKalaha = curr0;

                    this.pits[curr1] = southKalaha;
                    this.indexSouthKalaha = curr1;
                }
            }
        }

        /// <summary>
        /// This function makes a move for the given player. It takes arguments where the first is which side it should 
        /// start from, and the second is which pit to start from.
        /// </summary>
        /// <param name="side">North or South</param>
        /// <param name="takePosition">Which pit to take balls from</param>
        public bool move(Side side, int takePosition)
        {
            switch (side)
            {
                case Side.NORTH:
                    return makeMove(takePosition, indexSouthKalaha);

                case Side.SOUTH:
                    return makeMove(takePosition+pitCount+1, indexNorthKalaha);

                default:
                    return false;
            }
        }

        public int getKalaha(Side side)
        {
            switch (side)
            {
                case Side.NORTH:
                    return pits[indexNorthKalaha];
                
                case Side.SOUTH:
                    return pits[indexSouthKalaha];

                default:
                    return -1;
            }
        }

        public int getPitCount() { return this.pitCount; }

        public int[] getNorthSide()
        {
            int[] res = new int[this.pitCount];

            for (int i=0; i < res.Length; i++)
                res[i] = this.pits[i];

            return res;
        }

        public int[] getSouthSide()
        {
            int[] res = new int[this.pitCount];

            for (int i=0, j=this.pitCount+1; i < res.Length; i++, j++)
                res[i] = this.pits[j];

            return res;
        }

        public int getInitialBalls() { return this.initialBalls; }

        public int[] getPits() { return this.pits; }

        public override string ToString()
        {
            // Write variables
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Initial Balls: {0}\n", initialBalls);
            sb.AppendFormat("Pit Count: {0}\n\n", pitCount);

            // Show North side
            int start = pitCount-1, end = 0;
            sb.Append("  ");
            for (int i = start; i >= end; i--)
            {
                sb.Append(i+1);
                if (i != end)
                    sb.Append(" ");
            }
            sb.Append("\n\n");
            sb.Append("  ");
            for (int i=start; i >= end; i--)
            {
                sb.Append(pits[i]);
                if (i != end)
                    sb.Append(" ");
            }

            // Make divider
            sb.Append("\n");
            sb.Append(pits[indexNorthKalaha] + " ");
            start = 1; end = pitCount;
            for (int i=start; i <= end; i++)
            {
                sb.Append("- ");
            }
            sb.Append(pits[indexSouthKalaha]);

            // Show South side
            sb.Append("\n");
            sb.Append("  ");
            start = pitCount+1; end = pitCount*2;
            for (int i=start; i <= end; i++)
            {
                sb.Append(pits[i]);
                if (i != end)
                    sb.Append(" ");
            }
            sb.Append("\n\n");
            sb.Append("  ");
            for(int i = start; i <= end; i++)
            {
                sb.Append(i - pitCount);
                if (i != end)
                    sb.Append(" ");
            }
            sb.Append("\n");
            // Return the created String
            return sb.ToString();
        }

        private bool makeMove(int start, int indexAvoid)
        {
            int currPos = start, hand;
            while (true)
            {
                // Take balls from pit
                hand = pits[currPos]; pits[currPos] = 0;

                // Distribute balls
                while (hand != 0)
                {
                    currPos++;
                    if (currPos != indexAvoid)
                    {
                        pits[currPos]++; hand--;
                    }

                    // Check if end is reached
                    if (currPos == pits.GetUpperBound(0) && hand != 0)
                        currPos = 0;
                }

                // Check if turn shall continue
                if (currPos == indexNorthKalaha || currPos == indexSouthKalaha) {
                    bool continueTurn = false;
                    if (currPos == indexNorthKalaha)
                    {
                        foreach(int pit in getNorthSide())
                        {
                            if(pit != 0)
                            {
                                continueTurn = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach(int pit in getSouthSide())
                        {
                            if(pit != 0)
                            {
                                continueTurn = true;
                                break;
                            }
                        }
                    }
                    return continueTurn;
                }
                else if (pits[currPos] <= 1)
                    return false;
            }
        }

        private void initPits()
        {
            int count = pitCount*2 + 2;

            // Make pits and kalaha
            pits = new int[count];

            // Puts balls in every pit
            for (int i=0; i < count; i++)
            {
                if (i == pitCount || i == (count - 1))
                    pits[i] = 0;
                else
                    pits[i] = initialBalls;
            }

            // Kalaha indexes
            indexNorthKalaha = pitCount;
            indexSouthKalaha = count-1;
        }
    }
}
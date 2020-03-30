namespace KalahaBot
{
    /// <summary>
    /// This is an interface all players of the game should implement! 
    /// If a human is the player then makeMove() should ask the player what to do. 
    /// Otherwise if it's an AI agent, then makeMove() should do some AI stuff.
    /// </summary>
    public interface IPlayer
    { 
         string name { get; set; }

        /// <summary>
        /// For initialization of the player. This could be asking for their name if it's a human.
        /// </summary>
        void init();

        void makeMove(Board board);
    }
}
namespace PetGame.Common
{
    /// <summary>
    /// A unique identifier for each error that the application can produce, for
    /// dependent services to write code that can detect these errors. 
    /// </summary>
    public enum ErrorId
    {
        // @NOTE Suggested format: [Subject]_[Problem]_[Reason]
        // @NOTE Renaming: You cannot rename these without first checking that dependent
        //  code e.g. the frontend client isn't also updated + any logging / metrics / alerts.
        //  Ideally you would just never rename these.
        
        // Taking Tree
        /// <summary>
        /// User attempted to donate an item to the Taking Tree, but the GUID passed
        /// does not match anything in the player's inventory.
        /// </summary>
        TakingTree_UserCannotDonate_UserDoesNotHaveItem,
        /// <summary>
        /// User attempted to claim an item from the Taking Tree, but the GUID passed
        /// does not match anything in the Taking Tree's inventory.
        /// </summary>
        TakingTree_UserCannotClaim_ItemDoesNotExist,

        // Leaderboard
        /// <summary>
        /// User attempted to submit a new leaderboard entry, but they
        /// have already submitted the maximum amount of submissions for the current
        /// day (in their local time)
        /// </summary>
        Leaderboard_UserCannotSaveEntry_MaximumSubmissionsPerDayReached,
    }
}

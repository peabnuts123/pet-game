using System;
using System.Threading.Tasks;

namespace PetGame.Business
{
    public interface IGameService
    {
        /// <summary>
        /// Test whether a given game ID is a valid game that exists
        /// </summary>
        /// <param name="gameId">ID of the game</param>
        /// <returns>Whether the game ID is a valid game that exists</returns>
        Task<bool> IsValidGameId(Guid gameId);
    }
}
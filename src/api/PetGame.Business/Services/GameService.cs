using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetGame.Data;

namespace PetGame.Business
{
    public class GameService : IGameService
    {
        private PetGameContext db;

        public GameService(PetGameContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Test whether a given game ID is a valid game that exists
        /// </summary>
        /// <param name="gameId">ID of the game</param>
        /// <returns>Whether the game ID is a valid game that exists</returns>
        public async Task<bool> IsValidGameId(Guid gameId)
        {
            return await this.db.Games.AnyAsync((Game game) => game.Id == gameId);
        }
    }
}
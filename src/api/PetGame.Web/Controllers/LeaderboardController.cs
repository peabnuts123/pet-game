using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetGame.Business;
using PetGame.Common;
using PetGame.Data;

namespace PetGame.Web
{
    [ApiController]
    [Route("api/leaderboard")]
    public class LeaderboardController : ControllerBase
    {
        private ILogger<LeaderboardController> logger;
        private ILeaderboardService leaderboardService;
        private IUserService userService;
        private IGameService gameService;

        public LeaderboardController(ILogger<LeaderboardController> logger, ILeaderboardService leaderboardService, IUserService userService, IGameService gameService)
        {
            this.logger = logger;
            this.leaderboardService = leaderboardService;
            this.userService = userService;
            this.gameService = gameService;
        }

        [HttpPost]
        [Route("scores/today")]
        [Authorize]
        public async Task<ActionResult<IList<LeaderboardEntry>>> GetEntriesForToday(LeaderboardTodayDto dto)
        {
            string userAuthId = HttpContext.User.GetSubject();
            User user = await this.userService.GetUserByAuthId(userAuthId);

            // VALIDATION
            //  - Game ID
            Guid gameId = dto.gameId;
            if (!this.gameService.IsValidGameId(gameId))
            {
                return BadRequest(new
                {
                    message = $"Unknown game ID: {gameId}",
                });
            }

            // - Timezone offset
            int timeZoneOffsetMinutes = dto.timeZoneOffsetMinutes;
            if (timeZoneOffsetMinutes < LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS * 60)
            {
                return BadRequest(new
                {
                    message = $"Likely invalid time zone offset: {timeZoneOffsetMinutes}. Minimum supported offset is UTC{LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS} ({LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS * 60})",
                });
            }
            else if (timeZoneOffsetMinutes > LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS * 60)
            {
                return BadRequest(new
                {
                    message = $"Likely invalid time zone offset: {timeZoneOffsetMinutes}. Maximum supported offset is UTC+{LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS} ({LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS * 60})",
                });
            }


            IList<LeaderboardEntry> entries = await this.leaderboardService.GetUserEntriesForDate(
                user.Id,
                dto.gameId,
                DateTimeUtility.GetLocalDateTimeNow(dto.timeZoneOffsetMinutes),
                dto.timeZoneOffsetMinutes
            );

            return Ok(entries);
        }

        [HttpPost]
        [Route("scores/date")]
        [Authorize]
        public async Task<ActionResult<IList<LeaderboardEntry>>> GetEntriesForDate(LeaderboardDateDto dto)
        {
            string userAuthId = HttpContext.User.GetSubject();
            User user = await this.userService.GetUserByAuthId(userAuthId);

            // VALIDATION
            //  - Game ID
            Guid gameId = dto.gameId;
            if (!this.gameService.IsValidGameId(gameId))
            {
                return BadRequest(new
                {
                    message = $"Unknown game ID: {gameId}",
                });
            }

            // - Timezone offset
            int timeZoneOffsetMinutes = dto.timeZoneOffsetMinutes;
            if (timeZoneOffsetMinutes < LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS * 60)
            {
                return BadRequest(new
                {
                    message = $"Likely invalid time zone offset: {timeZoneOffsetMinutes}. Minimum supported offset is UTC{LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS} ({LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS * 60})",
                });
            }
            else if (timeZoneOffsetMinutes > LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS * 60)
            {
                return BadRequest(new
                {
                    message = $"Likely invalid time zone offset: {timeZoneOffsetMinutes}. Maximum supported offset is UTC+{LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS} ({LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS * 60})",
                });
            }


            IList<LeaderboardEntry> entries = await this.leaderboardService.GetUserEntriesForDate(
                user.Id,
                dto.gameId,
                dto.date,
                dto.timeZoneOffsetMinutes
            );

            return Ok(entries);
        }

        [HttpGet]
        [Route("game/{gameId}")]
        public async Task<ActionResult<IList<LeaderboardEntry>>> GetAllEntriesForGame(Guid gameId)
        {
            // VALIDATION
            if (!this.gameService.IsValidGameId(gameId))
            {
                return BadRequest(new
                {
                    message = $"Unknown game ID: {gameId}",
                });
            }

            return Ok(await this.leaderboardService.GetAllEntriesForGame(gameId));
        }

        [HttpPost]
        [Route("submit")]
        [Authorize]
        public async Task<ActionResult<LeaderboardEntry>> SaveEntry(LeaderboardEntryDto dto)
        {
            // VALIDATION
            //  - Game ID
            Guid gameId = dto.gameId;
            if (!this.gameService.IsValidGameId(gameId))
            {
                return BadRequest(new
                {
                    message = $"Unknown game ID: {gameId}",
                });
            }

            // - Score
            // @TODO validate score
            if (dto.score > 9999 || dto.score < 0)
            {
                return BadRequest(new
                {
                    message = "Stop trying to hack the leaderboard"
                });
            }

            // - Timezone offset
            int timeZoneOffsetMinutes = dto.timeZoneOffsetMinutes;
            if (timeZoneOffsetMinutes < LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS * 60)
            {
                return BadRequest(new
                {
                    message = $"Likely invalid time zone offset: {timeZoneOffsetMinutes}. Minimum supported offset is UTC{LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS} ({LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS * 60})",
                });
            }
            else if (timeZoneOffsetMinutes > LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS * 60)
            {
                return BadRequest(new
                {
                    message = $"Likely invalid time zone offset: {timeZoneOffsetMinutes}. Maximum supported offset is UTC+{LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS} ({LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS * 60})",
                });
            }


            string userAuthId = HttpContext.User.GetSubject();
            User user = await this.userService.GetUserByAuthId(userAuthId);

            try
            {
                LeaderboardEntry entry = await this.leaderboardService.SaveEntry(
                    user.Id,
                    gameId,
                    dto.score,
                    dto.timeZoneOffsetMinutes
                );

                return Ok(entry);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new
                {
                    message = e.Message,
                });
            }
        }
    }
}

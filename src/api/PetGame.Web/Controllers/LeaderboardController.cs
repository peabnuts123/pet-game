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
            if (!await this.gameService.IsValidGameId(dto.gameId.Value))
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.gameId)] = $"Unknown game ID: {dto.gameId.Value}",
                });
            }
            // - Timezone offset
            if (dto.timeZoneOffsetMinutes.Value < LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS * 60)
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.timeZoneOffsetMinutes)] = $"Likely invalid time zone offset: {dto.timeZoneOffsetMinutes.Value}. Minimum supported offset is UTC{LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS} ({LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS * 60})",
                });
            }
            else if (dto.timeZoneOffsetMinutes.Value > LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS * 60)
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.timeZoneOffsetMinutes)] = $"Likely invalid time zone offset: {dto.timeZoneOffsetMinutes.Value}. Maximum supported offset is UTC+{LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS} ({LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS * 60})",
                });
            }


            IList<LeaderboardEntry> entries = await this.leaderboardService.GetUserEntriesForDate(
                user.Id,
                dto.gameId.Value,
                DateTimeUtility.GetLocalDateTimeNow(dto.timeZoneOffsetMinutes.Value),
                dto.timeZoneOffsetMinutes.Value
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
            if (!await this.gameService.IsValidGameId(dto.gameId.Value))
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.gameId)] = $"Unknown game ID: {dto.gameId.Value}",
                });
            }
            // - Timezone offset
            if (dto.timeZoneOffsetMinutes.Value < LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS * 60)
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.timeZoneOffsetMinutes)] = $"Likely invalid time zone offset: {dto.timeZoneOffsetMinutes.Value}. Minimum supported offset is UTC{LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS} ({LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS * 60})",
                });
            }
            else if (dto.timeZoneOffsetMinutes.Value > LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS * 60)
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.timeZoneOffsetMinutes)] = $"Likely invalid time zone offset: {dto.timeZoneOffsetMinutes.Value}. Maximum supported offset is UTC+{LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS} ({LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS * 60})",
                });
            }


            IList<LeaderboardEntry> entries = await this.leaderboardService.GetUserEntriesForDate(
                user.Id,
                dto.gameId.Value,
                dto.date.Value,
                dto.timeZoneOffsetMinutes.Value
            );

            return Ok(entries);
        }

        [HttpPost]
        [Route("game")]
        public async Task<ActionResult<IList<LeaderboardEntry>>> GetTopEntriesForGame(LeaderboardTopGameEntriesDto dto)
        {
            // VALIDATION
            //  - Game ID
            if (!await this.gameService.IsValidGameId(dto.gameId.Value))
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.gameId)] = $"Unknown game ID: {dto.gameId}",
                });
            }
            //  - Top N
            if (dto.topN <= 0)
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.topN)] = $"Cannot be less than or equal to 0",
                });
            }
            else if (dto.topN > 1000)
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.topN)] = $"Cannot be greater than 1000",
                });
            }


            return Ok(await this.leaderboardService.GetTopEntriesForGame(dto.gameId.Value, dto.topN ?? 10));
        }

        [HttpPost]
        [Route("game/user")]
        public async Task<ActionResult<IList<LeaderboardEntry>>> GetTopUserEntriesForGame(LeaderboardTopUserGameEntriesDto dto)
        {
            // VALIDATION
            //  - Game ID
            if (!await this.gameService.IsValidGameId(dto.gameId.Value))
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.gameId)] = $"Unknown game ID: {dto.gameId}",
                });
            }
            //  - Top N
            if (dto.topN <= 0)
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.topN)] = $"Cannot be less than or equal to 0",
                });
            }
            else if (dto.topN > 1000)
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.topN)] = $"Cannot be greater than 1000",
                });
            }
            //  - User ID
            if (dto.userId.HasValue && !await this.userService.IsValidUserId(dto.userId.Value))
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.userId)] = $"Unknown user ID: {dto.userId.Value}",
                });
            }
            // Extract user ID from either the request body or the auth cookie
            // At least one must be present
            Guid userId;
            if (dto.userId.HasValue)
            {
                userId = dto.userId.Value;
            }
            else
            {
                // No user specified - default to user auth header 
                string userAuthId = HttpContext.User.GetSubject();

                if (string.IsNullOrEmpty(userAuthId))
                {
                    return ValidationError(new ValidationErrors
                    {
                        [nameof(dto.userId)] = $"Could not locate user ID. Either specify a `{nameof(dto.userId)}` parameter, or include a valid authentication cookie in the request",
                    });
                }
                else
                {
                    User user = await this.userService.GetUserByAuthId(userAuthId);
                    userId = user.Id;
                }
            }


            return Ok(await this.leaderboardService.GetTopUserEntriesForGame(userId, dto.gameId.Value, dto.topN ?? 10));
        }


        [HttpPost]
        [Route("submit")]
        [Authorize]
        public async Task<ActionResult<LeaderboardEntry>> SaveEntry(LeaderboardSaveEntryDto dto)
        {
            // VALIDATION
            //  - Game ID
            if (!await this.gameService.IsValidGameId(dto.gameId.Value))
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.gameId)] = $"Unknown game ID: {dto.gameId.Value}",
                });
            }
            // - Score
            // @TODO validate score
            if (dto.score.Value >= 9999 || dto.score.Value <= 0)
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.score)] = "Stop trying to hack the leaderboard"
                });
            }
            // - Timezone offset
            if (dto.timeZoneOffsetMinutes.Value < LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS * 60)
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.timeZoneOffsetMinutes)] = $"Likely invalid time zone offset: {dto.timeZoneOffsetMinutes.Value}. Minimum supported offset is UTC{LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS} ({LeaderboardEntry.MIN_TIME_ZONE_OFFSET_HOURS * 60})",
                });
            }
            else if (dto.timeZoneOffsetMinutes.Value > LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS * 60)
            {
                return ValidationError(new ValidationErrors
                {
                    [nameof(dto.timeZoneOffsetMinutes)] = $"Likely invalid time zone offset: {dto.timeZoneOffsetMinutes.Value}. Maximum supported offset is UTC+{LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS} ({LeaderboardEntry.MAX_TIME_ZONE_OFFSET_HOURS * 60})",
                });
            }


            string userAuthId = HttpContext.User.GetSubject();
            User user = await this.userService.GetUserByAuthId(userAuthId);

            try
            {
                LeaderboardEntry entry = await this.leaderboardService.SaveEntry(
                    user.Id,
                    dto.gameId.Value,
                    dto.score.Value,
                    dto.timeZoneOffsetMinutes.Value
                );

                return Ok(entry);
            }
            catch (LeaderboardService.MaxSubmissionsPerDayReachedException e)
            {
                return BadRequest(new ApiError(
                    errors: new GenericError(e)
                ));
            }
        }
    }
}

import ApiModel from "./base/ApiModel";
import UserProfile, { UserProfileDto } from "./UserProfile";

export interface LeaderboardEntryDto {
  id: string;
  score: number;
  entryTimeUTC: string;
  user: UserProfileDto;
}

export interface LeaderboardDateRequestDto {
  date: string;
  gameId: string;
  timeZoneOffsetMinutes: number;
}

export interface LeaderboardTodayRequestDto {
  gameId: string;
  timeZoneOffsetMinutes: number;
}

class LeaderboardEntry extends ApiModel<LeaderboardEntryDto> {
  public readonly id: string;
  public readonly score: number;
  public readonly entryTimeUTC: Date;
  public readonly user: UserProfile;

  public constructor(dto: LeaderboardEntryDto) {
    super();

    this.id = dto.id;
    this.score = dto.score;
    this.entryTimeUTC = new Date(dto.entryTimeUTC);
    this.user = new UserProfile(dto.user);
  }

  public toDto(): LeaderboardEntryDto {
    // @NOTE there is currently no use-case for this.
    //  It is is not needed. No APIs take this DTO as a request.
    //  It just serialises the model back to the same format 
    //  as what was received from the API
    const dto: LeaderboardEntryDto = {
      id: this.id,
      score: this.score,
      entryTimeUTC: this.entryTimeUTC.toISOString(),
      user: this.user.toDto(),
    };

    return dto;
  }
}

export default LeaderboardEntry;

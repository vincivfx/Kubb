namespace KubbAdminAPI.Models.ResponseModels.Challenge;

public class GetInfoResponse
{
    public List<InfoResponseTeam> Teams { get; set; }

    public class InfoResponseTeam(Models.Team team)
    {
        public Guid TeamId { get; set; } = team.TeamId;
        public string TeamName { get; set; } = team.TeamName;
        public TeamType TeamType { get; set; } = team.TeamType;
        public string OptionString { get; set; } = team.OptionString;
    }

    public class GetInfoChallengeItem(Models.Challenge challenge)
    {
        public Guid ChallengeId { get; set; } = challenge.ChallengeId;
        public string Name { get; set; } = challenge.Name;
        public DateTime? StartTime { get; set; } = challenge.StartTime;
        public DateTime? EndTime { get; set; } = challenge.EndTime;
        public int Questions { get; set; } = challenge.Questions.Count;
        public int MaxTeamPerUser { get; set; } = challenge.MaxTeamPerUser;
        public RunningChallengeStatus RunningStatus { get; set; } = challenge.RunningStatus;
    }

    public GetInfoChallengeItem ChallengeSetup { get; set; }

    public GetInfoResponse(List<Models.Team> teams, Models.Challenge challenge)
    {
        Teams = [];
        foreach (var team in teams) Teams.Add(new InfoResponseTeam(team));
        ChallengeSetup = new GetInfoChallengeItem(challenge);
    }
}
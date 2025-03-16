namespace KubbAdminAPI.Models.ResponseModels.ChallengeAdmin;

public class ChallengeInfoResponse
{
    public ChallengeItem Challenge { get; set; }
    public List<Team> Teams { get; set; }
    public List<Participation> Participations { get; set; }

    public class Team(Models.Team team)
    {
        public Guid TeamId { get; set; } = team.TeamId;
        public string TeamName { get; set; } = team.TeamName;
        public Guid UserId { get; set; } = team.Administrator.UserId;
        public DateTime Created { get; set; } = team.Created;
    }
    public class Participation(Models.Participation participation)
    {
        public string UserName { get; set; } = participation.User.Name;
        public string UserSurname { get; set; } = participation.User.Surname;
        public string EmailAddress { get; set; } = participation.User.EmailAddress;
        public Guid UserId { get; set; } = participation.User.UserId;
        public DateTime Created { get; set; } = participation.Created;
    }

    public class ChallengeItem(Models.Challenge challenge)
    {
        public string Name { get; set; } = challenge.Name;
        public DateTime? StartTime { get; set; } = challenge.StartTime;
        public DateTime? EndTime { get; set; } = challenge.EndTime;
        public DateTime Created { get; set; } = challenge.Created;
        public List<string> Questions { get; set; } = challenge.Questions;
        public Models.ChallengeStatus Status { get; set; } = challenge.Status;
        public Models.RunningChallengeStatus RunningStatus { get; set; } = challenge.RunningStatus;
        public string AlgorithmSettings { get; set; } = challenge.AlgorithmSettings;
    }

    public static ChallengeInfoResponse Create(Models.Challenge challenge, List<Participation> participations,
        List<Team> teams)
    {
        return new ChallengeInfoResponse
        {
            Teams = teams,
            Participations = participations,
            Challenge = new ChallengeItem(challenge)
        };
    }

}
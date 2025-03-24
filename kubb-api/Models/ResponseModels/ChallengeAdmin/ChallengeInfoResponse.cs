using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KubbAdminAPI.Models.ResponseModels.ChallengeAdmin;

public class ChallengeInfoResponse(Models.Challenge challenge, List<ChallengeInfoResponse.Participation> participations, List<ChallengeInfoResponse.Team> teams)
{
    public ChallengeItem Challenge { get; set; } = new ChallengeItem(challenge);
    public List<Team> Teams { get; set; } = teams;
    public List<Participation> Participations { get; set; } = participations;

    public class Team(Models.Team team, Models.User user)
    {
        public Guid TeamId { get; set; } = team.TeamId;
        public string TeamName { get; set; } = team.TeamName;
        public Guid UserId { get; set; } = team.Administrator.UserId;
        public DateTime Created { get; set; } = team.Created;
        public string? OwnerName { get; set; } = user.UserId != team.Administrator.UserId ? team.Administrator.Name + " " + team.Administrator.Surname : null;
        public string? OwnerEmail { get; set; } = user.UserId != team.Administrator.UserId ? team.Administrator.EmailAddress : null;
    }
    public class Participation(Models.Participation participation)
    {
        public string Name { get; set; } = participation.User.Name;
        public string Surname { get; set; } = participation.User.Surname;
        public string EmailAddress { get; set; } = participation.User.EmailAddress;
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
        public int MaxTeamPerUser { get; set; } = challenge.MaxTeamPerUser;
    }

}
namespace KubbAdminAPI.Models.ResponseModels.Challenge;

public class GetCurrentParticipationsResponse(int count, List<GetCurrentParticipationsResponse.Challenge> challenges)
{
    public int Count { get; set; } = count;
    public List<Challenge> Challenges { get; set; } = challenges;


    public class Challenge(Models.Challenge challenge)
    {
        public Guid ChallengeId { get; set; } = challenge.ChallengeId;
        public string Name { get; set; } = challenge.Name;
        public DateTime? StartTime { get; set; } = challenge.StartTime;
        public DateTime? EndTime { get; set; } = challenge.EndTime;
        public ChallengeStatus Status { get; set; } = challenge.Status;
        public RunningChallengeStatus RunningStatus { get; set; } = challenge.RunningStatus;

    }
}
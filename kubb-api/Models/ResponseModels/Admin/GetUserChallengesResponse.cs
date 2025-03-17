using KubbAdminAPI.Models;

namespace KubbAdminAPI.Models.ResponseModels.Admin;

public class GetUserChallengesResponse(List<GetUserChallengesResponse.Challenge> challenges)
{
    public List<Challenge> Challenges = challenges;

    public class Challenge(Models.Challenge challenge)
    {
        public string Name = challenge.Name;
        public DateTime? StartTime = challenge.StartTime;
        public DateTime? EndTime = challenge.EndTime;
        public RunningChallengeStatus RunningStatus = challenge.RunningStatus;
        public ChallengeStatus Status = challenge.Status;
    }
}
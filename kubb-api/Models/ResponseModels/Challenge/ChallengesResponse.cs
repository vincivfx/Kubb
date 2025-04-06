namespace KubbAdminAPI.Models.ResponseModels.Home;

public class ChallengesResponse(List<ChallengeSingle> challenges, int count)
{
    public int Count { get; set; } = count;
    public List<ChallengeSingle> Challenges { get; set; } = challenges;

    
}
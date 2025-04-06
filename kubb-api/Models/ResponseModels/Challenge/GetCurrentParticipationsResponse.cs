namespace KubbAdminAPI.Models.ResponseModels.Challenge;

public class GetCurrentParticipationsResponse(int count, List<ChallengeSingle> challenges)
{
    public int Count { get; set; } = count;
    public List<ChallengeSingle> Challenges { get; set; } = challenges;

}
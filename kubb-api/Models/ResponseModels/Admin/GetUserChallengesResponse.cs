using KubbAdminAPI.Models;

namespace KubbAdminAPI.Models.ResponseModels.Admin;

public class GetUserChallengesResponse(List<ChallengeSingle> challenges)
{
    public List<ChallengeSingle> Challenges = challenges;
}
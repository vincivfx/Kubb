namespace KubbAdminAPI.Models.ResponseModels.Home;

public class ChallengeSetupResponse(Models.Challenge challenge)
{
    public string Name { get; set; } = challenge.Name;
    public DateTime? StartTime { get; set; } = challenge.StartTime;
    public DateTime? EndTime { get; set; } = challenge.EndTime;
    public string AlgorithmSettings { get; set; } = challenge.AlgorithmSettings;
}
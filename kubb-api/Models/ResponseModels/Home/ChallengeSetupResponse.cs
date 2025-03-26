namespace KubbAdminAPI.Models.ResponseModels.Home;

public class ChallengeSetupResponse(Models.Challenge challenge)
{
    public string Name { get; set; } = challenge.Name;
    public DateTime? StartTime { get; set; } = challenge.StartTime;
    public DateTime? EndTime { get; set; } = challenge.EndTime;
    public ChallengeStatus Status { get; set; } = challenge.Status;
    public RunningChallengeStatus RunningStatus { get; set; } = challenge.RunningStatus;
    public string AlgorithmSettings { get; set; } = challenge.AlgorithmSettings;
    public int BasePoints { get; set; } = challenge.BasePoints;
}
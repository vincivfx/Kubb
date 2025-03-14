namespace KubbAdminAPI.Models.ResponseModels.Challenge;

public class GetAnswersResponse(Dictionary<Guid, GetAnswersResponse.TeamAnswer> teamAnswer)
{

    public Dictionary<Guid, TeamAnswer> TeamAnswers { get; set; } = teamAnswer;

    public class Answer(Models.Answer answer)
    {
        public Guid AnswerId { get; set; } = answer.AnswerId;
        public int Question { get; set; } = answer.Question;
        public string AnswerText { get; set; } = answer.AnswerText;
    }

    public class TeamAnswer
    {
        public Guid TeamId { get; set; }
        public List<Answer> Answers { get; set; } = [];
        public string TeamName { get; set; }

        public TeamAnswer(Team team, List<Models.Answer> answers)
        {
            TeamId = team.TeamId;
            TeamName = team.TeamName;
            foreach (var answer in answers) Answers.Add(new Answer(answer));
        }
    }

}
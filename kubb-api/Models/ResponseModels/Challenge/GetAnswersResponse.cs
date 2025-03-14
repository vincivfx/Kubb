namespace KubbAdminAPI.Models.ResponseModels.Challenge;

public class GetAnswersResponse(List<GetAnswersResponse.TeamAnswer> teamAnswer)
{

    public List<TeamAnswer> Answers { get; set; } = teamAnswer;

    public class Answer(Models.Answer answer, Models.Challenge challenge)
    {
        public Guid AnswerId { get; set; } = answer.AnswerId;
        public int Question { get; set; } = answer.Question;
        public string AnswerText { get; set; } = answer.AnswerText;

        public bool Correctness { get; set; } = answer.AnswerText == challenge.Questions[answer.Question];
    }

    public class TeamAnswer
    {
        public Guid TeamId { get; set; }
        public List<Answer> Answers { get; set; } = [];
        public string TeamName { get; set; }

        public TeamAnswer(Team team, List<Models.Answer> answers, Models.Challenge challenge)
        {
            TeamId = team.TeamId;
            TeamName = team.TeamName;
            foreach (var answer in answers) Answers.Add(new Answer(answer, challenge));
        }
    }

}
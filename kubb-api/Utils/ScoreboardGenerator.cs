using System.ComponentModel;
using KubbAdminAPI.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KubbAdminAPI.Utils;

public class ScoreboardGenerator
{
    public static string GenerateScoreboard(Challenge challenge, List<Team> teams, List<Answer> answers)
    {
        string output = "";

        var algorithmSettings = JsonSerializer.Deserialize<AlgorithmSettings>(challenge.AlgorithmSettings)!;

        if (algorithmSettings.freezeScoreboard > 0 && DateTime.UtcNow < challenge.EndTime && challenge.EndTime.AddMinutes(-algorithmSettings.freezeScoreboard) < DateTime.UtcNow)
        {
            return "(!)FROZEN";
        }

        var teamPoints = new Dictionary<Guid, SimpleTeam>();

        var rightAnswersGroups = new List<List<Answer>>();
        for (int i = 0; i < challenge.Questions.Count; i++)
        {
            rightAnswersGroups.Add([]);
        }

        foreach (var team in teams)
        {
            var element = new SimpleTeam(challenge.Questions.Count, team);
            teamPoints.Add(team.TeamId, element);
        }

        var points = new List<int>();
        for (var i = 0; i < challenge.Questions.Count; i++) points.Add(algorithmSettings.basePoints);

        var deriva = algorithmSettings.derivaTeams;
        var bonuses = algorithmSettings.Bonus;
        // var completeLineBonuses = new List<int> { 100, 60, 40, 30, 20, 10 };

        foreach (var answer in answers)
        {
            // security check on question index
            if (answer.Question >= challenge.Questions.Count) continue;
            if (DateTime.UtcNow.AddSeconds(-45) < answer.Created)
                teamPoints[answer.Team.TeamId].Cells[answer.Question].Highlight = true;

            if (!answer.AnswerText.Equals(challenge.Questions[answer.Question]))
            {
                points[answer.Question] += 1;
                teamPoints[answer.Team.TeamId].Cells[answer.Question].AddWrong();
            }
            else
            {
                // if a right question was given before, ignore the current one (problems with bonuses) 
                var validateAnswer = true;
                foreach (var rightAnswer in rightAnswersGroups[answer.Question])
                {
                    if (rightAnswer.Team == answer.Team)
                    {
                        validateAnswer = false;
                        break;
                    }
                }
                if (validateAnswer) rightAnswersGroups[answer.Question].Add(answer);
            }
        }

        foreach (var rightAnswersGroup in rightAnswersGroups)
        {
            rightAnswersGroup.Sort((a, b) => a.Created < b.Created ? -1 : 1);
        }

        for (var groupId = 0; groupId < points.Count; groupId += 1)
        {
            var rightAnswersGroup = rightAnswersGroups[groupId];

            double diffTime = 0;
            if (deriva <= 0)
            {
                diffTime = 0;
            }
            else if (rightAnswersGroup.Count >= deriva)
            {
                diffTime = rightAnswersGroup[deriva - 1].Created.Subtract(challenge.StartTime).TotalMinutes;
            }
            else if (DateTime.UtcNow < challenge.EndTime)
            {
                diffTime = DateTime.UtcNow.Subtract(challenge.StartTime).TotalMinutes;
            }
            else
            {
                diffTime = challenge.EndTime.AddMinutes(algorithmSettings.stopIncreseMinutes).Subtract(challenge.StartTime).TotalMinutes;
            }

            points[groupId] += Convert.ToInt32(diffTime);

            for (var i = 0; i < rightAnswersGroup.Count; i++)
            {
                var team = rightAnswersGroup[i].Team.TeamId;
                var bonus = 0;
                if (i < bonuses.Count) bonus = bonuses[i];
                teamPoints[team].Cells[rightAnswersGroup[i].Question]
                    .AddRight(points[groupId] + bonus);
            }
        }

        var pointLine = "";
        for (var pointId = 0; pointId < points.Count; pointId++)
        {
            if (pointId != 0) pointLine += ",";
            pointLine += points[pointId];
        }

        output += pointLine + "\n";

        var jollyTimeFinished = challenge.StartTime.AddMinutes(10) < DateTime.UtcNow;

        foreach (var teamPoint in teamPoints.ToList())
        {
            var teamLine = teamPoint.Value.TeamName + ",";

            for (var cell = 0; cell < teamPoint.Value.Cells.Count; cell += 1)
            {
                var t = teamPoint.Value.Cells[cell];

                // set jolly
                if (teamPoint.Value.TeamOptions.JollyId == cell || cell == 0 && jollyTimeFinished && teamPoint.Value.TeamOptions.JollyId == null)
                {
                    t.IsJolly = true;
                }

                if (cell > 0) teamLine += ",";
                if (t.HasBeenSet)
                    teamLine += t.GetPoints().ToString() + (t.Highlight ? 'H' : null);
                
                teamLine += jollyTimeFinished && t.IsJolly ? 'J' : null;

                if (t.HasBeenSet) teamLine += t.Direction ? '+' : '-';
            }

            output += teamLine + "\n";
        }

        return output;
    }
}

public record ScoreboardCell
{
    private int Points { get; set; } = 0;
    public bool HasBeenSet { get; private set; } = false;
    public bool Highlight { get; set; } = false;
    public bool Direction { get; private set; } = false;
    public bool IsJolly { get; set; } = false;

    public void AddWrong()
    {
        Points -= 10;
        HasBeenSet = true;
    }

    public void AddRight(int rightAnswerPoints)
    {
        if (Direction) return;
        Points += rightAnswerPoints;
        Direction = true;
        HasBeenSet = true;
    }

    public int? GetPoints()
    {
        if (this.IsJolly) return this.Points * 2;
        return this.Points;
    }
}

public class SimpleTeam
{
    public SimpleTeam(int size, Team team)
    {
        TeamName = team.TeamName;
        TeamId = team.TeamId;
        TeamOptions = JsonSerializer.Deserialize<TeamOptionString>(team.OptionString)!;

        for (var i = 0; i < size; i += 1) Cells.Add(new ScoreboardCell());
    }

    public List<ScoreboardCell> Cells { get; set; } = [];
    public string TeamName { get; set; }
    public Guid TeamId { get; set; }

    public TeamOptionString TeamOptions { get; set; }
}

// [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
public class AlgorithmSettings
{
    [JsonPropertyName("bp")]
    public int basePoints { get; set; } = 30;
    [JsonPropertyName("dt")]

    public int derivaTeams { get; set; } = 3;

    [JsonPropertyName("si")]
    public int stopIncreseMinutes { get; set; } = 20;

    [JsonPropertyName("fz")]

    public int freezeScoreboard { get; set; } = 0;

    [JsonPropertyName("bn")]
    public List<int> Bonus { get; set; } = [20, 15, 10, 8, 6, 5, 4, 3, 2, 1];
}

// [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
public class TeamOptionString
{
    [JsonPropertyName("j")]
    public int? JollyId { get; set; } = null;
}
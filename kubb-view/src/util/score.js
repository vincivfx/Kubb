export default {
    testScoreboard() {

        const problems = 24;
        const teams = 100;

        let output = '';

        for (let i = 0; i < problems; i += 1) {
            output += (20 + Math.ceil(Math.random() * 60)) + (i === 23 ? '\n' : ",");
        }

        for (let i = 0; i < teams; i += 1) {
            output += "Team #" + i;
            const jolly = Math.ceil(Math.random() * problems);

            for (let j = 0; j < problems; j += 1) {
                output += ",";
                if (Math.random() < 1 - i * (1 / teams)) {
                    output += (Math.ceil(Math.random() * 110) - 25) + "";
                    if (Math.random() > 0.97) output += "H";
                }
                if (jolly === j) output += "J";
            
            }

            output += "\n";
        }

        console.log(output);
        return output;

    },
    
    /**
     * 
     */
    parseScoreboard(text, challenge) {

        if (text === "(!)FROZEN") return 'frozen';

        const lines = text.split('\n');

        let teams = [];
        let questions = [];

        lines[0].split(',').forEach(item => {
            questions.push({
                points: item,
                answers: 0,
                blocked: false
            })
        })

        for (let i = 1; i <= lines.length; i += 1) {
            
            let totalPoints = 200;
            
            if (!lines[i]) continue;
            const parts = lines[i].split(',');

            let teamQuestions = [];

            for (let j = 1; j < parts.length; j += 1) {
                let arrow = null;

                let points = parts[j].replace(/H|J|-$|\+$/g, '');

                if (points !== '' && points < 0) arrow = 'down';
                if (points !== '' && points > 0) arrow = 'up';

                totalPoints += 1 * points;

                if (arrow === 'up') {
                    questions[j - 1]['answers'] += 1;
                    if (questions[j - 1]['answers'] >= (challenge.algorithmSettings.dt ?? 3)) 
                        questions[j - 1]['blocked'] = true;
                }

                teamQuestions.push({
                    points,
                    arrow,
                    highlight: parts[j].indexOf('H') >= 0,
                    jolly: parts[j].indexOf('J') >= 0,
                })
            }

            teams.push({
                name: parts[0],
                points: totalPoints,
                questions: teamQuestions
            })

        }
        
        teams = teams.sort((a, b) => b.points - a.points);
        
        teams.forEach((team, key) => team.position = key + 1); // set position value

        return {
            teams,
            questions
        }

    }
}
<script>
export default {
    data: () => ({
        teams: [
            {name: 'Scuola 1', points: 100, questions: [10, null, 20, 30], animate:[0,0,0,1]},
            {name: 'Scuola 1', points: 100, questions: [-10, null, 20, 30], animate:[0,0,0,1]},
            {name: 'Scuola 1', points: 100, questions: [10, null, 20, 30], animate:[0,0,0,1]},
            {name: 'Scuola 1', points: 100, questions: [10, null, 20, 30], animate:[0,0,0,1]},
            {name: 'Scuola 1', points: 100, questions: [10, null, 20, 30], animate:[0,0,0,1]},
            {name: 'Scuola 1', points: 100, questions: [10, null, 20, 30], animate:[0,0,0,1]},
            {name: 'Scuola 1', points: 100, questions: [10, null, 20, 30], animate:[0,0,0,1]},
        ] ,
    })
}
</script>

<template>
    <h1>Challenge name</h1>
    <div class="scoreboard-table">
        <div class="team" v-for="(team, key) in teams" :key="key">
            
            <div class="team-position">
                {{ key + 1 }} 
            </div>
            <div class="team-name">
                {{ team.name }}
            </div>
            <div class="team-points">
                {{ team.points }}
            </div>
            <div class="team-question" v-for="(question, key_question) in team.questions" :class="[{'positive': question !== null && question >= 0}, {'negative': question !== null && question < 0}, {'animate': team.animate[key_question]}]" :key="key2">
                <span v-if="question != null">
                    {{ question }}
                </span>
            </div>
        </div>
    </div>
</template>


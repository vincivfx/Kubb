<script lang="ts">
import InputBlock from '@/components/InputBlock.vue';
import Select from '@/components/Select.vue';
import Tabs from "@/components/Tabs.vue";
import Alert from "@/components/Alert.vue";
import Badge from "@/components/Badge.vue";

export default {
  components: {Badge, Alert, Tabs, Select, InputBlock},
  data: () => ({
    page: 'send-answer',
    challenge: {},
    teams: [],
    timer: null,
    timer_text: '',
    sendAnswerForm: {
      teamId: '',
      questionId: 0,
      answerText: ''
    },
    sendAnswerAlert: '',
    questions: [],
    answers: [],
    answerFilter: ''
  }),
  methods: {
    sendAnswer(e) {
      e.preventDefault()

      this.$http.post("Challenge/SendAnswer", this.sendAnswerForm).then((res) => {
        this.sendAnswerAlert = 'success';
        this.sendAnswerForm = {
          teamId: '',
          questionId: '',
          answerText: ''
        }
        this.correctness = res.data.correctness;
        setTimeout(() => {this.sendAnswerAlert = ''}, 10000)
      }).catch((err) => {
        this.sendAnswerAlert = 'error';
        setTimeout(() => {this.sendAnswerAlert = ''}, 10000)
      })
    },
    getAnswers() {
      this.$http.get("Challenge/GetAnswers?challengeId=" + encodeURIComponent(this.$route.query.id)).then(res => {
        this.answers = res.data.answers;
      })
    }
  },
  unmounted() {
    clearInterval(this.timer);
  },
  mounted() {
    this.$http.get("/Challenge/GetInfo?ChallengeId=" + encodeURIComponent(this.$route.query.id)).then(response => {
      this.challenge = response.data.challengeSetup;

      this.timer = setInterval(() => {
        const diff = Math.floor((new Date(this.challenge.endTime).getTime() - new Date().getTime()) / 1000);
        if (diff > 0)
          this.timer_text = Math.floor(diff / 3600) + "h " + Math.floor(diff / 60) % 60 + "m " + diff % 60 + "s"
        else
          this.timer_text = 'finished'
      }, 1000);
      
      for (let i = 0; i < this.challenge.questions; i += 1)
        this.questions.push({
          text: 'Challenge #' + (i + 1),
          key: i
        });


      this.teams = response.data.teams;


      this.teams.forEach(team => {
        team.text = team.teamName;
        team.key = team.teamId;
      });
    });
  }
}

</script>

<template>

  <h2>{{ challenge.name }}</h2>
  <p>
    Time 'till the end: {{ timer_text }}
  </p>

  <Tabs v-model="page"
        :tabs="[{text: 'Send answer', id: 'send-answer'}, {text: 'Set Jolly', id: 'set-jolly'}, {text: 'Answer List', id: 'answer-list'}]">
    <div v-if="page === 'send-answer'">
      <h3>Send answer</h3>
      <Alert v-if="sendAnswerAlert === 'error'" type="danger">
        Cannot send
      </Alert>
      <Alert v-if="sendAnswerAlert === 'success'" type="success">
        Answer sent successfully <Badge v-if="correctness" type="success">CORRECT</Badge>
        <Badge v-if="!correctness" type="danger">WRONG</Badge>
      </Alert>
      
      
      <form @submit="sendAnswer">
        <Select :options="teams" v-model="sendAnswerForm.teamId" label="Select team"></Select>

        <Select :options="questions" v-model="sendAnswerForm.questionId" label="Question"></Select>

        <InputBlock v-model="sendAnswerForm.answerText" label="Answer to question"></InputBlock>

        <input type="submit" value="Send Answer" class="btn primary">
      </form>
    </div>
    

    <div v-if="page === 'answer-list'">
      <button class="btn primary" @click="getAnswers()">Refresh</button>

      <Select v-model="answerFilter" :options="answers.map(item => ({text: item.teamName, key: item.teamName}))" placeholder="" label="Search by Team Name" />
      
      <div class="table-responsive">
        <table class="table">
          <thead>
            <tr>
              <th>Team Name</th>
              <td v-for="(_, qid) in questions" :key="qid">
                {{ qid + 1 }}
              </td>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(team, teamKey) in answers.filter(a => a.teamName.toLowerCase().indexOf(answerFilter.toLowerCase()) >= 0)" :key="teamKey">
              <td>
                {{ team.teamName }}
              </td>
              <td v-for="(_, qid) in questions" :key="qid">
                
                <Badge :type="answer.correctness ? 'success' : 'danger'" v-for="(answer, answerKey) in team.answers.filter(answer => answer.question === qid)" :key="answerKey">{{ answer.answerText }}</Badge>
              </td>
            
            </tr>
          </tbody>
        </table>
      </div>
      
    </div>

  </Tabs>

</template>
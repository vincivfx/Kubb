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
    questions: []
  }),
  methods: {
    sendAnswer(e) {
      e.preventDefault()

      this.$http.post("Challenge/SendAnswer", this.sendAnswerForm).then((res) => {
        this.sendAnswerAlert = 'success';
        this.sendAnswerForm = {
          teamId: '',
          questionId: 0,
          answerText: ''
        }
        this.correctness = res.data.correctness;
        setTimeout(() => {this.sendAnswerAlert = ''}, 10000)
      }).catch((err) => {
        this.sendAnswerAlert = 'error';
        setTimeout(() => {this.sendAnswerAlert = ''}, 10000)
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
  </Tabs>

</template>
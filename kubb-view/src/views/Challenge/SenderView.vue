<script lang="ts">
import InputBlock from '@/components/InputBlock.vue';
import Select from '@/components/Select.vue';
import Tabs from "@/components/Tabs.vue";
import Alert from "@/components/Alert.vue";
import Badge from "@/components/Badge.vue";
import Modal from '@/components/Modal.vue';

export default {
  components: { Badge, Alert, Tabs, Select, InputBlock, Modal },
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
    answerFilter: '',
    setJollyAlert: '',
    setJollyForm: {
      questionId: '',
      teamId: ''
    },
    deleteAnswerItem: {}
  }),
  methods: {
    deleteAnswerPrepareForm(team, answerKey) {
      this.deleteAnswerItem = { team, answer: team.answers[answerKey], answerKey };
      this.$refs.deleteAnswerModal.show();
    },
    deleteAnswer() { 
      this.$http.post("Challenge/DeleteAnswer", {
        answerId: this.deleteAnswerItem.answer.answerId
      }).then(() => {
        this.deleteAnswerItem.team.answers.splice(this.deleteAnswerItem.answerKey, 1);
        this.$refs.deleteAnswerModal.hide();
      }).catch(() => {
        alert("error");
      })
    },
    getCurrentTeamJolly() {
      let el = this.teams.filter(team => team.teamId === this.setJollyForm.teamId);
      if (el.length === 0) return false;
      return el[0];
    },
    setJolly(e) {
      e.preventDefault()
      this.$http.post("Challenge/SetJolly", this.setJollyForm).then(() => {
        this.setJollyAlert = 'success';
        this.teams.filter(team => team.teamId === this.setJollyForm.teamId)[0].optionString.j = this.setJollyForm.questionId;
        this.setJollyForm = {
          teamId: '',
          questionId: ''
        }
        setTimeout(() => { this.setJollyAlert = '' }, 10000)
      }).catch(() => {
        this.setJollyAlert = 'error';
        setTimeout(() => { this.setJollyAlert = '' }, 10000)
      })
    },
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
        setTimeout(() => { this.sendAnswerAlert = '' }, 10000)
      }).catch((err) => {
        this.sendAnswerAlert = 'error';
        setTimeout(() => { this.sendAnswerAlert = '' }, 10000)
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


      this.teams.forEach((team) => {
        team.text = team.teamName;
        team.optionString = JSON.parse(team.optionString);
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
    :tabs="[{ text: 'Send answer', id: 'send-answer' }, { text: 'Set Jolly', id: 'set-jolly' }, { text: 'Answer List', id: 'answer-list' }]">
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

    <div v-if="page === 'set-jolly'">
      <h3>Set Jolly</h3>

      <Alert v-if="setJollyAlert === 'error'" type="danger">
        Cannot set jolly
      </Alert>
      <Alert v-if="setJollyAlert === 'success'" type="success">
        Jolly set successfully
      </Alert>

      <Alert type="warning"
        v-if="setJollyAlert !== 'success' && getCurrentTeamJolly() && getCurrentTeamJolly().optionString.j">
        Jolly already set on <b>Question {{ getCurrentTeamJolly().optionString.j + 1 }}</b> for team <b>{{
          getCurrentTeamJolly().teamName }}</b>
      </Alert>

      <form @submit="setJolly">
        <Select v-model="setJollyForm.teamId" :options="teams" label="Select team"></Select>

        <Select v-model="setJollyForm.questionId" :options="questions" label="Question"></Select>

        <input type="submit" class="btn primary" value="Set Jolly">
      </form>


    </div>


    <div v-if="page === 'answer-list'">
      <button class="btn primary" @click="getAnswers()">Refresh</button>

      <Select v-model="answerFilter" :options="answers.map(item => ({ text: item.teamName, key: item.teamName }))"
        placeholder="" label="Search by Team Name" />

      <div class="table-responsive">
        <table class="table hover">
          <thead>
            <tr>
              <th>Team Name</th>
              <td v-for="(_, qid) in questions" :key="qid">
                {{ qid + 1 }}
              </td>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(team, teamKey) in answers.filter(a => a.teamName.toLowerCase().indexOf(answerFilter.toLowerCase()) >= 0)"
              :key="teamKey">
              <td>
                {{ team.teamName }}
              </td>
              <td v-for="(_, qid) in questions" :key="qid">

                <Badge @click="deleteAnswerPrepareForm(team, answerKey)" :type="answer.correctness ? 'success' : 'danger'"
                  v-for="(answer, answerKey) in team.answers.filter(answer => answer.question === qid)"
                  :key="answerKey">{{ answer.answerText }}</Badge>
              </td>

            </tr>
          </tbody>
        </table>
      </div>

    </div>

  </Tabs>

  <Modal ref="deleteAnswerModal" title="Delete answer">
    <div class="text-center">
      <p>
        Do you want to delete answer: <Badge :type="deleteAnswerItem.answer.correctness ? 'success' : 'danger'">{{
          deleteAnswerItem.answer.answerText }}</Badge> for team <b>{{ deleteAnswerItem.team.teamName }}</b>?
      </p>
      <div class="btn-group-right">
        <button class="btn danger" @click="deleteAnswer()">Yes, delete</button>
        <button class="btn" @click="$refs.deleteAnswerModal.hide()">Cancel</button>
      </div>
    </div>
  </Modal>

</template>
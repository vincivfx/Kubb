<script lang="ts">
import Modal from '@/components/Modal.vue';
import { SlPencil, SlPlus, SlTrash } from 'vue-icons-plus/sl';
import Tabs from "@/components/Tabs.vue";
import InputBlock from "@/components/InputBlock.vue";
import Alert from "@/components/Alert.vue";
import DateTimeInput from "@/components/DateTimeInput.vue";
import CheckBox from "@/components/CheckBox.vue";
import Badge from "@/components/Badge.vue";
import TeamTab from '@/partials/AdminView/TeamTab.vue';

export default {
  components: { Badge, CheckBox, DateTimeInput, Alert, InputBlock, Tabs, SlPencil, SlPlus, SlTrash, Modal, TeamTab },
  data: () => ({
    status: 'loading',
    teams: [],
    participations: [],
    page: 'overview',
    challenge: {
      questions: []
    },
    updateChallengeForm: {
      questions: []
    },
    updateChallengeStatus: '',
    submitChallengeError: false,
    updatedQuestions: false,
    updated: false,
    flags: {},
    editFlags: {},
    deleteChallengeName: '', // check to delete challenge the name typed
    algorithmSettings: {},
    updatedAlgorithmSettings: false
  }),
  methods: {
    clearFlags() {
      this.flags = [
        (this.challenge.status & 1) != 0,
        (this.challenge.status & 2) != 0,
        (this.challenge.status & 4) != 0,
        (this.challenge.status & 8) != 0
      ]
      this.updated = false;
      this.editFlags = JSON.parse(JSON.stringify(this.flags));
    },
    clearUpdatedQuestions() {
      this.updatedQuestions = false;
      this.updateChallengeForm.questions = JSON.parse(JSON.stringify(this.challenge.questions)); // JS SUCKS
    },
    clearUpdatedAlgorithmSettings() {
      this.updatedAlgorithmSettings = false;
      let a = JSON.parse(JSON.stringify(this.challenge.algorithmSettings));


      // this.algorithmSettings = a;    
    },
    saveChallenge(e = null, s = null) {
      if (e !== null)
        e.preventDefault();
      this.updateChallengeForm.status = 0;
      this.editFlags.forEach((flag, flagIndex) => {
        this.updateChallengeForm.status += (flag << flagIndex);
      })

      // format bonus string

      let algorithmSettings = JSON.parse(JSON.stringify(this.algorithmSettings));
      algorithmSettings.bn = algorithmSettings.bn.split(',').map(item => Number.parseInt(item))
      this.updateChallengeForm.algorithmSettings = JSON.stringify(algorithmSettings);

      if (s === 'submit') this.updateChallengeForm.runningStatus = 1;

      this.$http.put("/ChallengeAdmin/UpdateChallenge", this.updateChallengeForm).then(() => {
        this.updateChallengeStatus = 'success';
        this.challenge = JSON.parse(JSON.stringify(this.updateChallengeForm));
        this.updatedQuestions = false;
        this.clearUpdatedAlgorithmSettings();
        this.updated = false;
        this.flags = JSON.parse(JSON.stringify(this.editFlags));
        this.$refs.submitChallengeModal.hide();
        setTimeout(() => this.updateChallengeStatus = '', 20000);
      }).catch((e) => {
        console.error(e);
        this.updateChallengeStatus = 'error';
        setTimeout(() => this.updateChallengeStatus = '', 20000);
        if (s === 'submit') {
          this.submitChallengeError = true;
          setTimeout(() => this.submitChallengeError = false, 20000);
        }
      })
    },

    deleteChallenge(e) {
      e.preventDefault();
      this.$http.post("ChallengeAdmin/DeleteChallenge", {
        challengeId: this.$route.query.id
      }).then(() => this.$router.push({ name: 'challenges', params: { page: 'my' } }))
        .catch(() => alert("cannot delete challenge"));
    },
    manualStart() {
      this.$http.post("ChallengeAdmin/ManualStart", {
        challengeId: this.$route.query.id
      }).then(() => {
        this.challenge.runningStatus = 2;
        this.$refs.manualStartModal.hide();
      }).catch(() => alert("cannot start challenge"));
    }
  },
  mounted() {
    this.$http.get("/ChallengeAdmin/ChallengeInfo?challengeId=" + encodeURIComponent(this.$route.query.id)).then((response) => {
      this.challenge = response.data.challenge;
      this.challenge.challengeId = this.$route.query.id;
      this.updateChallengeForm = JSON.parse(JSON.stringify(response.data.challenge)); // js sucks!
      this.teams = response.data.teams;
      this.participations = response.data.participations;

      const algorithmSettingsRaw = this.challenge.algorithmSettings;

      const algorithmSettings = {
        dt: algorithmSettingsRaw.dt ?? 3,
        bp: algorithmSettingsRaw.bp ?? 30,
        si: algorithmSettingsRaw.si ?? 20,
        fz: algorithmSettingsRaw.fz ?? 0,
        bn: algorithmSettingsRaw.bn ? algorithmSettingsRaw.bn.join(",") : "20,15,10,8,6,5,4,3,2,1"
      };

      this.algorithmSettings = algorithmSettings;

      this.challenge.startTime = new Date(this.challenge.startTime);
      this.challenge.endTime = new Date(this.challenge.endTime);

      this.clearFlags();

      this.status = 'ok';
    }).catch((e) => {
      console.error(e);
      this.$router.push({ name: 'challenges' });
    })
  }
}
</script>

<template>
  <div v-if="status === 'ok'">

    <h2>{{ challenge.name }}</h2>

    <Tabs v-model="page"
      :tabs="[{ text: 'Challenge Overview', id: 'overview' }, { text: 'Teams', id: 'teams' }, { text: 'Participations', id: 'participations' }, { text: 'Questions', id: 'questions', onExit: clearUpdatedQuestions }, { text: 'Challenge Options', id: 'options', onExit: clearUpdatedAlgorithmSettings }]">
      <div v-if="page === 'overview'">
        <table class="text-left">
          <tr>
            <th>Running status</th>
            <td>
              <Badge v-if="challenge.runningStatus === 0" type="info">DRAFT</Badge>
              <Badge v-if="challenge.runningStatus === 1" type="info">SUBMITTED</Badge>
              <Badge v-if="challenge.runningStatus === 2" type="info">RUNNING</Badge>
              <Badge v-if="challenge.runningStatus === 3" type="info">FROZEN</Badge>
              <Badge v-if="challenge.runningStatus === 4" type="info">TERMINATED</Badge>
            </td>
          </tr>
          <tr>
            <th>Starting Time:</th>
            <td>{{ new Date(challenge.startTime).toLocaleString() }}</td>
          </tr>
          <tr>
            <th>Ending Time:</th>
            <td>{{ new Date(challenge.endTime).toLocaleString() }}</td>
          </tr>
        </table>
      </div>
      <TeamTab v-if="page === 'teams'" :challenge="challenge" :teams="teams" />
      <div v-if="page === 'participations'">
        <h3>Participations</h3>

        <div class="table-resposive">
          <table class="table">
            <thead>
              <tr>
                <th>Name and Surname</th>
                <th>Email Address</th>
                <th>Created at</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, key) in participations" :key="key">
                <td>{{ item.name }} {{ item.surname }}</td>
                <td>{{ item.emailAddress }}</td>
                <td>{{ new Date(item.created).toLocaleString() }}</td>
              </tr>
              <tr v-if="participations.length === 0">
                <td colspan="3">There are no participations</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <div v-if="page === 'options'">

        <fieldset :disabled="challenge.runningStatus !== 0">
          <h4>
            Challenge details
            <Badge type="warning" v-if="updated">NOT SAVED</Badge>
          </h4>
          <Alert type="danger" v-if="updateChallengeStatus === 'error'">
            We encountered an error trying to edit challenge details
          </Alert>
          <Alert type="success" v-if="updateChallengeStatus === 'success'">
            Challenge details saved successfully
          </Alert>
          <InputBlock @change="updated = true" v-model="updateChallengeForm.name" placeholder="name"
            label="Challenge name" />
          <DateTimeInput @change="updated = true"
            :invalid="new Date(updateChallengeForm.startTime).getTime() < new Date().getTime()"
            :min="new Date().getTime()" :max="updateChallengeForm.endTime" v-model="updateChallengeForm.startTime"
            label="Starting Time"></DateTimeInput>
          <DateTimeInput @change="updated = true"
            :invalid="new Date(updateChallengeForm.endTime).getTime() < new Date(updateChallengeForm.startTime).getTime()"
            :min="updateChallengeForm.startTime" v-model="updateChallengeForm.endTime" label="Ending Time">
          </DateTimeInput>

          <CheckBox :disabled="challenge.runningStatus !== 0" v-model="editFlags[0]" @change="updated = true">Challenge
            will be visibile on active
            challenges
            page</CheckBox>
          <CheckBox :disabled="challenge.runningStatus !== 0" v-model="editFlags[1]" @change="updated = true">Allow
            anonymous joins</CheckBox>
          <CheckBox :disabled="challenge.runningStatus !== 0" v-model="editFlags[2]" @change="updated = true">Allow
            joiners to remove answers</CheckBox>
          <CheckBox :disabled="challenge.runningStatus !== 0" v-model="editFlags[3]" @change="updated = true">Start
            challenge automatically at <i>starting
              time</i></CheckBox>

        </fieldset>
        <fieldset :disabled="challenge.runningStatus >= 2">
          <InputBlock @change="updated = true" type="number" placeholder="number of teams" required=""
            label="Maximum teams for participation" v-model="updateChallengeForm.maxTeamPerUser" />
          <InputBlock @change="updated = true" type="number" placeholder="base points" required=""
            label="base points for every team" v-model="updateChallengeForm.basePoints" />

          <input v-if="challenge.runningStatus < 2" type="submit" value="Save" class="btn primary"
            @click="saveChallenge">

          <hr />

          <div>
            <h4>Algorithm Settings<Badge type="warning" v-if="updatedAlgorithmSettings">NOT SAVED</Badge>
            </h4>
            <form @submit="saveChallenge">
              <InputBlock @change="updatedAlgorithmSettings = true" v-model="algorithmSettings.si" type="number"
                label="Minutes to end when stop points increase" />
              <InputBlock @change="updatedAlgorithmSettings = true" v-model="algorithmSettings.bp" type="number"
                label="Starting points per question" />
              <InputBlock @change="updatedAlgorithmSettings = true" v-model="algorithmSettings.fz" type="number"
                label="Minutes to end when scoreboard starts freezing" />
              <InputBlock @change="updatedAlgorithmSettings = true" v-model="algorithmSettings.dt" type="number"
                label="Correct answers per question before stop increment points" />
              <InputBlock @change="updatedAlgorithmSettings = true" v-model="algorithmSettings.bn"
                label="Bonus points for first answers (separated by comma)" pattern="^(,*[0-9]+)+$"></InputBlock>
              <input v-if="challenge.runningStatus < 2" type="submit" value="Save algorithm settings"
                class="btn primary">
            </form>
            <hr />
          </div>
        </fieldset>

        <div v-if="challenge.runningStatus === 1">
          <h4>Submit challenge</h4>

          <Alert type="warning">
            To make your challenge visible to the other people or just
            to start it, you need to set its status to submitted.<br>
            <b>NOTE</b> Once you set it to submitted status you won't be able to
            edit the details anymore.
          </Alert>

          <button @click="$refs.submitChallengeModal.show()" class="btn primary">Submit Challenge</button>

          <hr />
        </div>

        <div v-if="challenge.runningStatus === 1 && (challenge.status & 8) === 0">
          <h4>Manual Start</h4>
          <p>
            This challenge has been set to manual start.
          </p>
          <button class="btn primary" @click="$refs.manualStartModal.show()">Start Challenge</button>
          <hr />
        </div>

        <h4>Delete this challenge</h4>
        <p>
          If you request the challenge deletion, you will be not able to recover it.
        </p>
        <button class="btn danger" @click="$refs.deleteChallengeModal.show()">Delete Challenge</button>
      </div>
      <div v-if="page === 'questions'">
        <h3>
          Questions

          <Badge type="warning" v-if="updatedQuestions">NOT SAVED</Badge>
        </h3>

        <form @submit="saveChallenge">
          <div v-for="(_, qid) in updateChallengeForm.questions" :key="qid" class="">
            <InputBlock :disabled="challenge.runningStatus >= 2" tabindex="0" @change="updatedQuestions = true"
              @keyup="updatedQuestions = true" v-model="updateChallengeForm.questions[qid]"
              :placeholder="'Type answer for #' + (qid + 1)" :label="'Question #' + (qid + 1)">
              <button v-if="challenge.runningStatus < 2" tabindex="-1"
                @click="updateChallengeForm.questions.splice(qid, 1)" class="btn danger small">
                <SlTrash />
              </button>
            </InputBlock>
          </div>

          <div v-if="challenge.runningStatus < 2" class="text-center">

            <button tabindex="0" type="button" class="btn secondary small"
              @click="updateChallengeForm.questions.push('')">
              <SlPlus />
            </button>
            <input type="submit" class="btn primary" value="Save Questions" />
          </div>

        </form>

      </div>
    </Tabs>

    <Modal title="Delete this challenge" ref="deleteChallengeModal">
      Are you sure to delete <b>{{ challenge.name }}</b> challenge?
      <form @submit="deleteChallenge">
        <InputBlock v-model="deleteChallengeName" label="Type the challenge name"></InputBlock>
        <input :disabled="deleteChallengeName !== challenge.name" type="submit" class="btn danger"
          value="Delete this challenge">
      </form>
    </Modal>

    <Modal ref="manualStartModal" title="Manual Start">
      Are you sure to start <b>{{ challenge.name }}</b> challenge?
      <div class="text-center m-2">
        <button class="btn primary m-1" @click="manualStart()">Yes, start challenge</button>
        <button class="btn m-1" @click="$refs.manualStartModal.hide()">Cancel</button>
      </div>
    </Modal>

    <Modal title="Submit challenge" ref="submitChallengeModal">
      <Alert type="danger" v-if="submitChallengeError">
        Sorry, we encountered an error.
      </Alert>
      <Alert type="warning">
        To make your challenge visible to the other people or just
        to start it, you need to set its status to submitted.<br>
        <b>NOTE</b> Once you set it to submitted status you won't be able to
        edit the details anymore.
      </Alert>

      <button class="btn warning" @click="saveChallenge(null, 'submit')">Submit challenge</button>

    </Modal>
  </div>

</template>

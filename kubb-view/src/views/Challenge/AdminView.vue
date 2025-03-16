<script lang="ts">
import Modal from '@/components/Modal.vue';
import { SlPencil, SlPlus, SlTrash } from 'vue-icons-plus/sl';
import Tabs from "@/components/Tabs.vue";
import InputBlock from "@/components/InputBlock.vue";
import Alert from "@/components/Alert.vue";
import DateTimeInput from "@/components/DateTimeInput.vue";
import CheckBox from "@/components/CheckBox.vue";
import Badge from "@/components/Badge.vue";

export default {
  components: { Badge, CheckBox, DateTimeInput, Alert, InputBlock, Tabs, SlPencil, SlPlus, SlTrash, Modal },
  data: () => ({
    status: 'loading',
    teams: [],
    participations: [],
    page: 'overview',
    challenge: {
      questions: []
    },
    createTeamForm: {
      name: '',
      challengeId: '',
    },
    createTeamStatus: '',
    createTeamLastName: '',
    updateChallengeForm: {
      questions: []
    },
    updateChallengeStatus: '',
    submitChallengeError: false,
    updatedQuestions: false,
    updatedFlags: false,
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
      this.updatedFlags = false;
      this.editFlags = JSON.parse(JSON.stringify(this.flags));
    },
    clearUpdatedQuestions() {
      this.updatedQuestions = false;
      this.updateChallengeForm.questions = JSON.parse(JSON.stringify(this.challenge.questions)); // JS SUCKS
    },
    clearUpdatedAlgorithmSettings() {
      this.updatedAlgorithmSettings = false;
      this.algorithmSettings = JSON.parse(JSON.stringify(this.challenge.algorithmSettings));
    },
    saveChallenge(e = null, s = null) {
      if (e !== null)
        e.preventDefault();
      this.updateChallengeForm.status = 0;
      this.editFlags.forEach((flag, flagIndex) => {
        this.updateChallengeForm.status += (flag << flagIndex);
      })

      // format bonus string
      this.algorithmSettings.bn = this.algorithmSettings.bn.split(',').map(item => Number.parseInt(item))

      this.updateChallengeForm.algorithmSettings = JSON.stringify(this.algorithmSettings);

      if (s === 'submit') this.updateChallengeForm.runningStatus = 1;
      
      this.$http.put("/ChallengeAdmin/UpdateChallenge", this.updateChallengeForm).then(() => {
        this.updateChallengeStatus = 'success';
        this.challenge = JSON.parse(JSON.stringify(this.updateChallengeForm));
        this.updatedQuestions = false;
        this.updatedAlgorithmSettings = false;
        this.updatedFlags = false;
        this.flags = JSON.parse(JSON.stringify(this.editFlags));
        this.$refs.submitChallengeModal.hide();
        setTimeout(() => this.updateChallengeStatus = '', 20000);
      }).catch(() => {
        this.updateChallengeStatus = 'error';
        setTimeout(() => this.updateChallengeStatus = '', 20000);
        if (s === 'submit') {
          this.submitChallengeError = true;
          setTimeout(() => this.submitChallengeError = false, 20000);
        }
      })
    },
    createTeam(e) {
      e.preventDefault();

      this.$http.post("/Challenge/CreateTeam", this.createTeamForm).then((response) => {
        this.createTeamStatus = 'success';
        this.createTeamLastName = this.createTeamForm.name + "";
        setTimeout(() => this.createTeamStatus = '', 20000);
        this.teams.push({
          teamName: this.createTeamForm.name,
          teamId: response.data.teamId,
          created: new Date(),
        });
        this.createTeamForm.name = '';
      }).catch(() => {
        this.createTeamStatus = 'error';
        setTimeout(() => this.createTeamStatus = '', 20000);
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
      })
        .catch(() => alert("cannot start challenge"));
    }
  },
  mounted() {
    this.createTeamForm.challengeId = this.$route.query.id;
    this.$http.get("/ChallengeAdmin/ChallengeInfo?challengeId=" + encodeURIComponent(this.$route.query.id)).then((response) => {
      this.challenge = response.data.challenge;
      this.challenge.challengeId = this.$route.query.id;
      this.updateChallengeForm = JSON.parse(JSON.stringify(response.data.challenge)); // js sucks!
      this.teams = response.data.teams;
      this.participations = response.data.participations;
      
      const algorithmSettingsRaw = JSON.parse(response.data.challenge.algorithmSettings);
    
      const algorithmSettings = {
        dt: algorithmSettingsRaw.dt ?? 3,
        bp: algorithmSettingsRaw.bp ?? 30,
        si: algorithmSettingsRaw.si ?? 20,
        fz: algorithmSettingsRaw.fz ?? 0,
        bn: algorithmSettingsRaw.bn ?? "20,15,10,8,6,5,4,3,2,1"
      };

      this.challenge.algorithmSettings = algorithmSettings;
      this.algorithmSettings = JSON.parse(JSON.stringify(algorithmSettings));
      
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

    <h2>{{ challenge.name }}
      <button v-if="challenge.runningStatus === 0" @click="$refs.editChallengeModal.show()"
        class="btn btn-primary small">
        <SlPencil />
      </button>
    </h2>

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
      <div v-if="page === 'teams'">

        <h3>Teams
          <button @click="$refs.addTeamModal.show()" class="btn primary small">
            <SlPlus />
          </button>
        </h3>
        <table class="table">
          <thead>
            <tr>
              <th>Team name</th>
              <th>User</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, key) in teams" :key="key">
              <td>{{ item.teamName }}</td>
              <td><b v-if="item.owner">you</b></td>
              <td>
                <button class="btn small danger">
                  <SlTrash />
                </button>
              </td>

            </tr>
          </tbody>
        </table>
      </div>
      <div v-if="page === 'participations'">
        <h3>Participations</h3>
      </div>
      <div v-if="page === 'options'">

        <div v-if="challenge.runningStatus === 0">
          <h4>
            Challenge flags
            <Badge type="warning" v-if="updatedFlags">NOT SAVED</Badge>
          </h4>

          <CheckBox v-model="editFlags[0]" @change="updatedFlags = true">Challenge will be visibile on active challenges
            page</CheckBox>
          <CheckBox v-model="editFlags[1]" @change="updatedFlags = true">Allow anonymous joins</CheckBox>
          <CheckBox v-model="editFlags[2]" @change="updatedFlags = true">Allow joiners to remove answers</CheckBox>
          <CheckBox v-model="editFlags[3]" @change="updatedFlags = true">Start challenge automatically at <i>starting
              time</i></CheckBox>

          <button class="btn primary" @click="saveChallenge">Save flags</button>

          <hr />

          <h4>Submit challenge</h4>

          <Alert type="warning">
            To make your challenge visible to the other people or just
            to start it, you need to set its status to submitted.<br>
            <b>NOTE</b> Once you set it to submitted status you won't be able to
            edit the details anymore.
          </Alert>

          <button @click="$refs.submitChallengeModal.show()" class="btn primary">Submit Challenge</button>

          <hr />

          <div>
            <h4>Algorithm Settings<Badge type="warning" v-if="updatedAlgorithmSettings">NOT SAVED</Badge></h4>
            <form @submit="saveChallenge">
              <InputBlock @change="updatedAlgorithmSettings = true" v-model="algorithmSettings.si" type="number" label="Minutes to end when stop points increase" />
              <InputBlock @change="updatedAlgorithmSettings = true" v-model="algorithmSettings.bp" type="number" label="Starting points per question" />
              <InputBlock @change="updatedAlgorithmSettings = true" v-model="algorithmSettings.fz" type="number" label="Minutes to end when scoreboard starts freezing" />
              <InputBlock @change="updatedAlgorithmSettings = true" v-model="algorithmSettings.dt" type="number" label="Correct answers per question before stop increment points" />
              <InputBlock @change="updatedAlgorithmSettings = true" v-model="algorithmSettings.bn" label="Bonus points for first answers (separated by comma)" pattern="^(,*[0-9]+)+$"></InputBlock>
              <input type="submit" value="Save algorithm settings" class="btn primary">
            </form>
            <hr />
          </div>
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
            <InputBlock @change="updatedQuestions = true" @keyup="updatedQuestions = true"
              v-model="updateChallengeForm.questions[qid]" :placeholder="'Type answer for #' + (qid + 1)"
              :label="'Question #' + (qid + 1)">
              <button @click="updateChallengeForm.questions.splice(qid, 1)" class="btn danger small">
                <SlTrash />
              </button>
            </InputBlock>
          </div>

          <div class="text-center">

            <button type="button" class="btn secondary small" @click="updateChallengeForm.questions.push('')">
              <SlPlus />
            </button>
          </div>

          <input type="submit" class="btn primary" value="Save Questions" />
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

    <Modal title="Delete a team" ref="deleteTeamModal">

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


    <Modal title="Add a new team" ref="addTeamModal">
      <Alert type="danger" v-if="createTeamStatus === 'error'">
        Sorry, we encountered an error trying to create a new team
      </Alert>
      <Alert type="success" v-if="createTeamStatus === 'success'">
        Team {{ createTeamLastName }} created successfully!
      </Alert>
      <form @submit="createTeam">
        <InputBlock placeholder="team name..." v-model="createTeamForm.name" label="Type a name for the new team">
        </InputBlock>
        <input type="submit" class="btn primary" value="Create Team">
      </form>
    </Modal>

    <Modal title="Edit challenge settings" ref="editChallengeModal">
      <Alert type="danger" v-if="updateChallengeStatus === 'error'">
        We encountered an error trying to edit challenge details
      </Alert>
      <Alert type="success" v-if="updateChallengeStatus === 'success'">
        Challenge details saved successfully
      </Alert>
      <form @submit="saveChallenge">
        <InputBlock v-model="updateChallengeForm.name" placeholder="name" label="Challenge name" />
        <DateTimeInput v-model="updateChallengeForm.startTime" label="Starting Time"></DateTimeInput>
        <DateTimeInput v-model="updateChallengeForm.endTime" label="Ending Time"></DateTimeInput>
        <input type="submit" class="btn primary" value="Save">
      </form>
    </Modal>
  </div>

</template>
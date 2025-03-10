<script lang="ts">
import Modal from '@/components/Modal.vue';
import {SlPencil, SlPlus, SlTrash} from 'vue-icons-plus/sl';
import Tabs from "@/components/Tabs.vue";
import InputBlock from "@/components/InputBlock.vue";
import Alert from "@/components/Alert.vue";
import DateTimeInput from "@/components/DateTimeInput.vue";
import CheckBox from "@/components/CheckBox.vue";
import Badge from "@/components/Badge.vue";

export default {
  components: {Badge, CheckBox, DateTimeInput, Alert, InputBlock, Tabs, SlPencil, SlPlus, SlTrash, Modal},
  data: () => ({
    status: 'loading',
    teams: [],
    participations: [],
    page: 'overview',
    challenge: {},
    createTeamForm: {
      name: '',
      challengeId: '',
    },
    createTeamStatus: '',
    createTeamLastName: '',
    updateChallengeForm: {},
    updateChallengeStatus: '',
    submitChallengeError: false
  }),
  methods: {
    saveChallenge(e) {
      e.preventDefault();
      this.$http.put("/ChallengeAdmin/UpdateChallenge", this.updateChallengeForm).then(() => {
        this.updateChallengeStatus = 'success';
        this.challenge = JSON.parse(JSON.stringify(this.updateChallengeForm));
        setTimeout(() => this.updateChallengeStatus = '', 20000);
      }).catch(() => {
        this.updateChallengeStatus = 'error';
        setTimeout(() => this.updateChallengeStatus = '', 20000);
      })
    },
    submitChallenge() {
      let payload = JSON.parse(JSON.stringify(this.challenge));
      payload.runningStatus = 1; // status = SUBMITTED
      this.$http.put("/ChallengeAdmin/UpdateChallenge", payload).then(() => {
        this.challenge = payload;
        this.$refs.submitChallengeModal.hide();
      }).catch(() => {
        this.submitChallengeError = true;
        setTimeout(() => this.submitChallengeError = false, 20000);
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

      this.challenge.startTime = new Date(this.challenge.startTime);
      this.challenge.endTime = new Date(this.challenge.endTime);

      this.status = 'ok';
    }).catch(() => {
      this.$router.push({name: 'challenges'});
    })
  }
}
</script>

<template>

  <div v-if="status === 'ok'">

    <h2>{{ challenge.name }}
      <button v-if="challenge.runningStatus === 0" @click="$refs.editChallengeModal.show()" class="btn btn-primary small">
        <SlPencil/>
      </button>
    </h2>

    <Tabs v-model="page"
          :tabs="[{text: 'Challenge Overview', id: 'overview'}, {text: 'Teams', id: 'teams'}, {text: 'Participations', id: 'participations'}, {text: 'Challenge Options', id: 'options'}]">
      <div v-if="page === 'overview'">
        <table class="text-left">
          <tr>
            <th>Running status</th>
            <td>
              <Badge v-if="challenge.runningStatus === 0" type="info">DRAFT</Badge>
              <Badge v-if="challenge.runningStatus === 1" type="info">SUBMITTED</Badge>
              <Badge v-if="challenge.runningStatus === 2" type="info">FROZEN</Badge>
              <Badge v-if="challenge.runningStatus === 3" type="info">TERMINATED</Badge>
            </td>
          </tr>
          <tr>
            <th>Starting Time:</th>
            <td>{{ challenge.startTime.toLocaleString() }}</td>
          </tr>
          <tr>
            <th>Ending Time:</th>
            <td>{{ challenge.endTime.toLocaleString() }}</td>
          </tr>
        </table>
      </div>
      <div v-if="page === 'teams'">

        <h3>Teams
          <button @click="$refs.addTeamModal.show()" class="btn primary small">
            <SlPlus/>
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
                <SlTrash/>
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
            <Badge type="warning">not saved</Badge>
          </h4>
          
          <CheckBox>Challenge will be visibile on active challenges page</CheckBox>
          <CheckBox>Allow anonymous joins</CheckBox>
          <CheckBox>Allow joiners to remove answers</CheckBox>
          <CheckBox>Start challenge automatically at <i>starting time</i></CheckBox>
          
          <button class="btn primary">Save flags</button>
          
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
        </div>

        
        
        <h4>Delete this challenge</h4>
        <p>
          If you request the challenge deletion, you will be not able to recover it.
        </p>
        <button class="btn danger" @click="$refs.deleteChallengeModal.show()">Delete Challenge</button>
      </div>
    </Tabs>

    <Modal title="Delete this challenge" ref="deleteChallengeModal">
      Are you sure to delete <b>{{ challenge.name }}</b> challenge?
      <InputBlock>
        Type the challenge name
      </InputBlock>
      <input :disabled="false" type="submit" class="btn danger" value="Delete this challenge">
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

      <button class="btn warning" @click="submitChallenge()">Submit challenge</button>

    </Modal>


    <Modal title="Add a new team" ref="addTeamModal">
      <Alert type="danger" v-if="createTeamStatus === 'error'">
        Sorry, we encountered an error trying to create a new team
      </Alert>
      <Alert type="success" v-if="createTeamStatus === 'success'">
        Team {{ createTeamLastName }} created successfully!
      </Alert>
      <form @submit="createTeam">
        <InputBlock placeholder="team name..." v-model="createTeamForm.name">Type a name for the new team</InputBlock>
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
        <InputBlock v-model="updateChallengeForm.name" placeholder="name">
          Challenge name
        </InputBlock>
        <DateTimeInput v-model="updateChallengeForm.startTime">
          Starting Time
        </DateTimeInput>
        <DateTimeInput v-model="updateChallengeForm.endTime">
          Ending Time
        </DateTimeInput>
        <input type="submit" class="btn primary" value="Save">
      </form>
    </Modal>
  </div>

</template>
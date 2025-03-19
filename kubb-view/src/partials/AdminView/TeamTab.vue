<template>

  <h3>Teams
    <button v-if="challenge.runningStatus < 2" @click="$refs.addTeamModal.show()" class="btn primary small">
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
        <td>
          <span v-if="item.ownerEmail !== null">
            {{ item.ownerName }}
            <i>({{ item.ownerEmail }})</i>
          </span>
          <span v-else>you</span>
        </td>
        <td>
          <button v-if="challenge.runningStatus < 2" class="btn small danger">
            <SlTrash />
          </button>
        </td>

      </tr>
    </tbody>
  </table>

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
</template>

<script>
import Alert from '@/components/Alert.vue';
import InputBlock from '@/components/InputBlock.vue';
import Modal from '@/components/Modal.vue';
import { SlPlus, SlTrash } from 'vue-icons-plus/sl';

export default {
  props: ['teams', 'challenge'],
  components: { Modal, SlPlus, SlTrash, InputBlock, Alert },
  methods: {
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
  },
  data: () => ({
    createTeamForm: {
      name: '',
      challengeId: '',
    },
    createTeamStatus: '',
    createTeamLastName: '',
  }),
  mounted() {
    this.createTeamForm.challengeId = this.$route.query.id;
  }
}
</script>
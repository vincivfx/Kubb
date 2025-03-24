<template>
  <h2>
    {{ challenge.name }}
  </h2>

  <p>
    <SlCalender /> {{ new Date(challenge.startTime).toLocaleString() }} - {{ new
      Date(challenge.endTime).toLocaleString() }}
  </p>

  <p>
    Maximum teams per participation: {{ challenge.maxTeamPerUser }}
  </p>

  <TeamTab :challenge="challenge" :teams="teams" />

</template>

<script>
import Alert from '@/components/Alert.vue';
import InputBlock from '@/components/InputBlock.vue';
import Modal from '@/components/Modal.vue';
import TeamTab from '@/partials/AdminView/TeamTab.vue';
import { SlCalender, SlPlus, SlTrash } from 'vue-icons-plus/sl';


export default {
  components: { SlCalender, SlTrash, SlPlus, Modal, InputBlock, Alert, TeamTab },
  data: () => ({
    challenge: {},
    createTeamForm: {
      name: '',
      challengeId: ''
    },
    teams: [],
    createTeamStatus: '',
    createTeamLastName: ''
  }),
  methods: {

  },
  mounted() {
    this.$http.get("Challenge/GetInfo?challengeId=" + encodeURIComponent(this.$route.query.id)).then(res => {
      this.challenge = res.data.challengeSetup;
      this.teams = res.data.teams;

      this.createTeamForm.challengeId = this.$route.query.id;
    })
  }
}
</script>
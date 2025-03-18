<script>
import {SlPlus} from "vue-icons-plus/sl";
import Modal from "@/components/Modal.vue";
import InputBlock from "@/components/InputBlock.vue";
import Alert from "@/components/Alert.vue";
import ChallengeInfo from "@/components/ChallengeInfo.vue";
import Pagination from "@/components/Pagination.vue";
export default {
  name: "MyChallengesPartialView",
  components: {Pagination, ChallengeInfo, Alert, InputBlock, Modal, SlPlus},
  data: () => ({
    challenges: [],
    createChallengeForm: {
      challengeName: ''
    },
    createChallengeStatus: '',
    filterByName: ''
  }),
  methods: {
    createChallenge(e) {
      e.preventDefault();
      this.$http.post("/ChallengeAdmin/CreateChallenge", this.createChallengeForm).then((response) => {
        this.$router.push({name: 'challenge-admin', query: {id: response.data.challengeId}});
      }).catch((error) => {
        this.createChallengeStatus = 'error';
        setTimeout(() => this.createChallengeStatus = '', 10000);
      });
    }
  },
  mounted() {
    this.$http.get("/Challenge/All").then((response) => {
      this.challenges = response.data;
    })
  }
}
</script>

<template>
  <Modal title="Create new challenge" ref="createChallengeModal">
    <Alert type="danger" v-if="createChallengeStatus === 'error'">
      We encountered an error while creating new challenge. Please try again and check
      if you are allowed to create challenges.
    </Alert>
    <form @submit="createChallenge">
      <InputBlock placeholder="challenge name" v-model="createChallengeForm.challengeName" label="Type the challenge name"></InputBlock>
      <div class="text-right">
        <input type="submit" value="Create new challenge" class="btn primary">
      </div>
    </form>
  </Modal>
  
  <h2 class="d-table-cell" style="padding: 20px 0;">
    My Challenges
    <button class="btn small" @click="$refs.createChallengeModal.show()">
      <SlPlus />
    </button>
  </h2>

  <InputBlock v-model="filterByName" label="Filter by name" placeholder="name..." />
  
  <ChallengeInfo send="" admin="" v-for="(item, key) in challenges.filter(ch => ch.name.toLowerCase().indexOf(filterByName.toLowerCase()) >= 0)" :challenge="item" :key="key" />
  
  <Pagination />
</template>

<style scoped>

</style>
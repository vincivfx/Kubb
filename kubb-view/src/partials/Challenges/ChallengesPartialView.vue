<script>
import ChallengeInfo from "@/components/ChallengeInfo.vue";

export default {
  name: "ChallengesPartialView",
  components: {ChallengeInfo},
  props: ['repo', 'title'],
  data: () => ({
    challenges: []
  }),
  mounted() {
    this.$http.get("/Home/Challenges/" + this.repo).then((response) => {
      this.challenges = response.data;
    })
  }
}
</script>

<template>
  <h2>{{title}}</h2>
  <ChallengeInfo :challenge="challenge" v-for="(challenge, challengeIndex) in challenges" :key="challengeIndex" />
</template>

<style scoped>

</style>
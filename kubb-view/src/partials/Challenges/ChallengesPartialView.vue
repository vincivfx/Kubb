<script>
import Alert from "@/components/Alert.vue";
import ChallengeInfo from "@/components/ChallengeInfo.vue";
import Pagination from "@/components/Pagination.vue";

export default {
  name: "ChallengesPartialView",
  components: {ChallengeInfo, Pagination, Alert},
  props: ['repo', 'title'],
  data: () => ({
    challenges: [],
    page: 0,
    count: 0
  }),
  methods: {
    loadPage(page) {
      this.$http.get("/Home/" + this.repo + "?offset=" + encodeURIComponent(page)).then((response) => {
        this.challenges = response.data.challenges;
        this.count = response.data.count;
        this.page = page;
      })
    }
  },
  mounted() {
    this.loadPage(0);
  }
}
</script>

<template>
  <h2>{{title}}</h2>
  <ChallengeInfo :challenge="challenge" v-for="(challenge, challengeIndex) in challenges" :key="challengeIndex" />
  <Pagination @move="(i) => loadPage(page + i)" :page="page" :perPage="25" :count="count" />
  <Alert type="warning" v-if="count === 0">
    There are no challenges
  </Alert>
</template>

<style scoped>

</style>
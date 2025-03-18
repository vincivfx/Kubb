<script>
import Modal from '@/components/Modal.vue';
import Tabs from '@/components/Tabs.vue';
import { SlPlus } from 'vue-icons-plus/sl';
import ChallengeInfo from "@/components/ChallengeInfo.vue";
import MyChallengesPartialView from "@/partials/MyChallengesPartialView.vue";
import Pagination from '@/components/Pagination.vue';
import ChallengesPartialView from "@/partials/Challenges/ChallengesPartialView.vue";
import JoinableChallengesPartialView from '@/partials/Challenges/JoinableChallengesPartialView.vue';

export default {
  data: () => ({
    challenges: [],
    page: ''
  }),
  components: {ChallengesPartialView, MyChallengesPartialView, ChallengeInfo, Tabs, SlPlus, Pagination, JoinableChallengesPartialView},
  mounted() {
    this.page = this.$route.params.page ?? '';
  },
  watch: {
    page(newVal) {
      this.$router.push({
        name: 'challenges',
        params: {page: newVal}
      })
    }
  }
}
</script>

<template>
  <Tabs v-model="page" :tabs="[{text: 'Running Challenges', id: ''}, {text: 'Archived Challenges', id: 'archived'}, {text: 'My Challenges', id: 'my', runIf: () => $authSession.getStored()}, {text: 'Joinable Challenges', id: 'join', runIf: () => $authSession.getStored()}]">
    <ChallengesPartialView v-if="page === ''" title="Running Challenges" repo="Challenges" />
    <ChallengesPartialView v-if="page === 'archived'" title="Archived Challenges" repo="Archived"></ChallengesPartialView>
    <MyChallengesPartialView v-if="page === 'my'" />
    <JoinableChallengesPartialView v-if="page === 'join'" />
  </Tabs>
</template>

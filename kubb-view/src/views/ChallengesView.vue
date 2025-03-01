<script>
import Modal from '@/components/Modal.vue';
import Tabs from '@/components/Tabs.vue';
import { SlPlus } from 'vue-icons-plus/sl';
import ChallengeInfo from "@/components/ChallengeInfo.vue";
import MyChallengesPartialView from "@/partials/MyChallengesPartialView.vue";

export default {
  data: () => ({
    challenges: [
      {name: 'Gara di Aprile', organizer: 'Rossi'}
    ],
    page: ''
  }),
  components: {MyChallengesPartialView, ChallengeInfo, Tabs, SlPlus},
  mounted() {
    let validTypes = [undefined, '', 'archive', 'joinable', 'my'];

    if (validTypes.indexOf(this.$route.params.type) === -1) {
      this.$router.push({name: 'error'});
    }

    this.page = this.$route.params.type === undefined ? '' : this.$route.params.type;

    switch(this.$route.params.type) {
      case(undefined):
        this.$http.get('/Home/Challenges').then(response => {
          console.log(response);
        })
        break;
    }
  }
}
</script>

<template>
  <Tabs v-model="page" :tabs="[{text: 'Running Challenges', id: ''}, {text: 'Archived Challenges', id: 'archived'}, {text: 'My Challenges', id: 'my', runIf: () => $authSession.getStored()}]">
    <div v-if="page === ''">
      <h2>Running Challenges</h2>
      <ChallengeInfo :challenge="{name: 'Gara di Aprile', description: '', teams: 50, startTime: 'now', endTime: 'now'}" />
    </div>
    <MyChallengesPartialView v-if="page === 'my'" />
    
    
  </Tabs>
</template>

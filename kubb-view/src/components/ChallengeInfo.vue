<script>
import {RouterLink} from "vue-router";

export default {
  name: "ChallengeInfo",
  props: ['challenge', 'admin'],
  components: {RouterLink}
}
</script>

<template>
  <div class="b-1">
    <div class="bg-primary p-2">
      <h3>{{challenge.name}}</h3>
    </div>
    <div class="p-2">
      <p>
        {{challenge.description}}
      </p>
      <p>
        from {{challenge.startTime}}
        to {{challenge.endTime}}
      </p>
      <p>
        50 teams
      </p>
      <div class="text-right btn-group-right">
        <RouterLink :to="{name: 'challenge-sender', query: {id: challenge.challengeId}}" class="btn primary">Send answers</RouterLink>
        <RouterLink :to="{name: 'challenge-admin', query: {id: challenge.challengeId}}" class="btn primary" v-if="admin !== undefined && admin !== false">Administrator</RouterLink>
        <RouterLink :to="{name: 'challenge-score', query: {id: challenge.challengeId}}" class="btn primary">Follow</RouterLink>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
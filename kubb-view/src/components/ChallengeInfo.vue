<script>
import {RouterLink} from "vue-router";
import Badge from "@/components/Badge.vue";

export default {
  name: "ChallengeInfo",
  props: ['challenge', 'admin'],
  components: {Badge, RouterLink}
}
</script>

<template>
  <div class="b-1 mv-2">
    <div :class="['p-2', {'bg-primary': challenge.runningStatus <= 1}, {'bg-gray': challenge.runningStatus > 1}]">
      <h3>{{challenge.name}} <Badge type="warning" v-if="challenge.runningStatus === 0">(DRAFT)</Badge></h3>
    </div>
    <div class="p-2">
      <p>
        {{challenge.description}}
      </p>
      <p>
        from {{new Date(challenge.startTime).toLocaleString()}}
        to {{new Date(challenge.endTime).toLocaleString()}}
      </p>
      <p>
        50 teams
      </p>
      <div class="text-right btn-group-right">
        <RouterLink v-if="challenge.runningStatus === 1" :to="{name: 'challenge-sender', query: {id: challenge.challengeId}}" class="btn primary">Send answers</RouterLink>
        <RouterLink v-if="challenge.runningStatus < 3 && admin !== undefined && admin !== false" :to="{name: 'challenge-admin', query: {id: challenge.challengeId}}" class="btn primary">Manage</RouterLink>
        <RouterLink v-if="challenge.runningStatus > 0" :to="{name: 'challenge-score', query: {id: challenge.challengeId}}" class="btn primary">Follow</RouterLink>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
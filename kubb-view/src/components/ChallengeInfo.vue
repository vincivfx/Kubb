<script>
import {RouterLink} from "vue-router";
import Badge from "@/components/Badge.vue";

export default {
  name: "ChallengeInfo",
  props: ['challenge', 'admin', 'send', 'guest'],
  components: {Badge, RouterLink}
}
</script>

<template>
  <div class="b-1 mv-2">
    <div :class="['p-2', {'bg-primary': challenge.runningStatus <= 2}, {'bg-gray': challenge.runningStatus > 2}]">
      <h3>{{challenge.name}} 
        <Badge type="warning" v-if="challenge.runningStatus === 0">DRAFT</Badge>
        <Badge type="secondary" v-if="challenge.runningStatus === 1">SUBMITTED</Badge>
        <Badge type="secondary" v-if="challenge.runningStatus === 2">RUNNING</Badge>
        <Badge type="secondary" v-if="challenge.runningStatus === 3">FROZEN</Badge>
        <Badge type="secondary" v-if="challenge.runningStatus === 4">TERMINATED</Badge>
      </h3>
    </div>
    <div class="p-2">
      <p>
        {{challenge.description}}
      </p>
      <p>
        Starting at {{new Date(challenge.startTime).toLocaleString()}}<br>
        Ending at {{new Date(challenge.endTime).toLocaleString()}}
      </p>
      <div class="text-right btn-group-right">
        <RouterLink v-if="send !== false && send !== undefined && [1, 2].indexOf(challenge.runningStatus) >= 0" :to="{name: 'challenge-sender', query: {id: challenge.challengeId}}" class="btn primary">Send answers</RouterLink>
        <RouterLink v-if="challenge.runningStatus < 4 && admin !== undefined && admin !== false" :to="{name: 'challenge-admin', query: {id: challenge.challengeId}}" class="btn primary">Manage</RouterLink>
        <RouterLink v-if="challenge.runningStatus < 2 && guest !== undefined && guest !== false" :to="{name: 'guest-admin', query: {id: challenge.challengeId}}" class="btn primary">Manage</RouterLink>
        <RouterLink v-if="challenge.runningStatus > 0" :to="{name: 'challenge-score', query: {id: challenge.challengeId}}" class="btn primary">
          {{ challenge.runningStatus <= 2 ? 'Follow' : 'See Scoreboard' }}
        </RouterLink>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
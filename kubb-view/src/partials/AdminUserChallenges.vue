<template>
  <div class="table-resposive">
    <table class="table">
      <thead>
        <tr>
          <th colspan="2">Challenge name</th>
          <th colspan="2">Date</th>
          <th>Options</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, key) in challenges" :key="key">
          <td>
            {{ item.name }}
          </td>
          <td>
            <Badge v-if="item.runningStatus === 0" type="secondary">DRAFT</Badge>
            <Badge v-if="item.runningStatus === 1" type="secondary">SUBMITTED</Badge>
            <Badge v-if="item.runningStatus === 2" type="secondary">RUNNING</Badge>
            <Badge v-if="item.runningStatus === 3" type="secondary">FROZEN</Badge>
            <Badge v-if="item.runningStatus === 4" type="secondary">TERMINATED</Badge>
          </td>
          <td>
            {{new Date(item.startTime).toLocaleString()}}
          </td>
          <td>
            {{new Date(item.endTime).toLocaleString()}}
          </td>
          <td>
            <RouterLink class="btn small secondary" :to="{name: 'challenge-score', query: {id: item.challengeId}}">live</RouterLink>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import { RouterLink } from 'vue-router';
import Badge from "@/components/Badge.vue";

export default {
  components: {Badge, RouterLink },
  props: ["userId"],
  data: () => ({
    challenges: []
  }),
  mounted() {
    this.$http.get("Admin/GetUserChallenges?userId=" + encodeURIComponent(this.userId)).then(r => {
      this.challenges = r.data;
    })
  }
}
</script>
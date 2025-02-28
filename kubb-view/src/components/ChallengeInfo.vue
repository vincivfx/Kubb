<script>
export default {
  name: "ChallengeInfo",
  props: ['challenge'],
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
      <div class="text-right">
        <button class="btn primary">Follow</button>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
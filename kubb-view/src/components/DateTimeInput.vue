<script>
import InputBlock from "@/components/InputBlock.vue";

export default {
  name: "DateTimeInput",
  components: {InputBlock},
}
</script>

<template>
  <InputBlock v-model="modelValue" placeholder="dd/mm/YYYY HH:MM" ><slot /></InputBlock>
</template>

<style scoped>

</style>
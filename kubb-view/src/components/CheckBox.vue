<script>
export default {
  name: "CheckBox",
  props: ['modelValue', 'disabled'],
  data: () => ({
    currentValue: false,
  }),
  methods: {
    switchValue() {
      if (this.disabled) return;
      this.currentValue = !this.currentValue;
      this.$emit('update:modelValue', this.currentValue);
      this.$emit('change', this.currentValue);
    }
  },
  mounted() {
    this.currentValue = this.modelValue;
  }
}
</script>

<template>
  <div class="mv-2">
    <input style="display: none" v-model="currentValue" type="checkbox" />{{ c }}
    <button :disabled="disabled" type="button" :class="['btn checkbox', { 'checkbox-active': currentValue }]"
      class="btn-checkbox" @click="switchValue">
      <svg v-if="currentValue" viewBox="0 0 30 30" xmlns="http://www.w3.org/2000/svg">
        <line x1="5" y1="5" x2="25" y2="25" stroke="black" stroke-width="4" />
        <line x1="25" y1="5" x2="5" y2="25" stroke="black" stroke-width="4" />
      </svg>
      &nbsp;
    </button>

    <label @click="switchValue">
      <slot />
    </label>
  </div>
</template>

<style scoped></style>
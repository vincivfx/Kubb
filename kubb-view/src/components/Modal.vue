<template>
  <div v-if="visible">
    <div class="modal-background"></div>
    <div class="modal">
      <div class="modal-content">
        <div class="modal-header">
          {{ title }}

          <button class="modal-button-close" @click="hide();">
            <svg viewBox="0 0 40 40" xmlns="http://www.w3.org/2000/svg">
              <line x1="10" y1="10" x2="30" y2="30" stroke="white" stroke-width="4" />
              <line x1="30" y1="10" x2="10" y2="30" stroke="white" stroke-width="4" />
            </svg>
          </button>
        </div>
        <div class="modal-body">
          <slot />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { SlClose } from 'vue-icons-plus/sl';

export default {
  props: ['title'],
  components: { SlClose },
  data: () => ({
    visible: false
  }),
  methods: {
    show() { this.visible = true; },
    hide() { this.visible = false; }
  }
}
</script>
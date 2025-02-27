<template>
    <div v-if="visible" :class="['alert', type]">
        <div class="icon">
            <SlExclamation v-if="type === 'danger'" />
            <SlExclamation v-if="type === 'warning'" />
            <SlCheck v-if="type === 'success'" />
        </div>
        <div class="alert-content">
            <slot />
        </div>

        
    </div>
</template>

<script>
import { SlCheck, SlClose, SlExclamation } from 'vue-icons-plus/sl';

export default {
    props: ['type', 'invisible'],
    components: {SlExclamation, SlClose, SlCheck},
    data: () => ({
        visible: true
    }),
    methods: {
        show() {
            this.visible = true;
        },
        hide() {
            this.visible = false;
        }
    },
    mounted() {
        if (this.invisible !== undefined && this.invisible !== false) this.visible = false;
    }
}
</script>
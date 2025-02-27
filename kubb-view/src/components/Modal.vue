<template>
    <div v-if="visible">
        <div class="modal-background"></div>
        <div class="modal">
            <div class="modal-header">
                {{ title }}

                <button class="modal-button-close" @click="hide();"><SlClose /> </button>
            </div>
            <div class="modal-content">
                <slot />
            </div>
        </div>
    </div>
</template>

<script>
import { SlClose } from 'vue-icons-plus/sl';

export default {
    props: ['title'],
    components: {SlClose},
    data: () => ({
        visible: false
    }),
    methods: {
        show() {this.visible = true;},
        hide() {this.visible = false;}
    }
}
</script>
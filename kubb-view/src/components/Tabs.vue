<template>

    <div class="tabs">

        <div class="tabs-header">
            <span v-for="(item, key) in tabs" :key="key">
                <button v-if="!item.runIf || item.runIf()" @click="goto(key)" :class="[{'selected': modelValue === item.id}]">{{ item.text }}</button>
            </span>
        </div>

        <div class="tabs-container">
            <slot />
        </div>

    </div>

</template>

<script>
export default {
    props: ['tabs', 'modelValue'],
    methods: {
        goto(key) {
            if (this.tabs[key].onExit) {
                this.tabs[key].onExit();
            }
            this.$emit('update:modelValue', this.tabs[key].id)
        }
    },
}
</script>
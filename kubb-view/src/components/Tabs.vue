<template>

    <div class="tabs">

        <div class="tabs-header">
            <span v-for="(item, key) in tabs">
                <button v-if="!item.runIf || item.runIf()" @click="goto(item.id)" :class="[{'selected': page === item.id}]">{{ item.text }}</button>
            </span>
        </div>

        <div class="tabs-container">
            <slot />
        </div>

    </div>

</template>

<script>
export default {
    props: ['tabs'],
    data: () => ({
        page: ''
    }),
    methods: {
        goto(key) {
            this.page = key;
            this.$emit('update:modelValue', key)
        }
    },
    mounted() {
        this.page = this.modelValue;
    }
}
</script>
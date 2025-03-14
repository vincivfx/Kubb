<script lang="ts">
import InputBlock from './InputBlock.vue';
export default {
    components: {InputBlock},
    props: ['options', 'label', 'modelValue'],
    data: () => ({
        visibleSelect: false,
        filter: ''
    }),
    methods: {
        hideSelect() {
            setTimeout(() => this.visibleSelect = false, 250)
        },
        select(item) {
            this.filter = item.text;
            this.$emit('update:modelValue', item.key);
        },
        write() {
            this.$emit('update:modelValue', this.filter);
            console.log(this.filter);
        },
        clear() {
            this.filter = '';
        }
    },
    watch: {
        modelValue(newValue) {
            if (newValue === "") this.filter = "";
        }
    }
}

</script>

<template>

    <div class="input-select">
        <InputBlock @keyup="write" @change="write" v-model="filter" @focusout="hideSelect" @focus="visibleSelect = true" :label="label" />
        <div v-if="visibleSelect" class="select-box">
            <button type="button" @click="select(item)" v-for="(item, key) in options.filter(t => t.text.indexOf(filter) >= 0)" :key="key">
                {{ item.text }}
            </button>
        </div>
    </div>

</template>
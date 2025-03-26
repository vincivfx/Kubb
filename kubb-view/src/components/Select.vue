<script lang="ts">
import InputBlock from './InputBlock.vue';
export default {
    components: { InputBlock },
    props: ['options', 'label', 'modelValue', 'readonly'],
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
            this.$emit('change', item.key);
            this.$emit('update:modelValue', item.key);
        },
        write() {
            if (this.readonly === false || this.readonly === undefined)
                this.$emit('update:modelValue', this.filter);
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
        <InputBlock @keyup="write" @change="write" v-model="filter" @focusout="hideSelect" @focus="visibleSelect = true"
            :label="label">
        </InputBlock>

        <div v-if="visibleSelect" class="select-box">
            <button type="button" @click="select(item)"
                v-for="(item, key) in options.filter(t => t.text.indexOf(filter) >= 0)" :key="key">
                {{ item.text }}
            </button>
        </div>
    </div>

</template>
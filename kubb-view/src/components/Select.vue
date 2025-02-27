<script lang="ts">
import InputBlock from './InputBlock.vue';
export default {
    components: {InputBlock},
    props: ['options']
}

</script>

<template>

    <div class="input-select">
        <InputBlock><slot /></InputBlock>
        <div class="select-box">
            <button v-for="(item, key) in options">
                {{ item }}
            </button>
        </div>
    </div>

</template>
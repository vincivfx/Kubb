<script setup lang="ts">
import InputBlock from '@/components/InputBlock.vue';
import Select from '@/components/Select.vue';

</script>

<template>

    <h3>Send answers</h3>
    <Select :options="['ciao', 'come', 'stai']">
        Select team
    </Select>

    <Select :options="[1,2,3,4]">
        Question
    </Select>

    <InputBlock>
        Answer to question
    </InputBlock>

    <button class="btn primary">Send</button>

</template>
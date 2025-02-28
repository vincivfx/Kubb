<script setup lang="ts">
import InputBlock from '@/components/InputBlock.vue';
import Select from '@/components/Select.vue';

</script>

<template>

    <h3>Send answers</h3>
    <Select :options="[{text: 'Hello', key: 'h'}, {text: 'Ciao', key: 'c'}]">
        Select team
    </Select>

    <Select :options="[{text: '#1 Problema', key: 0}]">
        Question
    </Select>

    <InputBlock>
        Answer to question
    </InputBlock>

    <button class="btn primary">Send</button>

</template>
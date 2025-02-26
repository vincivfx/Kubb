<script setup lang="ts">
import InputBlock from '@/components/InputBlock.vue';

</script>

<template>
    <h2>Kubb Contest Platform :: Registration</h2>
    <div class="rows">
        <div class="col">
            <InputBlock>Type your email address</InputBlock>
            <InputBlock>Type your first name</InputBlock>
            <InputBlock>Type your last name</InputBlock>
        </div>
        <div class="col">
            <InputBlock>Choose a secure password</InputBlock>
            <InputBlock>Type again your password</InputBlock>
        </div>
    </div>

    

    <input type="submit" class="btn primary" value="Register">
</template>
<script setup lang="ts">
import Alert from '@/components/Alert.vue';
import InputBlock from '@/components/InputBlock.vue';

</script>

<template>
    <h2>Kubb Contest Platform :: Registration</h2>

    <Alert v-if="!$settings.registration" type="warning">
        Registration to KCP has been disabled by administrator
    </Alert>

    <div v-if="$settings.registration">
        <div class="rows">
            <div class="col">
                <InputBlock label="Type your email address" />
                <InputBlock label="Type your first name" />
                <InputBlock label="Type your last name" />
            </div>
            <div class="col">
                <InputBlock label="Choose a secure password" />
                <InputBlock label="Type again your password" />
            </div>
        </div>



        <input type="submit" class="btn primary" value="Register">
    </div>
</template>
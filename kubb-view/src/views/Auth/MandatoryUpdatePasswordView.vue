<script setup lang="ts">
import Alert from '@/components/Alert.vue';
import InputBlock from '@/components/InputBlock.vue';
import PasswordSecurityCheck from '@/components/PasswordSecurityCheck.vue';

</script>

<template>
    <h2>Update your password</h2>
    <Alert type="warning">
        Due to security reasons, you must update your password now!
    </Alert>

    <form>
        <InputBlock>
            
        </InputBlock>
        <PasswordSecurityCheck />
        <InputBlock />
        <Alert type="danger">
            Password and its confirmation should be the same.
        </Alert>
        <input type="submit" class="btn primary" value="Update my password">
    </form>
</template>
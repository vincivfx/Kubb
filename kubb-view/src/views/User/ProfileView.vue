<script setup lang="ts">
import InputBlock from '@/components/InputBlock.vue';

</script>

<template>
    <h2>
        Manage your account, Vincenzo
    </h2>
    
    <div class="rows">
        <div class="col">
            <h3>Update your Password</h3>

            <form>
                <InputBlock>Current password</InputBlock>
                <InputBlock>Type your new password</InputBlock>
                <InputBlock>Confirm your password</InputBlock>

                <input type="submit" class="btn primary" value="Update password">
            </form>
        </div>

        <div class="col">
            <h3>Logins</h3>
        </div>
    </div>
</template>
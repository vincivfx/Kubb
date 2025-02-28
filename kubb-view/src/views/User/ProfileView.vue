<script lang="ts">
import InputBlock from '@/components/InputBlock.vue';
import PasswordSecurityCheck from '@/components/PasswordSecurityCheck.vue';
export default {
    data: () => ({
        profile: {}
    }),
    components: {InputBlock, PasswordSecurityCheck},
    methods: {
        async logout() {

        },
        async updatePassword(e) {
            e.preventDefault()
        }
    }
}
</script>

<template>

    <h2>
        Manage your account, {{ $authSession.getName() }},
        <button @click="logout()" class="btn small">
            Logout
        </button>
    </h2>
    
    <div class="rows">
        <div class="col">
            <h3>Update your Password</h3>

            <form @submit="updatePassword">
                <InputBlock>Current password</InputBlock>
                <InputBlock>Type your new password</InputBlock>

                <PasswordSecurityCheck />

                <InputBlock>Confirm your password</InputBlock>

                <input type="submit" class="btn primary" value="Update password">
            </form>
        </div>

        <div class="col">
            <h3>Logins</h3>
        </div>
    </div>
</template>
<script>
import Alert from '@/components/Alert.vue';
import InputBlock from '@/components/InputBlock.vue';
import PasswordSecurityCheck from '@/components/PasswordSecurityCheck.vue';

export default {
    components: { InputBlock, PasswordSecurityCheck, Alert },
    data: () => ({
        updatePasswordForm: {
            password: '',
            emailAddress: '',
            token: ''
        },
        confirmPassword: '',
        status: ''
    }),
    methods: {
        updatePassword(e) {
            e.preventDefault();
            this.$http.post("Auth/CompleteRecoverPassword", this.updatePasswordForm).then(() => this.status = 'success')
                .catch(() => this.status = 'error');
        }
    },
    mounted() {
        this.updatePasswordForm = {
            password: '',
            emailAddress: this.$route.query.email,
            token: this.$route.query.token
        }
    }
}
</script>

<template>
    <h2>Recover your Account</h2>
    <Alert v-if="status === 'error'" type="danger">
        We encountered an error while updating your password, please check again
        your current password.
    </Alert>

    <Alert v-if="status === 'success'" type="success">
        Password have been updated successfully!
    </Alert>

    <form @submit="updatePassword" v-if="status === ''">
        <InputBlock disabled="" v-model="updatePasswordForm.emailAddress" label="Email address" />
        <InputBlock v-model="updatePasswordForm.password" type="password" placeholder="new password..."
            label="Type your new password" />
        <PasswordSecurityCheck :passwd="updatePasswordForm.password" />
        <InputBlock v-model="confirmPassword" type="password" placeholder="new password..."
            label="Type again your new password" />
        <Alert :type="updatePasswordForm.password === confirmPassword ? 'success' : 'danger'">
            Password and its confirmation should be the same.
        </Alert>

        <input type="submit" class="btn primary" value="Update my password">
    </form>
</template>
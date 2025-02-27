<script lang="ts">
import Alert from '@/components/Alert.vue';
import InputBlock from '@/components/InputBlock.vue';
import Modal from '@/components/Modal.vue';

export default {
    components: {InputBlock, Modal, Alert},
    data: () => ({
        loginForm: {
            emailAddress: '',
            password: ''
        },
        recoverPasswordForm: {
            emailAddress: ''
        },
        recoveryStatus: 'none',
    }),
    methods: {
        async recoverPassword(e) {
            e.preventDefault();
            this.$http.post('/Auth/RecoverPassword', this.recoverPasswordForm).then(response => {
                this.recoveryStatus = 'success';
            }).catch(error => {
                this.recoveryStatus = 'error';
            })
        },
        async login(e) {
            e.preventDefault();
            this.$http.post('/Auth/Login', this.loginForm).then((response) => {
            }).catch((error) => {
                if (error.response.status !== 500) {
                    this.$refs.wrongLoginAlert.show();
                }
            });
        },
    }
}
</script>

<template>
    
    <div class="container">
        <h2>Kubb Contest Platform :: Login</h2>
        <p>

        </p>

        <Alert ref="wrongLoginAlert" type="danger" invisible="" dismissible="">
            Account not found or wrong password, please check your credentials 
        </Alert>

        <form @submit="login">
            <InputBlock v-model="loginForm.emailAddress">Type your email address</InputBlock>
            <InputBlock v-model="loginForm.password">Type your password</InputBlock>
            <p>
                Have you forgotten your password?
                <button type="button" class="btn link" @click="$refs.recoverPassword.show(); recoveryStatus = 'none';">Recover your password</button>
            </p>


            <input type="submit" class="btn primary" value="Login">
        </form>
    </div>

    <div class="container" v-if="$settings.registration">
        <h3>Register into Kubb Contest Platform</h3>
        <p>
            Have you never tried KCP before? Register right now!
        </p>
        <div class="text-right">
            <RouterLink :to="{name: 'register'}" class="btn secondary">Register right now!</RouterLink>
        </div>
    </div>

    <Modal ref="recoverPassword" title="Recover your password">
        <Alert type="success" v-if="recoveryStatus === 'success'">
            We will send you in a moment an email with a temporary password.
            You will login with that password and then you will be required to update it.
        </Alert>
        <Alert type="danger" v-if="recoveryStatus === 'error'">
            We encountered a problem trying to recover your password.
            Maybe the email address is wrong or you have requested the recovery
            just some moments ago.
        </Alert>
        
        <form @submit="recoverPassword" v-if="recoveryStatus === 'none'">
            
            <InputBlock v-model="recoverPasswordForm.emailAddress">Type your email address</InputBlock>
            <input type="submit" class="btn primary" value="Recover password">
        </form>
    </Modal>



</template>
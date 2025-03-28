<script lang="ts">
import Alert from '@/components/Alert.vue';
import InputBlock from '@/components/InputBlock.vue';
import Modal from '@/components/Modal.vue';
import VueTurnstile from 'vue-turnstile';

export default {
    components: {InputBlock, Modal, Alert, VueTurnstile},
    data: () => ({
        loginForm: {
            emailAddress: '',
            password: '',
            turnstileToken: ''
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
            this.$refs.wrongLoginAlert.hide();
            this.$http.post('/Auth/Login', this.loginForm).then((response) => {
                if (response.status === 200) {
                    this.$authSession.setStored(response.data);
                    if (response.data.mustChangePassword) {
                      this.$router.push({name: 'mandatory-update-password'});
                    } else {
                      this.$router.push({name: 'profile'});
                    }
                } else {
                    this.$refs.wrongLoginAlert.show();
                    this.$refs.loginTurnstile.reset();
                }
            }).catch((error) => {
                if (error.response && error.response.status !== 500) {
                    this.$refs.wrongLoginAlert.show();
                    this.$refs.loginTurnstile.reset();
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
            <InputBlock placeholder="email address..." type="email" required v-model="loginForm.emailAddress" label="Type your email address"></InputBlock>
            <InputBlock placeholder="password..." type="password" required v-model="loginForm.password" label="Type your password"></InputBlock>
            <p>
                Have you forgotten your password?
                <button type="button" class="btn link" @click="$refs.recoverPassword.show(); recoveryStatus = 'none';">Recover your password</button>
            </p>

            <VueTurnstile v-if="$settings.enableTurnstile" ref="loginTurnstile" v-model="loginForm.turnstileToken" :site-key="$turnstileSiteKey"  />

            <input type="submit" class="btn primary" value="Login">
        </form>
    </div>

    <div class="container" v-if="$settings.enableRegistration">
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
            
            <InputBlock placeholder="email address..." type="email" required v-model="recoverPasswordForm.emailAddress" label="Type your email address"></InputBlock>
            
            <VueTurnstile v-if="$settings.enableTurnstile" v-model="recoverPasswordForm.turnstileToken" :site-key="$turnstileSiteKey"  />

            <input type="submit" class="btn primary" value="Recover password">
        </form>
    </Modal>



</template>
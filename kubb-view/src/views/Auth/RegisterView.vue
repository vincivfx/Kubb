<script lang="ts">
import Alert from '@/components/Alert.vue';
import InputBlock from '@/components/InputBlock.vue';
import VueTurnstile from "vue-turnstile";
import PasswordSecurityCheck from "@/components/PasswordSecurityCheck.vue";

export default {
  components: {PasswordSecurityCheck, Alert, InputBlock, VueTurnstile},
  data: () => ({
    registerForm: {
      turnstileToken: '',
      name: '',
      surname: '',
      password: '',
      emailAddress: '',
      termsAccept: 'I accept'
    },
    passwordConfirmation: '',
    registerStatus: ''
  }),
  methods: {
    register(e) {
      e.preventDefault();
      this.$http.post('Auth/Register', this.registerForm).then(() => {
        this.registerStatus = 'success';
      }).catch(() => {
        this.registerStatus = 'error';
        setInterval(() => this.registerStatus = '', 12000);
        this.$refs.registerTurnstile.reset();
      })
    }
  }
}
</script>

<template>
  <h2>Kubb Contest Platform :: Registration</h2>

  <Alert v-if="!$settings.enableRegistration" type="warning">
    Registration to KCP has been disabled by administrator
  </Alert>
  
  <Alert type="danger" v-if="registerStatus === 'error'">
    Error
  </Alert>
  
  <Alert type="success" v-if="registerStatus === 'success'">
    You have been registered on Kubb. Now please check your email inbox
    and click on the link we have sent you, in order to confirm the registration
  </Alert>

  <form @submit="register" v-if="registerStatus !== 'success'">
    <div v-if="$settings.enableRegistration">
      <div class="rows">
        <div class="col">
          <InputBlock v-model="registerForm.emailAddress" label="Type your email address"/>
          <InputBlock v-model="registerForm.name" label="Type your first name"/>
          <InputBlock v-model="registerForm.surname" label="Type your last name"/>
        </div>
        <div class="col">
          <InputBlock type="password" v-model="registerForm.password" label="Choose a secure password"/>
          <PasswordSecurityCheck :passwd="registerForm.password"/>
          <InputBlock type="password" v-model="passwordConfirmation" label="Type again your password"/>
        </div>
      </div>


      <VueTurnstile v-if="$settings.enableTurnstile" ref="registerTurnstile" v-model="registerForm.turnstileToken" :site-key="$turnstileSiteKey"/>

      <input type="submit" class="btn primary" value="Register">
    </div>
  </form>
</template>
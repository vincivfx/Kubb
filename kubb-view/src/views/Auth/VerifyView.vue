<script>
import InputBlock from "@/components/InputBlock.vue";
import Alert from "@/components/Alert.vue";

export default {
  name: "VerifyView",
  components: {Alert, InputBlock},
  data: () => ({
    verifyStatus: ''
  }),
  mounted() {
    this.$http.post("Auth/CompleteRegistration", {
      registrationToken: this.$route.query.token,
      emailAddress: this.$route.query.email
    }).then(() => {
      this.verifyStatus = 'success';
    }).catch(() => this.verifyStatus = 'error');
  }
}
</script>

<template>
  <h2>Verify your Account</h2>
  <Alert v-if="verifyStatus === 'success'" type="success">
    Your account has been confirmed
    <RouterLink :to="{name: 'auth'}" class="btn small">Login</RouterLink>
  </Alert>
  <Alert v-if="verifyStatus === 'error'" type="danger">
    We encountered a problem trying to complete your registration. Please try again.
  </Alert>
</template>

<style scoped>

</style>
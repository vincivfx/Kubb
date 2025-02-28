<script lang="ts">
import Alert from '@/components/Alert.vue';
import InputBlock from '@/components/InputBlock.vue';
import PasswordSecurityCheck from '@/components/PasswordSecurityCheck.vue';
export default {
  components: {Alert, InputBlock, PasswordSecurityCheck},
  data: () => ({
    updatePasswordForm: {
      currentPassword: '',
      newPassword: '',
    },
    confirmPassword: '',
    error: false
  }),
  methods: {
    updatePassword(e) {
      e.preventDefault();
      
      this.$http.post('/Auth/UpdatePassword', this.updatePasswordForm).then(() => {
        let session = this.$authSession.getStored();
        delete session.mustChangePassword;
        this.$authSession.setStored(session);
        this.$router.push({name: 'profile'});
      }).then(() => {
        this.error = true;
        setTimeout(() => this.error = false, 5000);
      })
      
    }
  }
}
</script>

<template>
    <h2>Update your password</h2>
    <Alert v-if="error" type="danger">
      We encountered an error while updating your password, please check again
      your current password.
    </Alert>
  
    <Alert type="warning">
        Due to security reasons, you must update your password now!
    </Alert>

    <form @submit="updatePassword">
        <InputBlock v-model="updatePasswordForm.currentPassword" placeholder="current password" type="password">
          Please enter again your current password:
        </InputBlock>
      
        <InputBlock v-model="updatePasswordForm.newPassword" type="password" placeholder="new password...">
            Type your new password
        </InputBlock>
        <PasswordSecurityCheck :passwd="updatePasswordForm.newPassword" />
        <InputBlock v-model="confirmPassword" type="password" placeholder="new password...">Type again your new password</InputBlock>
        <Alert :type="updatePasswordForm.newPassword === confirmPassword ? 'success' : 'danger'">
            Password and its confirmation should be the same.
        </Alert>
        <input type="submit" class="btn primary" value="Update my password">
    </form>
</template>
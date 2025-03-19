<script lang="ts">
import InputBlock from '@/components/InputBlock.vue';
import PasswordSecurityCheck from '@/components/PasswordSecurityCheck.vue';
import Tabs from "@/components/Tabs.vue";
import MandatoryUpdatePasswordView from "@/views/Auth/MandatoryUpdatePasswordView.vue";
import Alert from "@/components/Alert.vue";
import Badge from "@/components/Badge.vue";

export default {
  data: () => ({
    profile: {},
    page: 'home',
    updatePasswordForm: {
      currentPassword: '',
      newPassword: '',
    },
    confirmPassword: '',
    updatePasswordStatus: 'none'
  }),
  components: {Badge, Alert, MandatoryUpdatePasswordView, Tabs, InputBlock, PasswordSecurityCheck},
  methods: {
    updatePassword(e) {
      e.preventDefault();

      this.$http.post('/Auth/UpdatePassword', this.updatePasswordForm).then(() => {
        this.updatePasswordStatus = 'success';
        this.updatePasswordForm = {
          currentPassword: '',
              newPassword: '',
        };
        this.confirmPassword = '';
        setTimeout(() => this.updatePasswordStatus = '', 10000);
      }).then(() => {
        this.updatePasswordStatus = 'error';
        setTimeout(() => this.updatePasswordStatus = '', 5000);
      })

    }
  },
  mounted() {
    this.$http.get('/User/GetProfile').then((response) => {
      this.profile = response.data;
    })
  }
}
</script>

<template>

  <h2>
    Manage your account, {{ $authSession.getName() }}
  </h2>

  <Tabs
      v-model="page"
      :tabs="[{'text': 'Account overview', id: 'home'}, {'text': 'Update password', id: 'passwd'}, {text: 'Last logins', id: 'logins'}]">

    <div v-if="page === 'home'">
      <h3>Account Overview</h3>
      <p>
        <b>Name</b>: {{profile.name}} <br>
        <b>Surname</b>: {{profile.surname}} <br>
        <b>Email Address</b>: {{profile.emailAddress}} <br>
      </p>
    </div>
    
    <div v-if="page === 'passwd'">
      <h3>Update your password</h3>
      <Alert v-if="updatePasswordStatus === 'error'" type="danger">
        We encountered an error while updating your password, please check again
        your current password.
      </Alert>
      
      <Alert v-if="updatePasswordStatus === 'success'" type="success">
        Your password has been updated successfully.
      </Alert>

      <form @submit="updatePassword">
        <InputBlock v-model="updatePasswordForm.currentPassword" placeholder="current password" type="password" label="Please enter again your current password:"></InputBlock>

        <InputBlock v-model="updatePasswordForm.newPassword" type="password" placeholder="new password..." label="Type your new password"></InputBlock>
        <PasswordSecurityCheck :passwd="updatePasswordForm.newPassword"/>
        <InputBlock v-model="confirmPassword" type="password" placeholder="new password..." label="Type again your new password"></InputBlock>
        <Alert :type="updatePasswordForm.newPassword === confirmPassword ? 'success' : 'danger'">
          Password and its confirmation should be the same.
        </Alert>
        <input type="submit" class="btn primary" value="Update my password">
      </form>
    </div>

    <div v-if="page === 'logins'">
      <h3>Last logins on my account</h3>
    </div>

  </Tabs>

</template>
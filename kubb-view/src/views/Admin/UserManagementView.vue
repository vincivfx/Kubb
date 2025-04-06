<template>
  <h2>
    User Management
    <button @click="createUserModalShow" class="btn small">
      <SlPlus />
    </button>
  </h2>

  <div class="table-responsive">
    <table style="zoom: 0.7;" class="table hover">
      <thead>
        <tr>
          <th>Name</th>
          <th>Surname</th>
          <th>Email Address</th>
          <th>Status</th>
        </tr>
      </thead>
      <tbody>
        <tr @click="showUserInfo(user)" v-for="(user, userKey) in users" :key="userKey">
          <td>{{ user.name }}</td>
          <td>{{ user.surname }}</td>
          <td>{{ user.emailAddress }}</td>
          <td>
            <Badge v-if="user.status & 32" type="secondary m-1">ADMIN</Badge>
            <Badge v-if="user.status & 16" type="secondary m-1">JOIN</Badge>
            <Badge v-if="user.status & 8" type="secondary m-1">CREATOR</Badge>
            <Badge v-if="user.status & 4" type="secondary m-1">PASSWD</Badge>
            <Badge v-if="user.status & 2" type="secondary m-1">ACTIVE</Badge>
            <Badge v-if="user.status & 1" type="secondary m-1">NEEDS VERIFICATION</Badge>
          </td>
        </tr>
      </tbody>
    </table>
  </div>

  <Pagination @move="(i) => loadPage(page + i)" :page="page" :perPage="25" :count="count" />
  <Modal class="modal-tabs" :title="'Edit user - ' + user.emailAddress" ref="userInfoModal">
    <Tabs v-model="editUserTab" :tabs="[{text: 'Edit User', id: ''}, {text: 'Challenges', id: 'challenges'}]">
      <AdminUserChallenges v-if="editUserTab === 'challenges'" :userId="user.UserId" />
      <div v-if="editUserTab === ''">
        <Alert type="danger" v-if="updateUserStatus === 'error'">
          Cannot save this user
        </Alert>
        <form @submit="updateUser">
          <InputBlock label="Name" v-model="user.name" />
          <InputBlock label="Surname" v-model="user.surname" />

          <CheckBox v-model="flags[1]">Is user active</CheckBox>
          <CheckBox v-model="flags[2]">Must change password after next login</CheckBox>
          <CheckBox v-model="flags[3]">Allow challenge creation</CheckBox>
          <CheckBox v-model="flags[4]">Allow join</CheckBox>
          <CheckBox v-model="flags[5]">Administrator</CheckBox>


          <CheckBox v-model="user.resetPassword">Reset password?</CheckBox>

          <div v-if="user.resetPassword">
            <InputBlock v-model="user.password" label="Type user password" placeholder="password..."
                        type="password" />
            <PasswordSecurityCheck :passwd="user.password" />
          </div>

          <div class="text-right btn-group-right">
            <button type="button" class="btn danger" @click="previewDelete()">Delete this user</button>
            <input type="submit" value="Save user" class="btn primary">
          </div>
        </form>
      </div>
    </Tabs>
  </Modal>

  <Modal title="Delete user" ref="deleteUserModal">
    <div class="text-center">
      Are you sure to delete {{ user.name }} {{ user.surname }} (<b>{{ user.emailAddress }}</b>)
      <div class="btn-group-right m-2">
        <button @click="deleteAccount()" class="btn danger">Yes, delete account</button>
        <button @click="$refs.deleteUserModal.hide()" class="btn">Cancel</button>
      </div>
    </div>
  </Modal>

  <Modal title="Create user" ref="createUserModal">
    <Alert type="danger" v-if="createUserStatus === 'error'">
      Cannot create this user
    </Alert>
    <form @submit="createUser">
      <InputBlock v-model="createUserForm.name" label="Type user name" placeholder="name..." />
      <InputBlock v-model="createUserForm.surname" label="Type user surname" placeholder="surname..." />
      <InputBlock v-model="createUserForm.emailAddress" label="Type user email address" placeholder="email address..."
        type="email" />

      <CheckBox v-model="createUserForm.flags[1]">Is user active</CheckBox>
      <CheckBox v-model="createUserForm.flags[2]">Must change password after next login</CheckBox>
      <CheckBox v-model="createUserForm.flags[3]">Allow challenge creation</CheckBox>
      <CheckBox v-model="createUserForm.flags[4]">Allow join</CheckBox>
      <CheckBox v-model="createUserForm.flags[5]">Administrator</CheckBox>

      <CheckBox v-model="createUserForm.sendVerificationEmail">Send verification email</CheckBox>
      <CheckBox v-model="createUserForm.sendRandomPassword">Send random password via email</CheckBox>

      <div v-if="!createUserForm.sendRandomPassword">
        <InputBlock v-model="createUserForm.password" label="Type user password" placeholder="password..."
          type="password" />
        <PasswordSecurityCheck :passwd="createUserForm.password" />
      </div>

      <input type="submit" class="btn primary" value="Create user">
    </form>
  </Modal>
</template>

<script>
import Alert from '@/components/Alert.vue';
import Badge from '@/components/Badge.vue';
import CheckBox from '@/components/CheckBox.vue';
import InputBlock from '@/components/InputBlock.vue';
import Modal from '@/components/Modal.vue';
import Pagination from '@/components/Pagination.vue';
import PasswordSecurityCheck from '@/components/PasswordSecurityCheck.vue';
import AdminUserChallenges from '@/partials/AdminUserChallenges.vue';
import { SlPlus } from 'vue-icons-plus/sl';
import Tabs from "@/components/Tabs.vue";

export default {
  components: {Tabs, AdminUserChallenges, Alert, Pagination, Badge, Modal, InputBlock, CheckBox, SlPlus, PasswordSecurityCheck },
  data: () => ({
    users: [],
    page: 0,
    userCount: 0,
    user: {},
    updateUserStatus: '',
    flags: [],
    createUserForm: {},
    createUserStatus: "",
    editUserTab: ''
  }),
  methods: {
    createUser(e) {
      e.preventDefault();
      let status = 0;
      for (let i = 0; i < 32; i += 1)
        if (this.createUserForm.flags[i])
          status += (1 << i);
      this.createUserForm.status = status;
      this.$http.post("Admin/CreateUser", this.createUserForm).then(() => {
        this.$refs.createUserModal.hide();
        this.createUserStatus = '';
        this.loadPage(this.page);
      }).catch(() => {
        this.createUserStatus = 'error';
        setInterval(() => this.createUserStatus = '', 20000);
      });
    },
    createUserModalShow() {
      this.createUserForm = {
        name: '',
        surname: '',
        emailAddress: '',
        flags: [],
        password: '',
        sendRandomPassword: false,
        sendVerificationEmail: false
      };
      for (let i = 0; i < 32; i += 1) this.createUserForm.flags.push(0);
      this.$refs.createUserModal.show();
    },
    previewDelete() {
      this.$refs.userInfoModal.hide();
      this.$refs.deleteUserModal.show();
    },
    deleteAccount() {

    },
    updateUser(e) {
      e.preventDefault();
      let status = 0;
      for (let i = 0; i < 32; i += 1)
        if (this.flags[i])
          status += (1 << i);
      this.user.status = status;

      this.$http.put("Admin/UpdateUser", this.user).then(res => {
        this.updateUserStatus = "";
        this.$refs.userInfoModal.hide();
        this.loadPage(this.page);
      }).catch(err => {
        this.updateUserStatus = "error";
        setTimeout(() => this.updateUserStatus = "", 20000);
      })
    },
    showUserInfo(user) {
      this.$http.get("Admin/GetUser?userId=" + encodeURIComponent(user.userId)).then(res => {
        this.$refs.userInfoModal.show();
        this.user = res.data;
        this.user.UserId = user.userId;
        this.user.password = '';
        this.user.resetPassword = false;
        this.flags = [];
        let flag = this.user.status;

        while (flag > 0) {
          this.flags.push(flag % 2);
          flag >>= 1;
        }

        // fill with zeros
        for (let i = 0; i < 32; i += 1) this.flags.push(0);
      })
    },
    loadPage(page) {
      this.page = page;
      this.$http.get("Admin/Users?limit=25&offset=" + encodeURIComponent(page)).then(response => {
        this.userCount = response.data.count;
        this.users = response.data.users;
      })
    }
  },
  mounted() {
    this.loadPage(0);
  }
}
</script>
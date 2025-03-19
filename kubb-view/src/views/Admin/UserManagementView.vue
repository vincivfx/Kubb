<template>
  <h2>User Management</h2>
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
  <Modal :title="'Edit user - ' + user.emailAddress" ref="userInfoModal">
    <InputBlock label="Name" v-model="user.name" />
    <InputBlock label="Surname" v-model="user.surname" />

    <CheckBox>Administrator</CheckBox>
    <CheckBox>Allow join</CheckBox>
    <CheckBox>Allow challenge creation</CheckBox>
    <CheckBox>Must change password after next login</CheckBox>
    <CheckBox>Is user active</CheckBox>

    <div class="text-right btn-group-right">
      <button type="button" class="btn danger">Delete this user</button>
      <input type="submit" value="Save user" class="btn primary">
    </div>
  </Modal>

  <Modal title="Delete user">

  </Modal>
</template>

<script>
import Badge from '@/components/Badge.vue';
import CheckBox from '@/components/CheckBox.vue';
import InputBlock from '@/components/InputBlock.vue';
import Modal from '@/components/Modal.vue';
import Pagination from '@/components/Pagination.vue';

export default {
  components: { Pagination, Badge, Modal, InputBlock, CheckBox },
  data: () => ({
    users: [],
    page: 0,
    userCount: 0,
    user: {}
  }),
  methods: {
    showUserInfo(user) {
      this.$http.get("Admin/GetUser?userId=" + encodeURIComponent(user.userId)).then(res => {
        this.$refs.userInfoModal.show();
        this.user = res.data;
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
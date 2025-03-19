<script>
import { RouterLink, RouterView } from 'vue-router'
import { SlLogout, SlUser } from 'vue-icons-plus/sl'
import Modal from './components/Modal.vue';

export default {
  components: { SlUser, RouterLink, RouterView, SlLogout, Modal },
  data: () => ({
    headerScoreView: true,
    mainColor: '#0000ff'
  }),
  methods: {
    disableHeaderScoreView(v) {
      this.headerScoreView = !v;
    },
    logout: function () {
      this.$refs.logoutModal.hide();
      this.$http.head('/logout').then(() => {
        this.$authSession.removeStored();
        this.$router.push({ name: 'login' });
      }).catch((error) => {
        if (error.response.status === 404) {
          this.$authSession.removeStored();
          this.$router.push({ name: 'login' });
        }
      })
    },
  },
  mounted() {
    this.mainColor = this.$settings.color;
  }
}
</script>

<template>
  <header v-if="headerScoreView || $route.name !== 'challenge-score'">

    <div class="nav-left">
      <div class="header-main-sitename">
        Kubb <span class="small"></span>
      </div>

      <div class="header-nav">
        <RouterLink :to="{ name: 'challenges' }">Challenges</RouterLink>
        <RouterLink :to="{ name: 'admin-user-management' }" v-if="$authSession.getStored().isServerAdministrator">Server
          Management</RouterLink>
        <a href="https://github.com/vincivfx/Kubb" target="_blank">Discover more</a>
      </div>
    </div>

    <div class="nav-rigth">

      <div class="nav-right-icons">
        <RouterLink v-if="!$authSession.getStored()" :to="{ name: 'login' }" class="right-nav-btn">
          <SlUser />
          <span>Login</span>
        </RouterLink>

        <RouterLink v-if="$authSession.getStored()" :to="{ name: 'profile' }" class="right-nav-btn">
          <SlUser />
          <span>{{ $authSession.getName() }}</span>
        </RouterLink>
        <button @click="$refs.logoutModal.show()" v-if="$authSession.getStored()" class="right-nav-btn">
          <SlLogout />
        </button>
      </div>


    </div>

  </header>


  <div
    :class="{ 'container': !$route.meta.view, 'scoreboard': $route.meta.view === 'score', 'container no-padding': $route.meta.view === 'tabs' }">
    <RouterView v-on:disableHeaderScoreView="disableHeaderScoreView" />
  </div>

  <div class="footer">
    Made with &lt;3 from Italy<br>
    &copy; Vincenzo Gallina 2025
  </div>

  <Modal title="Logout" ref="logoutModal">
    <div class="text-center">
      Are you sure to logout from Kubb?

      <div class="btn-group-right m-2">
        <button @click="logout()" class="btn primary">Yes, log me out</button>
        <button @click="$refs.logoutModal.hide()" class="btn">Cancel</button>
      </div>

    </div>
  </Modal>

</template>

<style scss>
.scoreboard {
  text-align: center;
}

:root {
  --mainColor: v-bind(mainColor);
}
</style>

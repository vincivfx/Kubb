<script>
import { RouterLink, RouterView } from 'vue-router'
import { SlUser } from 'vue-icons-plus/sl'

export default {
  components: {SlUser, RouterLink, RouterView},
  data: () => ({
    headerScoreView: true
  }),
  methods: {
    disableHeaderScoreView(v) {
      this.headerScoreView = !v;
    }
  }
}
</script>

<template>
  <header v-if="headerScoreView || $route.name !== 'challenge-score'">

    <div class="nav-left">
      <div class="header-main-sitename">
        KCP <span class="small">Kubb Contest Platform</span>
      </div>

      <div class="header-nav">
        <RouterLink :to="{name: 'discover'}">Discover more</RouterLink>        
        <RouterLink :to="{name: 'challenges'}">Challenges</RouterLink>        
      </div>
    </div>

    <div class="nav-rigth">

      <div class="nav-right-icons">
        <RouterLink v-if="!$authSession.getStored()" :to="{name: 'login'}" class="right-nav-btn">
          <SlUser />
          <span>Login</span>
        </RouterLink>

        <RouterLink v-if="$authSession.getStored()" :to="{name: 'profile'}" class="right-nav-btn">
          <SlUser />
          <span>{{ $authSession.getName() }}</span>
        </RouterLink>
      </div>
      

    </div>

  </header>

  <div :class="{'container': !$route.meta.view, 'scoreboard': $route.meta.view === 'score', 'container no-padding': $route.meta.view === 'tabs'}">
    <RouterView v-on:disableHeaderScoreView="disableHeaderScoreView" />
  </div>

  <div class="footer">
    &copy; Vincenzo Gallina 2025
  </div>

</template>

<style scss>
.scoreboard {
  text-align: center;
}
</style>

<script setup>
import { RouterLink, RouterView } from 'vue-router'
import { SlUser } from 'vue-icons-plus/sl'
</script>

<template>
  <header>

    <div class="nav-left">
      <div class="header-main-sitename">
        KCP <span class="small">Kubb Contest Platform</span>
      </div>

      <div class="header-nav">
        <RouterLink :to="{name: 'challenges'}">Challenges</RouterLink>
        <RouterLink :to="{name: 'challenges', params: {'type': 'archive'}}">Archived Challenges</RouterLink>
        <RouterLink :to="{name: 'challenges', params: {'type': 'joinable'}}">Join Challenge</RouterLink>
        <RouterLink :to="{name: 'challenges', params: {'type': 'my'}}">My Challenges</RouterLink>
        <a href="#">Administration</a>
      </div>
    </div>

    <div class="nav-rigth">

      <div class="nav-right-icons">
        <RouterLink :to="{name: 'login'}" class="right-nav-btn">
          <SlUser />
          <span>Login</span>
        </RouterLink>
      </div>
      

    </div>

  </header>

  <div :class="{'container': !$route.meta.view, 'scoreboard': $route.meta.view === 'score'}">
    <RouterView />
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

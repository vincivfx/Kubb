<script setup>
import { RouterLink, RouterView } from 'vue-router'
import { SlUser } from 'vue-icons-plus/sl'
</script>

<template>
  <header>

    <div class="nav-left">
      <div class="header-main-sitename">
        Kubb
      </div>

      <div class="header-nav">
        <a href="#">Challenges</a>
        <a href="#">Join</a>
        <a href="#">Archive</a>
        <a href="#">Management Area</a>
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

  <div :class="{'container': !$route.meta.view}">
    <RouterView />
  </div>

  <div class="footer">
    &copy; Vincenzo Gallina 2025
  </div>

</template>

<style scoped>
</style>

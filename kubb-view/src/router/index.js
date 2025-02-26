import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/auth',
      name: 'login',
      component: () => import('../views/Auth/LoginView.vue'),
      meta: {
        view: 'empty'
      }
    },
    {
      path: '/auth/register',
      name: 'register',
      component: () => import('../views/Auth/RegisterView.vue')
    },
    {
      path: '/',
      name: 'root',
      redirect: '/challenges'
    },
    {
      path: '/challenges',
      name: 'challenges',
      component: () => import('../views/ChallengesView.vue')
    },
    {
      path: '/score',
      name: 'score',
      component: () => import('../views/ScoreView.vue'),
      meta: {
        view: 'score'
      }
    }
  ],
})

export default router

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
      path: '/profile',
      name: 'profile',
      component: () => import('../views/User/ProfileView.vue')
    },
    {
      path: '/challenges/:type?',
      name: 'challenges',
      component: () => import('../views/ChallengesView.vue')
    },
    {
      path: '/challenge/score',
      name: 'challenge-score',
      component: () => import('../views/ScoreView.vue'),
      meta: {
        view: 'score'
      }
    },
    {
      path: '/challenge/send',
      name: 'challenge-sender',
      component: () => import('../views/Challenge/SenderView.vue')
    },
    {
      path: '/challenge/admin',
      name: 'challenge-admin',
      component: () => import('../views/Challenge/AdminView.vue')
    },
    {
      path: '/error',
      name: 'error',
      component: () => import('../views/ErrorView.vue')
    }
  ],
})

export default router

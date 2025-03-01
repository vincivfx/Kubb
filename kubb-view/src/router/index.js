import DiscoverView from '@/views/DiscoverView.vue'
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/discover',
      name: 'discover',
      component: DiscoverView,
      meta: {
        title: 'Discover'
      }
    },
    {
      path: '/auth',
      name: 'login',
      component: () => import('../views/Auth/LoginView.vue'),
      meta: {
        view: 'empty',
        title: 'Login'
      }
    },
    {
      path: '/auth/register',
      name: 'register',
      component: () => import('../views/Auth/RegisterView.vue')
    },
    {
      path: '/auth/update-password',
      name: 'mandatory-update-password',
      component: () => import('../views/Auth/MandatoryUpdatePasswordView.vue')
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
      component: () => import('../views/ChallengesView.vue'),
      meta: {
        view: 'tabs'
      }
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

router.afterEach((from, to, next) => {
  document.title = from.meta.title + " :: Kubb";
})

export default router

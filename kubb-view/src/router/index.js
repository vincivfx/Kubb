import DiscoverView from '@/views/DiscoverView.vue'
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/discover',
      name: 'discover',
      // component: DiscoverView,
      redirect: '/',
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
      component: () => import('../views/Auth/RegisterView.vue'),
      meta: {
        title: 'Register'
      }
    },
    {
      path: '/auth/verify',
      name: 'verify',
      component: () => import('../views/Auth/VerifyView.vue'),
      meta: {
        title: 'Verify your Account'
      }
    },
    {
      path: '/auth/update-password',
      name: 'mandatory-update-password',
      component: () => import('../views/Auth/MandatoryUpdatePasswordView.vue'),
      meta: {
        title: 'Mandatory password update'
      }
    },    
    {
      path: '/',
      name: 'index',
      redirect: '/challenges'
    },
    {
      path: '/profile',
      name: 'profile',
      component: () => import('../views/User/ProfileView.vue'),
      meta: {
        title: 'Profile'
      }
    },
    {
      path: '/challenges/:page?',
      name: 'challenges',
      component: () => import('../views/ChallengesView.vue'),
      meta: {
        view: 'tabs',
        title: 'Challenges'
      }
    },
    {
      path: '/live',
      name: 'challenge-score',
      component: () => import('../views/ScoreView.vue'),
      meta: {
        view: 'score',
        title: 'Live'
      }
    },
    {
      path: '/challenge/send',
      name: 'challenge-sender',
      component: () => import('../views/Challenge/SenderView.vue'),
      meta: {
        title: 'Send Answers'
      }
    },
    {
      path: '/challenge/admin',
      name: 'challenge-admin',
      component: () => import('../views/Challenge/AdminView.vue'),
      meta: {
        title: 'Challenge Admin'
      }
    },
    {
      path: '/error',
      name: 'error',
      component: () => import('../views/ErrorView.vue'),
      
    },
    {
      path: '/auth/recover',
      name: 'recover',
      component: () => import('../views/Auth/RecoverView.vue'),
      meta: {
        title: 'Recover your Password'
      }
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'not-found',
      component: () => import('../views/NotFoundView.vue')
    }
  ],
})

router.afterEach((from, to, next) => {
  document.title = from.meta.title + " :: Kubb";
})

export default router

import { createRouter, createWebHistory } from 'vue-router'
import authSession from '@/util/session';
import authRoutes from './auth';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [ 
    {
      path: '/',
      name: 'index',
      redirect: '/challenges'
    },
    {
      path: '/auth',
      children: authRoutes,
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
      path: '/challenge/guest-admin',
      name: 'guest-admin',
      component: () => import('../views/Challenge/GuestAdminView.vue'),
      meta: {
        title: 'Guest Admin'
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
      component: () => import('../views/NotFoundView.vue'),
      title: {
        meta: 'Not Found'
      }
    },
    {
      path: '/mgmt',
      name: 'admin-user-management',
      component: () => import('../views/Admin/UserManagementView.vue'),
      title: {
        meta: 'User Management'
      }
    }
  ],
})

router.afterEach((from, to) => {
  document.title = from.meta.title + " :: Kubb";

  if (from.name !== 'mandatory-update-password' && authSession.getStored().mustChangePassword) {
    router.push({name: 'mandatory-update-password'})
  }
})

export default router

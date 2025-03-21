const authRoutes = [
    {
        path: '',
        name: 'login',
        component: () => import('../views/Auth/LoginView.vue'),
        meta: {
            view: 'empty',
            title: 'Login'
        }
    },
    {
        path: 'register',
        name: 'register',
        component: () => import('../views/Auth/RegisterView.vue'),
        meta: {
            title: 'Register'
        }
    },
    {
        path: 'verify',
        name: 'verify',
        component: () => import('../views/Auth/VerifyView.vue'),
        meta: {
            title: 'Verify your Account'
        }
    },
    {
        path: 'update-password',
        name: 'mandatory-update-password',
        component: () => import('../views/Auth/MandatoryUpdatePasswordView.vue'),
        meta: {
            title: 'Mandatory password update'
        }
    },
];

export default authRoutes;
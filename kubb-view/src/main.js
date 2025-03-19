import './assets/Kubb.scss'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import axios from 'axios'
import authSession from './util/session';
import vClickOutside from 'v-click-outside'

const app = createApp(App)

app.config.globalProperties.$turnstileSiteKey = TURNSTILE_CLIENT_TOKEN;

let hostname = '/';

if (window.location.hostname === 'localhost' || window.location.hostname === "127.0.0.1")
	hostname = "http://127.0.0.1:5216/";

const axiosInstance = axios.create({
    baseURL: hostname,
    timeout: 1000,
    headers: {
        'Content-Type': 'application/json',
    },
});
axiosInstance.interceptors.request.use((config) => {
    const authStored = authSession.getStored();
    if (authStored) {
        config.headers['X-AuthId'] = authStored.loginId;
        config.headers['X-AuthToken'] = authStored.token;
    } 
    return config;
})
axiosInstance.interceptors.response.use((response) => {
    return response;
}, (error) => {
    if (error.response && error.response.status === 401 && error.response.data === 'Invalid login') {
        authSession.removeStored();
        router.push({name: 'login'})
    }
    return Promise.reject(error);
})

app.config.globalProperties.$http = axiosInstance;
app.config.globalProperties.$authSession = authSession;

app.use(router)

axiosInstance.get("Home/SystemConfiguration").then(response => {
    app.config.globalProperties.$settings = response.data;
    app.mount('#app')
})





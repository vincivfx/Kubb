import './assets/Kubb.scss'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import axios from 'axios'
import authSession from './util/session';

const app = createApp(App)

app.config.globalProperties.$settings = {
    registration: false
}

const axiosInstance = axios.create({
    baseURL: "http://127.0.0.1:5216/",
    timeout: 1000,
    headers: {},
});
axiosInstance.interceptors.request.use((config) => {
    const authStored = authSession.getStored();
    if (authStored) {
        config.headers['X-AuthId'] = authStored.loginId;
        config.headers['X-AuthToken'] = authStored.token;
    } 
    return config;
})

app.config.globalProperties.$http = axiosInstance;
app.config.globalProperties.$authSession = authSession;

app.use(router)

app.mount('#app')

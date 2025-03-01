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

app.config.globalProperties.$http = axiosInstance;
app.config.globalProperties.$authSession = authSession;

app.use(router)

app.mount('#app')

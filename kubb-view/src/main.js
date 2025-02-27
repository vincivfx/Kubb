import './assets/Kubb.scss'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import axios from 'axios'

const app = createApp(App)

app.config.globalProperties.$settings = {
    registration: false
}

app.config.globalProperties.$http = axios.create({
    baseURL: "http://127.0.0.1:5216/",
    timeout: 1000,
    headers: {},
})


app.use(router)

app.mount('#app')

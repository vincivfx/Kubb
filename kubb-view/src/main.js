import './assets/Kubb.scss'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import axios from 'axios'
import authSession from './util/session';
import vClickOutside from 'v-click-outside'

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

app.directive('click-outside', {
    beforeMount: function (element, binding) {
        console.log({
            element,
            binding
        });

        //  check that click was outside the el and his children
        element.clickOutsideEvent = function (event) {
            // and if it did, call method provided in attribute value
            if (!(element === event.target || element.contains(event.target))) {
                binding.value(event);
            }
        };
        document.body.addEventListener('click', element.clickOutsideEvent)
    },
    unmounted: function (element) {
        document.body.removeEventListener('click', element.clickOutsideEvent)
    }
});

app.mount('#app')

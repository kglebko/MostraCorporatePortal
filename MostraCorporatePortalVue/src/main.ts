/*import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap'*/

import './assets/base.css'
import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import { useAuthStore } from './stores/auth'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.use(router)

// Инициализация auth store после создания pinia
const authStore = useAuthStore()
authStore.init()

app.mount('#app')

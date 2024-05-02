// import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import { Quasar } from 'quasar'
import quasarUserOptions from './quasar-user-options'
import ApiHelper from './helpers/apiHelper'

const app = createApp(App).use(Quasar, quasarUserOptions)
const apiHelper = new ApiHelper()
app.provide('$apiHelper', apiHelper)
app.use(createPinia())
app.use(router)

app.mount('#app')

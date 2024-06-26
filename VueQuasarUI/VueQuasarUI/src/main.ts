import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import { Quasar } from 'quasar'
import quasarUserOptions from './quasar-user-options'
import ApiHelper from './helpers/api/apiHelper'

const app = createApp(App)
const apiHelper = new ApiHelper()
app.provide('$apiHelper', apiHelper)
app.use(createPinia())
app.use(router)
app.use(Quasar, quasarUserOptions)

app.mount('#app')

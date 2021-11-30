import { createApp } from 'vue'
import App from './App.vue'
import router from './Router'
import websocketPlugin from '@/websocket/websocketPlugin'
import store from './store'
import PrimeVue from 'primevue/config';

// import "primevue/resources/themes/saga-blue/theme.css"
import "primevue/resources/themes/bootstrap4-dark-blue/theme.css"
import "primevue/resources/primevue.min.css"
import "primeicons/primeicons.css"
import 'primeflex/primeflex.css';

const app = createApp(App)


app.use(router)
app.use(PrimeVue)
app.use(store)
app.use(websocketPlugin)
app.mount('#app')
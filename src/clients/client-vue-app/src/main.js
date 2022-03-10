import {createApp} from 'vue'
import App from './App.vue'
import routerPlugin from './Router'
import websocketPlugin from '@/websocket/websocketPlugin'
import store from './store'

// PrimeVue
import PrimeVue from 'primevue/config'
import ToastService from 'primevue/toastservice'
import ConfirmationService from 'primevue/confirmationservice'
import BadgeDirective from 'primevue/badgedirective'

// import "primevue/resources/themes/saga-blue/theme.css"
import 'primevue/resources/themes/bootstrap4-dark-blue/theme.css'
import 'primevue/resources/primevue.min.css'
import 'primeicons/primeicons.css'
import 'primeflex/primeflex.css'

const app = createApp(App)


app.use(PrimeVue)
app.use(ToastService)
app.use(ConfirmationService)
app.directive('badge', BadgeDirective)


app.use(store)
app.use(routerPlugin)
app.use(websocketPlugin)
app.mount('#app')

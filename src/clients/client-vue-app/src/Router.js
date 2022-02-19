import {createRouter, createWebHashHistory} from 'vue-router'
import Home from './views/Home'
import Order from './views/Order'

export const routes = [
  {path: '/', component: Home},
  {
    path: '/orders',
    component: Order,
    meta: {
      requireLogin: true
    }
  },
]


const routerPlugin = {
  install(app) {
    const router = createRouter({
      history: createWebHashHistory(),
      routes,
    })

    // get the store
    const $store = app.config.globalProperties.$store

    function waitUntilLogin(next) {
      if ($store.getters.hasTriedLogin) {
        if ($store.getters.isUserLogin) {
          next()
        } else {
          next('/')
          // Show login tooltip
          const loginToast = {
            severity: 'warn',
            summary: 'Login Required',
            detail: 'You need to login to navigate to that page',
            life: 3000
          }
          $store.dispatch('AddNewToast', loginToast)
        }
      } else {
        // wait
        setTimeout(() => {
          console.log('waiting to call waitUntilLogin again')
          waitUntilLogin(next)
        }, 1000)
      }
    }

    router.beforeEach((to, from, next) => {
      if (to.matched.some((record) => record.meta.requireLogin)) {
        waitUntilLogin(next)
      } else {
        next()
      }
    })

    app.use(router)
  }
}

export default routerPlugin

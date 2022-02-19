import {createRouter, createWebHashHistory} from 'vue-router'
import Home from './views/Home'
import Login from '@/views/Login'

export const routes = [
  {path: '/', component: Home},
  {
    path: '/login',
    name: 'login',
    component: Login,
    meta: {
      anonymous: true
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
          next('/login')
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
      if (to.matched.some((record) => record.meta.anonymous)) {
        next()
      } else {
        waitUntilLogin(next)
      }
    })

    app.use(router)
  }
}

export default routerPlugin

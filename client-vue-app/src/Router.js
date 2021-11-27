import {createRouter, createWebHashHistory} from 'vue-router'
import Home from './views/Home'
import Order from './views/Order'

export const routes = [
  { path: '/', component: Home },
  { path: '/orders', component: Order },
]


const router = createRouter({
  history: createWebHashHistory(),
  routes,
})
export default router

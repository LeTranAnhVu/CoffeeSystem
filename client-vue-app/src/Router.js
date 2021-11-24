import {createRouter, createWebHashHistory} from 'vue-router'
import Home from './views/Home'
import Cart from './views/Cart'

export const routes = [
  { path: '/', component: Home },
  { path: '/cart', component: Cart },
]


const router = createRouter({
  history: createWebHashHistory(),
  routes,
})
export default router

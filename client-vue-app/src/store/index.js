import {createStore} from 'vuex'
import cart from '@/store/modules/cart-store'
import product from '@/store/modules/product-store'
import order from '@/store/modules/order-store'
import user from '@/store/modules/user-store'
// import websocketModule from '@/store/modules/ws-store'

const store = createStore({
  modules: {
    cart,
    order,
    product,
    user,
  }
})
// store.registerModule('ws', websocketModule)
export default store

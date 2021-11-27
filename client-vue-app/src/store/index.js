import {createStore} from 'vuex'
import cart from '@/store/modules/cart-store'
import product from '@/store/modules/product-store'
import order from '@/store/modules/order-store'
import user from '@/store/modules/user-store'

const store = createStore({
  modules: {
    cart,
    order,
    product,
    user
  }
})

export default store

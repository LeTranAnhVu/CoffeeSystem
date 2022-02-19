import {createStore} from 'vuex'
import cart from '@/store/modules/cart-store'
import product from '@/store/modules/product-store'
import order from '@/store/modules/order-store'
import user from '@/store/modules/user-store'
import toast from '@/store/modules/toast-store'
import notification from '@/store/modules/notification-store'

const store = createStore({
  modules: {
    cart,
    order,
    product,
    user,
    toast,
    notification
  }
})

export default store

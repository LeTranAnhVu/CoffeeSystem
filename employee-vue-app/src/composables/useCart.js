import {computed, onMounted} from 'vue'
import {useStore} from 'vuex'

export default function useCart(store) {
  store = store ? store : useStore()
  const cartItems = computed(() => store.getters.getCartItems)
  const getCartById = store.getters.getCartItemById
  const updateToCart = (item) => store.commit('UPSERT_TO_CART', item)
  const deleteFromCart = (itemId) => store.commit('DELETE_FROM_CART', itemId)

  const makeOrderFromCart = async () => {
    const cartItemsValue = cartItems.value
    if(!cartItemsValue.length) return

    // Prepare order dto
    // e.g { productIds: [1,2,3] }
    const productIds = cartItemsValue.map(item => item.id)
    // dispatch
    await store.dispatch('createOrder', {productIds})
    // Clear cart after create order
    store.commit('CLEAR_CART')
  }

  return {
    cartItems,
    getCartById,
    updateToCart, deleteFromCart,
    makeOrderFromCart
  }
}

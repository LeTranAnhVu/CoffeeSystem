import {computed, onMounted} from 'vue'
import {useStore} from 'vuex'

export default function useOrder(store) {
  store = store ? store : useStore()
  const orders = computed(() => store.getters.getOrders)
  const fetchOrders = () => store.dispatch('fetchOrders')
  const getOrderById = store.getters.getOrderById

  return {
    orders,
    getOrderById,
    fetchOrders
  }
}

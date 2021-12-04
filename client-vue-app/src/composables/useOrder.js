import {computed, onMounted} from 'vue'
import {useStore} from 'vuex'

export default function useOrder(store) {
  store = store ? store : useStore()
  const orders = computed(() => store.getters.getOrders)
  const orderStatuses = computed(() => store.getters.getOrderStatuses)
  const fetchOrderNeeds = async () => {
    const actions = [
      store.dispatch('fetchOrders'),
      store.dispatch('fetchOrderStatuses')
    ]

    await Promise.all(actions)
  }
  const getOrderById = store.getters.getOrderById
  const cancelOrder = async (orderId) => store.dispatch('cancelOrder', orderId)
  const isLoadingOrders = computed(() => store.getters.getOrderLoadingState)

  return {
    orders,
    orderStatuses,
    getOrderById,
    isLoadingOrders,
    fetchOrderNeeds,
    cancelOrder
  }
}

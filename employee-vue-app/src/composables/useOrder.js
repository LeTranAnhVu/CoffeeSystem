import {computed} from 'vue'
import {useStore} from 'vuex'
import {OrderFilterTypes} from '@/store/modules/order-store'

export default function useOrder(store) {
  store = store ? store : useStore()
  const orders = computed(() => store.getters.getOrders)
  const activeOrders = computed(() => store.getters.getActiveOrders)
  const readyOrders = computed(() => store.getters.getReadyOrders)
  const cancelledOrders = computed(() => store.getters.getCancelledOrders)
  const orderStatuses = computed(() => store.getters.getOrderStatuses)
  const fetchOrderNeeds = async () => {
    const actions = [
      store.dispatch('fetchOrders'),
      store.dispatch('fetchOrderStatuses')
    ]

    await Promise.all(actions)
  }
  const getOrderById = store.getters.getOrderById
  const updateOrderStatus = async (orderId, statusCode) => store.dispatch('updateOrderStatus', {orderId, statusCode})
  const isLoadingOrders = computed(() => store.getters.getOrderLoadingState)

  return {
    orders, activeOrders, readyOrders, cancelledOrders,
    orderStatuses,
    getOrderById,
    isLoadingOrders,
    fetchOrderNeeds,
    updateOrderStatus
  }
}

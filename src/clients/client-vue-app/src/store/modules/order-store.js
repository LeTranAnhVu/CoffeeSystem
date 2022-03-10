import orderApi from '@/api/order'

const orderMappingFn = (order, getters) => {
  order.orderedProducts = order.orderedProducts.map(orderedProduct => {
    const productId = orderedProduct.productId
    orderedProduct.product = getters.getProductById(productId)
    return orderedProduct
  })

  return order
}

const order = {
  state: () => ({
    orders: [],
    isLoading: true,
    statuses: []
  }),
  mutations: {
    REPLACE_ORDERS(state, orders) {
      state.orders = [...orders]
    },

    REPLACE_ORDER_STATUSES(state, statuses) {
      state.statuses = [...statuses]
    },

    UPSERT_ORDER(state, order) {
      if (order.id) {
        const idx = state.orders.findIndex(savedOrder => savedOrder.id === order.id)
        if (idx !== -1) {
          // exists
          state.orders.splice(idx, 1, {...order})
        } else {
          state.orders = [...state.orders, {...order}]
        }
      }
    },

    UPDATE_ORDER_STATUS(state, {id, statusCode, statusName}) {
      // Get order
      const idx = state.orders.findIndex(order => order.id === id)
      if (idx === -1) return

      const updateOrder = state.orders[idx]
      updateOrder.statusCode = statusCode
      updateOrder.statusName = statusName
      state.orders.splice(idx, 1, updateOrder)
    },

    SET_LOADING_STATE(state, isLoading) {
      state.isLoading = isLoading
    }

  },
  actions: {
    async fetchOrders(context) {
      context.commit('SET_LOADING_STATE', true)
      const orders = await orderApi.fetchOrders()
      context.commit('REPLACE_ORDERS', orders)
      context.commit('SET_LOADING_STATE', false)
    },

    async fetchOrderStatuses(context) {
      const statuses = await orderApi.fetchOrderStatuses()
      context.commit('REPLACE_ORDER_STATUSES', statuses)
    },

    async createOrder(context, order) {
      const createdOrder = await orderApi.createOrder(order)
      context.commit('UPSERT_ORDER', createdOrder)
    },

    async cancelOrder(context, orderId) {
      const updatedOrder = await orderApi.cancelOrder(orderId)
      context.commit('UPSERT_ORDER', updatedOrder)
    }
  },
  getters: {
    getOrders: (state, getters) => {
      return state.orders.map(order => orderMappingFn(order, getters))
    },

    getOrderStatuses: (state, getters) => {
      return state.statuses
    },

    getOrderLoadingState: (state) => {
      return state.isLoading
    },

    getOrderById: (state, getters) => (id) => {
      const order = state.orders.find(item => item.id === id)
      return order ? orderMappingFn(order, getters) : null
    },
  }
}

export default order

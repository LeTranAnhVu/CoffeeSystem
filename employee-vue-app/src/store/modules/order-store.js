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
  }),
  mutations: {
    REPLACE_ORDERS(state, orders) {
      state.orders = [...orders]
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
    }

  },
  actions: {
    async fetchOrders(context) {
      const orders = await orderApi.fetchOrders()
      context.commit('REPLACE_ORDERS', orders)
    },

    async createOrder(context, order) {
      const createdOrder = await orderApi.createOrder(order)
      context.commit('UPDATE_USER', createdOrder)
    }
  },
  getters: {
    getOrders: (state, getters) => {
      const result = state.orders.map(order => orderMappingFn(order, getters))
      return result
    },

    getOrderById: (state, getters) => (id) => {
      const order = state.orders.find(item => item.id === id)
      return order ? orderMappingFn(order, getters) : null
    },
  }
}

export default order

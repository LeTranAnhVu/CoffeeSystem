const cart = {
  state: () => ({
    items: [],
  }),
  mutations: {
    UPSERT_TO_CART(state, {id, name, price}) {
      if (id && name && price !== null && price !== undefined) {
        const idx = state.items.findIndex(savedItem => savedItem.id === id)
        if (idx !== -1) {
          // Exits
          state.items.splice(idx, 1, {id, name, price})
        } else {
          state.items = [...state.items, {id, name, price}]
        }
      }
    },
    DELETE_FROM_CART(state, itemId) {
      if (itemId) {
        const idx = state.items.findIndex(savedItem => savedItem.id === itemId)
        if (idx !== -1) {
          // Exits
          state.items.splice(idx, 1)
        }
      }
    },

    CLEAR_CART(state) {
      state.items = []
    }

  },
  actions: {},
  getters: {
    getCartItems: (state) => {
      return state.items
    },

    getCartItemById: (state) => (id) => {
      return state.items.find(item => item.id === id)
    },
  }
}

export default cart

import productApi from '@/api/products'

const product = {
  state: () => ({
    products: [],
  }),
  mutations: {
    REPLACE_PRODUCTS(state, products) {
      state.products = [...products]
    },
  },
  actions: {
    async fetchProducts(context) {
      const products = await productApi.fetchProducts()
      context.commit('REPLACE_PRODUCTS', products)
    },

  },
  getters: {
    getProducts: (state) => {
      return state.products
    },

    getProductById: (state) => (id) => {
      return state.products.find(item => item.id === id)
    },
  }
}

export default product

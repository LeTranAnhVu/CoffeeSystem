import {useStore} from 'vuex'
import {computed} from 'vue'

export default function useProduct(store) {
  store = store ? store : useStore()
  const fetchProducts = () => store.dispatch('fetchProducts')
  const products = computed(() => store.getters.getProducts)
  const getProductById = store.getters.getProductById

  return {
    products,
    fetchProducts,
    getProductById
  }
}

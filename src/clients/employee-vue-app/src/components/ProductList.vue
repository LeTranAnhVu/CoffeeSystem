<template>
  <Card>
    <template #title>
      <h2>Products</h2>
      <Divider />
    </template>
    <template #content>
      <div v-for="item in products" :key="item.id">
        <ProductItem :product="item"/>
        <Divider />
      </div>
    </template>
  </Card>
</template>

<script>
import Card from 'primevue/card'
import Divider from 'primevue/divider'
import ProductItem from './ProductItem'
import {onMounted, ref} from 'vue'
import {getProducts} from '@/api/products'
import useCart from '@/composables/useCart'
import {useStore} from 'vuex'
import useProduct from '@/composables/useProduct'

export default {
  name: 'ProductList',
  components: {
    Card, Divider,
    ProductItem
  },
  setup() {
    const store = useStore()
    const {products, fetchProducts} = useProduct(store)
    onMounted(async () => {
      await fetchProducts()
    })

    return {
      products,
    }
  },
}
</script>

<style lang="scss" scoped>

</style>

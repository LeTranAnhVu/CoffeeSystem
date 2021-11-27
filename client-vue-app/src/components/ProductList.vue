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
import {getAll} from '@/api/products'

export default {
  name: 'ProductList',
  components: {
    Card, Divider,
    ProductItem
  },
  setup() {
    const products = ref([])
    onMounted(async () => {
      products.value = await getAll()
    })

    return {
      products,
    }
  },
}
</script>

<style lang="scss" scoped>

</style>

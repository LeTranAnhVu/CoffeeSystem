<template>
  <Card>
    <template #title>
      Products
    </template>
    <template #content>
      <div v-for="item in products" :key="item.id">
        <ProductItem :product="item"/>
      </div>
    </template>
  </Card>
</template>

<script>
  import Card from 'primevue/card'
  import ProductItem from './ProductItem'
  import {onMounted, ref} from 'vue'
  import {getAll} from '../api/products'

  export default {
    name: 'ProductList',
    components: {
      Card,
      ProductItem
    },
    setup() {
      const products = ref([])
      const isLoading = ref(false)
      onMounted(async () => {
        isLoading.value = true
        products.value = await getAll();
        isLoading.value = false
      });

      return {
        products,
        isLoading
      }
    },
  }
</script>

<style lang="scss" scoped>

</style>

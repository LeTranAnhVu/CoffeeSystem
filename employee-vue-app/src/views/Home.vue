<template>
  <div class="p-d-flex p-mt-6 p-jc-center p-px-2">
    <div class="product-list">
      <ProductList/>
    </div>
    <div class="product-list p-ml-2">
      <CartList/>
    </div>
  </div>
</template>

<script>
import ProductList from '../components/ProductList'
import CartList from '@/components/Cart/CartList'

export default {
  name: 'Home',
  components: {
    CartList,
    ProductList
  },
}
</script>

<style scoped>
.product-list {
  width: 50%;
  min-width: 400px;
}
</style>

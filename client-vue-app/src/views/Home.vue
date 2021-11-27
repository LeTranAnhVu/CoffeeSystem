<template>
  <div class="product-list p-mt-6">
    <ProductList/>
  </div>
</template>

<script>
  import ProductList from '../components/ProductList'

  export default {
    name: 'Home',
    components: {
      ProductList
    },
  }
</script>

<style scoped>
  .product-list{
    width: 50%;
    min-width: 400px;
    margin: auto;
  }
</style>

<template>
  <div class="product-list">
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
    margin: auto;
  }
</style>

<template>
  <div class="product-item">
    <div class="info">
      <p>{{product.name}}</p>
      <p>{{product.price}}</p>
    </div>
    <div class="action">
      <v-btn>
        Order
      </v-btn>
    </div>
  </div>
</template>

<script>
  export default {
    name: 'ProductItem',
    props: {
      product: {
        type: Object,
        default: () => ({
          name: "unknown",
          price: 0
        })
      }
    }
  }
</script>

<style lang="scss" scoped>
 .product-item {
   display: flex;
   flex-direction: row;
   width: 100%;
   justify-content: space-between;
 }
</style>

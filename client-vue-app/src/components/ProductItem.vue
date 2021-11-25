<template>
  <div class="product-item">
    <div class="info">
      <p>{{product.name}}</p>
      <p>{{product.price}}</p>
    </div>
    <div class="action">
      <Button>
        Order
      </Button>
    </div>
  </div>
</template>

<script>
  import Button from 'primevue/button'
  export default {
    name: 'ProductItem',
    components: {Button},
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

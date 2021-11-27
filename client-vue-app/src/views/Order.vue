<template>
  <div class="p-d-flex p-mt-6 p-jc-center p-px-2">
    <OrderProductList/>
  </div>
</template>

<script>

import OrderProductList from '@/components/Order/OrderProductList'

export default {
  name: 'Order',
  components: {
    OrderProductList
  }
}
</script>

<style scoped>

</style>

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
  },
  async beforeRouteEnter(to, from, next) {
    await next(async _vm => {
      const store = _vm.$store
      await store.dispatch('joinGroup')
    })

  },
  async beforeRouteLeave(to, from) {
    const store = this.$store
    await store.dispatch('leaveGroup')
  }
}
</script>

<style scoped>

</style>

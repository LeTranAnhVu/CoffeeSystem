<template>
  <Card class="order-list">
    <template #title>
      <h2>Order</h2>
      <Divider/>
    </template>
    <template #content>
      <template v-if="orders.length">
        <div v-for="item in orders" :key="item.id">
          <OrderItem :item="item"/>
          <Divider/>
        </div>
      </template>
      <template v-else>
        <span>Cart is empty</span>
      </template>
    </template>
  </Card>
</template>

<script>
import Card from 'primevue/card'
import Divider from 'primevue/divider'
import Button from 'primevue/button'
import {useStore} from 'vuex'
import OrderItem from '@/components/Order/OrderItem'
import useOrder from '@/composables/useOrder'
import {onMounted} from 'vue'
import useProduct from '@/composables/useProduct'

export default {
  name: 'OrderProductList',
  components: {
    Card, Divider, Button,
    OrderItem
  },
  props: {
    isOverlay: {
      type: Boolean,
      default: false
    }
  },
  setup() {
    const store = useStore()
    const {orders, fetchOrders} = useOrder(store)
    const {fetchProducts} = useProduct(store)

    onMounted(async () => {
      const fetches = [fetchOrders, fetchProducts]
      await Promise.all(fetches.map(f => f()))
    })
    return {
      orders
    }
  },
}
</script>

<style lang="scss" scoped>
.order-list {
  width: 100%
}
</style>

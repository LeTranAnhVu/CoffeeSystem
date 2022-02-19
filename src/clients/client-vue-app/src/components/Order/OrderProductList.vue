<template>
  <Card class="order-list">
    <template #title>
      <h2>Order</h2>
      <Divider/>
    </template>
    <template #content>
      <template v-if="!isLoadingOrders">
        <template v-if="orders.length">
          <div>
            <div v-for="item in orders" :key="item.id">
              <OrderItem :order="item"/>
              <Divider/>
            </div>

          </div>
        </template>

        <template v-else>
          <span>Cart is empty</span>
        </template>
      </template>
      <template v-else>
        <!--Skeleton-->
        <div class="p-d-flex p-jc-between p-mb-2">
          <Skeleton width="10rem"></Skeleton>
          <Skeleton width="10rem"></Skeleton>
        </div>
        <Skeleton class="p-mb-2"></Skeleton>
        <Skeleton width="10rem" class="p-mb-2"></Skeleton>
      </template>
    </template>
  </Card>
</template>

<script>
import Card from 'primevue/card'
import Divider from 'primevue/divider'
import Button from 'primevue/button'
import Skeleton from 'primevue/skeleton'
import {useStore} from 'vuex'
import OrderItem from '@/components/Order/OrderItem'
import useOrder from '@/composables/useOrder'
import {onMounted} from 'vue'
import useProduct from '@/composables/useProduct'

export default {
  name: 'OrderProductList',
  components: {
    Card, Divider, Button, Skeleton,
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
    const {orders, fetchOrderNeeds, isLoadingOrders} = useOrder(store)
    const {fetchProducts} = useProduct(store)

    onMounted(async () => {
      const fetches = [fetchOrderNeeds, fetchProducts]
      await Promise.all(fetches.map(f => f()))
    })
    return {
      orders,
      isLoadingOrders
    }
  },
}
</script>

<style lang="scss" scoped>
.order-list {
  width: 100%
}
</style>

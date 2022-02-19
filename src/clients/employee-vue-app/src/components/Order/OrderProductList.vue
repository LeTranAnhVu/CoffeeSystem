<template>
  <Card class="order-list">
    <template #title>
      <h2>Order: {{displayOrders.length}}/{{orders.length}}</h2>
      <div class="p-d-flex p-jc-end order-filter">
        <div class="p-field-radiobutton p-mr-5">
          <RadioButton id="1" :value="orderFilterTypes.active" v-model="orderFilterType" />
          <label for="1">Active</label>
        </div>
        <div class="p-field-radiobutton p-mr-5">
          <RadioButton id="2" :value="orderFilterTypes.ready" v-model="orderFilterType" />
          <label for="2">Done</label>
        </div>
        <div class="p-field-radiobutton p-mr-5">
          <RadioButton id="3" :value="orderFilterTypes.cancelled" v-model="orderFilterType" />
          <label for="3">Cancelled</label>
        </div>
        <div class="p-field-radiobutton">
          <RadioButton id="4" value="all" v-model="orderFilterType" />
          <label for="4">All</label>
        </div>
      </div>
      <Divider/>
    </template>
    <template #content>
      <template v-if="!isLoadingOrders">
        <template v-if="displayOrders.length">
          <div>
            <div v-for="item in displayOrders" :key="item.id">
              <OrderItem :order="item"/>
              <Divider/>
            </div>
          </div>
        </template>

        <template v-else>
          <span>There are no orders in `{{orderFilterType}}` category</span>
        </template>
      </template>
      <template v-else>
        <!--Skeleton-->
        <OrderItemSkeleton/>
      </template>
    </template>
  </Card>
</template>

<script>
import Card from 'primevue/card'
import Divider from 'primevue/divider'
import Button from 'primevue/button'
import RadioButton from 'primevue/radiobutton';

import {useStore} from 'vuex'
import OrderItem from '@/components/Order/OrderItem'
import useOrder from '@/composables/useOrder'
import {computed, onMounted, reactive, ref, watch} from 'vue'
import useProduct from '@/composables/useProduct'
import OrderItemSkeleton from '@/components/Order/OrderItemSkeleton'
import {OrderFilterTypes} from '@/store/modules/order-store'


export default {
  name: 'OrderProductList',
  components: {
    Card, Divider, Button, RadioButton,
    OrderItemSkeleton,
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
    const {orders, cancelledOrders, readyOrders, activeOrders, fetchOrderNeeds, isLoadingOrders} = useOrder(store)
    const {fetchProducts} = useProduct(store)

    const orderFilterTypes = reactive(OrderFilterTypes)
    const orderFilterType = ref(orderFilterTypes.active)

    const displayOrders = computed( () => {
      switch (orderFilterType.value) {
        case orderFilterTypes.active: return activeOrders.value
        case orderFilterTypes.ready: return readyOrders.value
        case orderFilterTypes.cancelled: return cancelledOrders.value
        default: return orders.value
      }
    })

    // Hooks
    onMounted(async () => {
      const fetches = [fetchOrderNeeds, fetchProducts]
      await Promise.all(fetches.map(f => f()))
    })

    return {
      displayOrders, orders, orderFilterTypes,
      isLoadingOrders,
      orderFilterType,
    }
  },
}
</script>

<style lang="scss" scoped>
.order-list {
  width: 100%
}
.order-filter {
  .p-field-radiobutton {
    font-size: 0.7em;
  }

}
</style>

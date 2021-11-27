<template>
  <Card class="order-item">
    <template #title>
      <div class="p-d-flex p-jc-between p-ai-center">
        <div class="info">
          <p>#{{ item.id }}</p>
        </div>
        <div class="status" :style="{color:'var(--yellow-300)'}">
          Status: {{item.statusName}}
        </div>
      </div>
      <Divider/>
    </template>
    <template #content>
      <template v-if="item.orderedProducts && item.orderedProducts.length">
        <div v-for="{product} in item.orderedProducts" :key="product?.id">
          <div v-if="product" class="p-d-flex p-jc-between">
            <span>{{product.name}}</span> -
            <span>{{product.price}}</span>
          </div>
          <Divider/>
        </div>
      </template>
    </template>
  </Card>
</template>

<script>
import Button from 'primevue/button'
import {useStore} from 'vuex'
import {computed, ref, toRefs} from 'vue'
import useCart from '@/composables/useCart'
import Divider from 'primevue/divider'
import Card from 'primevue/card'

export default {
  name: 'OrderItem',
  components: {Button, Divider, Card},
  props: {
    item: {
      type: Object,
      require: true,
      default: null,
    }
  },
  setup(props) {
    const { item } = toRefs(props)
    return {
    }
  },
}
</script>

<style lang="scss" scoped>
//.order-item {
//  display: flex;
//  flex-direction: row;
//  width: 100%;
//  justify-content: space-between;
//  align-items: center;
//
//  .product-list {
//    display: flex;
//    justify-content: space-between;
//    align-items: center;
//  }
//}
</style>

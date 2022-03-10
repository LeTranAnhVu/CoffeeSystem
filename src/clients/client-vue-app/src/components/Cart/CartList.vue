<template>
  <Card>
    <template #title v-if="!isOverlay">
      <h2>Cart</h2>
      <Divider/>
    </template>
    <template #content>
      <template v-if="cartItems.length">
        <div v-for="item in cartItems" :key="item.id">
          <CartItem :item="item"/>
          <Divider/>
        </div>
        <div class="p-d-flex p-jc-end">
          <Button class="p-button-warning" @click="handleMakeOrder">Make An Order</Button>
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
import useCart from '@/composables/useCart'
import {useStore} from 'vuex'
import CartItem from '@/components/Cart/CartItem'

export default {
  name: 'CartList',
  components: {
    Card, Divider, Button,
    CartItem
  },
  props: {
    isOverlay: {
      type: Boolean,
      default: false
    }
  },
  setup() {
    const store = useStore()
    const {cartItems, getCartById, updateToCart, makeOrderFromCart} = useCart(store)
    const handleMakeOrder = async () => {
      await makeOrderFromCart()
    }
    return {
      cartItems,
      handleMakeOrder
    }
  },
}
</script>

<style lang="scss" scoped>

</style>

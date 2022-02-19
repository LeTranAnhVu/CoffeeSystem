<template>
  <div class="product-item">
    <div class="info">
      <p>{{ product.name }}</p>
      <p>{{ product.price }}</p>
    </div>
    <div class="action">
      <Button v-if="!isInCart" @click="handleAddToCart">
        Order
      </Button>
      <template v-else>
        <div class="p-d-flex p-ai-center">
          <Button iconPos="right" label="Ordered" icon="pi pi-times" class="p-button-rounded p-button-danger p-button-text" @click="handleDeleteFromCart" />
        </div>
      </template>
    </div>
  </div>
</template>

<script>
import Button from 'primevue/button'
import {useStore} from 'vuex'
import {computed, ref} from 'vue'
import useCart from '@/composables/useCart'

export default {
  name: 'ProductItem',
  components: {Button},
  props: {

    product: {
      type: Object,
      require: true,
      default: () => ({
        name: 'unknown',
        price: 0
      })
    }
  },
  setup(props) {
    const store = useStore()
    const {getCartById, updateToCart, deleteFromCart} = useCart(store)
    const isInCart = computed(() => !!getCartById(props.product.id))

    const handleAddToCart = () => {
      updateToCart(props.product)
    }
    const handleDeleteFromCart = () => {
      deleteFromCart(props.product.id)
    }

    return {
      isInCart,
      handleAddToCart, handleDeleteFromCart
    }
  },
}
</script>

<style lang="scss" scoped>
.product-item {
  display: flex;
  flex-direction: row;
  width: 100%;
  justify-content: space-between;
  align-items: center;
}
</style>

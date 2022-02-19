<template>
  <div class="item-item">
    <div class="info">
      <p>{{ item.name }}</p>
      <p>{{ item.price }}</p>
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
  name: 'CartItem',
  components: {Button},
  props: {
    item: {
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
    const isInCart = computed(() => !!getCartById(props.item.id))

    const handleAddToCart = () => {
      updateToCart(props.item)
    }

    const handleDeleteFromCart = () => {
      deleteFromCart(props.item.id)
    }

    return {
      isInCart,
      handleAddToCart, handleDeleteFromCart
    }
  },
}
</script>

<style lang="scss" scoped>
.item-item {
  display: flex;
  flex-direction: row;
  width: 100%;
  justify-content: space-between;
  align-items: center;
}
</style>

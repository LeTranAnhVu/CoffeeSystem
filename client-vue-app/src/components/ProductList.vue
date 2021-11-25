<template>
  <v-card
      class="mx-auto product-list"
      max-width="700"
      tile
  >
    <v-card-header>
      <v-card-header-text>
        <v-card-title>Products</v-card-title>
      </v-card-header-text>
      <v-progress-circular
          class="spinner"
          v-if="isLoading"
          indeterminate
          size="20"
      ></v-progress-circular>
    </v-card-header>
    <v-divider></v-divider>
    <div class="card-body">
      <v-list two-line>
        <template v-if="products.length">
          <template v-for="(item, index) in products" :key="item.id" >

            <v-list-item >
              <ProductItem :product="item"/>
            </v-list-item>

            <v-divider v-if="index < products.length - 1"></v-divider>
          </template>
        </template>

        <template v-else>
          <v-list-item>
            <p>There is no products</p>
          </v-list-item>
        </template>
      </v-list>
    </div>

    </v-card>
</template>

<script>
  import ProductItem from './ProductItem'
  import {onMounted, ref} from 'vue'
  import {getAll} from '../api/products'

  export default {
    name: 'ProductList',
    components: {
      ProductItem,
    },
    setup() {
      const products = ref([])
      const isLoading = ref(false)
      onMounted(async () => {
        isLoading.value = true
        products.value = await getAll();
        isLoading.value = false
      });

      return {
        products,
        isLoading
      }

    },
  }
</script>

<style lang="scss" scoped>
</style>

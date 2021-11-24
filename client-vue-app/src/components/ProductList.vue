<template>
  <v-card
      class="mx-auto"
      max-width="700"
      tile
  >
    <v-card-header>
      <v-card-header-text>
        <v-card-title>Products</v-card-title>
      </v-card-header-text>
    </v-card-header>

    <v-divider></v-divider>

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
      onMounted(async () => {
        products.value = await getAll();
      });

      return {
        products
      }

    },
  }
</script>

<style scoped>

</style>

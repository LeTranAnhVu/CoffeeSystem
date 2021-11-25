<template>
  <v-footer
      app
      padless
  >
    <v-col
        class="text-center"
        cols="12"
    >
      {{ new Date().getFullYear() }} — <strong>Brian's coffee system</strong>
    </v-col>
  </v-footer>
</template>

<script>
  export default {
    name: 'Footer'
  }
</script>

<style scoped>

</style>

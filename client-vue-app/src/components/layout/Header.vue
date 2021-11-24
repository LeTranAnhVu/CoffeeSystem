<template>
  <v-app-bar app>
    <v-app-bar-title>Brian's Coffee</v-app-bar-title>
    <v-spacer></v-spacer>
    <v-btn text>
      <router-link to="/">Home</router-link>
    </v-btn>
    <v-btn class="mr-3" text>
      <router-link to="/cart">Cart</router-link>
    </v-btn>
    <v-divider inset vertical></v-divider>

    <!--Login part-->
    <v-btn
        class="mr-4 ml-5"
        color="primary"
        plain
    >
      <v-icon left icon="mdi-handshake-outline"></v-icon>

      <span>Login</span>
    </v-btn>
  </v-app-bar>
</template>

<script>
  export default {
    name: 'Header'
  }
</script>

<style scoped>

</style>

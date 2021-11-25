<template>
  <v-app-bar app>
    <v-app-bar-title>Brian's Coffee</v-app-bar-title>
    <v-spacer></v-spacer>
    <v-btn class="mr-3" to="/">
      Home
    </v-btn>
    <v-btn class="mr-3" to="/cart">
      <v-icon color="grey lighten-1">
        mdi-cart
      </v-icon>
    </v-btn>
    <v-divider inset vertical></v-divider>

    <!--Login part-->
        <LoginDialog class="mr-2 ml-5" v-bind="attrs" v-on="on"/>


    <!--Register part-->
    <RegisterDialog/>


  </v-app-bar>
</template>

<script>
  import RegisterDialog from '../RegisterDialog'
  import LoginDialog from '../LoginDialog'
  export default {
    name: 'Header',
    components: {RegisterDialog, LoginDialog},

  }
</script>

<style lang="scss">
.auth-dialog{
  margin: auto;
  .v-overlay__content {
    width: 900px;
    transform: translateY(-35%);

  }

}
</style>

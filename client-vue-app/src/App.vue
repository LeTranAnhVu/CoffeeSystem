<template>
  <v-app>
    <Header/>
    <v-main>
      <div class="mt-16">
        <router-view></router-view>
      </div>
    </v-main>
  <Footer/>
  </v-app>
</template>

<script>
import Header from './components/layout/Header'
import Footer from './components/layout/Footer'
export default {
  name: 'App',

  components: {
    Header,
    Footer
  },

  mounted() {
    const env =  process.env.VUE_APP_TITLE_INFO
    document.title = `${env} - Brian's coffee`;
  },
}
</script>

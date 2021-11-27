<template>
  <div>
    <Header/>
    <router-view></router-view>
    <Footer/>

  </div>
</template>

<script>
import Header from './components/layout/Header/Header'
import Footer from './components/layout/Footer'
import {onMounted} from 'vue'
import useLogin from '@/composables/useLogin'

export default {
  name: 'App',

  components: {
    Header,
    Footer
  },

  setup() {
    const {checkUserLogin} = useLogin()
    onMounted(async () => {
      const env = process.env.VUE_APP_TITLE_INFO
      document.title = `${env} - Brian's coffee`
      await checkUserLogin()
    })
  },
}
</script>
<style lang="scss">
body {
  margin: 0;
  background-color: var(--surface-b);
  font-family: var(--font-family);
  color: var(--text-color);

  .p-divider-solid.p-divider-vertical::before {
    border-left-style: solid;
  }

  .p-divider-solid.p-divider-horizontal:before {
    border-top-style: solid;
  }
}

.p-dialog-mask {
  background: rgb(111 116 121 / 61%);
}
</style>

<template>
  <div>
    <Toast position="bottom-right" />
    <ConfirmDialog></ConfirmDialog>
    <Message v-if="wsTestMessage" severity="success">{{ wsTestMessage }}</Message>
    <Header/>
    <template v-if="triedLogin">
      <router-view></router-view>
    </template>
    <template v-else>
      <h5>Loading ...</h5>
      <ProgressBar mode="indeterminate" style="height: .5em" />
    </template>
    <Footer/>
  </div>
</template>

<script>
import Message from 'primevue/message'
import ProgressBar from 'primevue/progressbar'
import Toast from 'primevue/toast'
import Button from 'primevue/button'
import ConfirmDialog from 'primevue/confirmdialog'

import Header from './components/layout/Header/Header'
import Footer from './components/layout/Footer'
import {computed, onMounted} from 'vue'
import useLogin from '@/composables/useLogin'
import {useStore} from 'vuex'
import useAppToast from '@/composables/useAppToast'

export default {
  name: 'App',

  components: {
    Header,
    Footer,
    Message, ProgressBar, Toast, Button, ConfirmDialog
  },

  setup() {
    const {checkUserLogin, triedLogin} = useLogin()
    const store = useStore()
    onMounted(async () => {
      const env = process.env.VUE_APP_TITLE_INFO
      document.title = `${env} - Brian's coffee Client App`
      await checkUserLogin()
      await store.dispatch('listenToTestMessage')
    })

    const wsTestMessage = computed(() => store.getters.getTestMessage)

    // toast
    useAppToast();

    return {
      wsTestMessage,
      triedLogin,
    }
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

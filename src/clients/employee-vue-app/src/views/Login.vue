<template>
  <div class="p-d-flex p-mt-6 p-jc-center p-px-2">
    <LoginForm @login-done="closeLoginDone"/>
  </div>
</template>

<script>
import LoginForm from '@/components/form/LoginForm'
import {useRouter} from 'vue-router'


export default {
  name: 'Login',
  components: {
    LoginForm
  },
  setup() {
    const router = useRouter()
    const closeLoginDone = () => {
      router.push("/")
    }

    return {
      closeLoginDone
    }
  }
}
</script>

<style scoped>
</style>

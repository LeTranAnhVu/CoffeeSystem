<template>
  <div>
    <div class="p-fluid">
      <div class="p-field">
        <label for="email" autofocus>Email</label>
        <InputText id="email" v-model="email" type="email"/>
      </div>
      <div class="p-field">
        <label for="password">Password</label>
        <InputText id="password" v-model="password" type="password"/>
      </div>
    </div>
    <div class="p-d-flex p-jc-end p-mt-5">
      <Button class="p-button-warning" label="Login" icon="pi pi-check" @click="handleLogin"/>
    </div>
  </div>

</template>

<script>
import {ref} from 'vue'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
import useLogin from '@/composables/useLogin'

export default {
  name: 'LoginForm',
  emits: ['login-done'],
  components: {
    InputText, Button
  },
  setup(props, {emit}) {
    const email = ref('')
    const password = ref('')

    const {login} = useLogin()
    const handleLogin = async () => {
      await login(email.value, password.value)
      emit('login-done')
    }
    return {
      email, password, handleLogin
    }
  }
}
</script>

<style scoped>

</style>

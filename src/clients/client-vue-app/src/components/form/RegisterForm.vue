<template>
  <div>
    <div class="p-fluid">
      <div class="p-field">
        <label for="username">Username</label>
        <InputText autocomplete="off" id="username" v-model="username" type="text"/>
      </div>
      <div class="p-field">
        <label for="email">Email</label>
        <InputText autocomplete="off" id="email" v-model="email" type="email"/>
      </div>
      <div class="p-field">
        <label for="password">Password</label>
        <InputText autocomplete="off" v-model="password" id="password" type="password"/>
      </div>
    </div>
    <div class="p-d-flex p-jc-end p-mt-5">
      <Button class="p-button-warning" label="Create new user" icon="pi pi-plus" @click="handleRegister" />
    </div>
  </div>
</template>

<script>
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'

import {ref} from 'vue'
import useRegister from '@/composables/useRegister'

export default {
  name: 'RegisterForm',
  emits: ['register-done'],
  components: {
    InputText, Button
  },

  setup(props, {emit}) {
    const email = ref('')
    const username = ref('')
    const password = ref('')
    const {register} = useRegister()

    const handleRegister = async () => {
      await register(username.value, email.value, password.value)
      emit('register-done')
    }
    return {
      email, password, username, handleRegister
    }
  }
}
</script>

<style scoped>
</style>

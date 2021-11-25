<template>
  <div class="p-fluid">
    <div class="p-field">
      <label for="email">Email</label>
      <InputText id="email" type="email" />
    </div>
    <div class="p-field">
      <label for="password">Password</label>
      <InputText id="password" type="password" />
    </div>
  </div>
</template>

<script>
  import InputText from 'primevue/inputtext';
  export default {
    name: 'LoginForm',
    components: {
      InputText
    }
  }
</script>

<style scoped>

</style>

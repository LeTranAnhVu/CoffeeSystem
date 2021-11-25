<template>
  <div>
    <Menubar :model="items">
      <template #start>
        <p>Brian's coffee</p>
      </template>
      <template #end>
        <!--<Button icon="pi pi-shopping-cart" class="btn-cart p-button-lg p-button-rounded p-button-text p-mr-3" badge="2" to="/cart" />-->

        <Button class="p-mr-3 p-button-secondary" label="Login" icon="pi pi-fw pi-user" @click="openLogin" />
        <Dialog header="Login" v-model:visible="isShowLogin" :style="{width: '50vw'}">
          <LoginForm/>
          <template #footer>
            <Button label="Login" icon="pi pi-check" @click="handleLogin" autofocus />
          </template>
        </Dialog>

        <Button label="Register" icon="pi pi-fw pi-user-plus" @click="openRegister" />
        <Dialog header="Register" v-model:visible="isShowRegister" :style="{width: '50vw'}">
          <RegisterForm/>
          <template #footer>
            <Button label="Register" icon="pi pi-check" @click="handleRegister" autofocus />
          </template>
        </Dialog>
      </template>


    </Menubar>
  </div>
</template>

<script>
  import {ref} from 'vue'
  import Menubar from 'primevue/menubar';
  import Dialog from 'primevue/dialog';
  import Button from 'primevue/button';
  import LoginForm from '../LoginForm'
  import RegisterForm from '../RegisterForm'

  export default {
    name: 'Header',
    components: {
      Menubar, Dialog, Button,
      LoginForm,
      RegisterForm,
    },

    setup() {
      const isShowLogin = ref(false);
      const isShowRegister = ref(false);
      const openLogin = () => {
        handleRegister()
        isShowLogin.value = true;
      };

      const openRegister = () => {
        handleLogin()
        isShowRegister.value = true;
      };

      const handleLogin = () => {
        isShowLogin.value = false;
      };

      const handleRegister = () => {
        isShowRegister.value = false;
      };

      const items = ref([
        {
          label:'Home',
          to: '/'
        },
        {
          label:'',
          icon:'pi pi-fw pi-shopping-cart',
          to: '/cart'
        },
      ]);

      return {
        items,
        isShowLogin, openLogin, handleLogin,
        isShowRegister, openRegister, handleRegister}
    }
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

<template>
  <div class="app-header">
    <Menubar :model="menus">
      <template #start>
        <p>Brian's coffee</p>
      </template>
      <template #end >
        <div class="p-d-flex">
          <Divider layout="vertical" />
          <template v-if="isLogin">
            <Button class="p-button-text p-button-info" type="button" :label="userInfo.username" @click="openUserMenu"
                    aria-haspopup="true"
                    aria-controls="overlay_menu"/>
            <Menu id="overlay_menu" ref="userMenu" :model="userMenuItems" :popup="true"/>
          </template>
          <template v-else>
            <Button class="p-mr-3 p-button-primary p-button-text" label="Login" icon="pi pi-fw pi-user"
                    @click="openLoginDialog"/>
            <Dialog header="Login" v-model:visible="isShowLogin" :style="{minWidth: '400px', width: '30vw'}">
              <LoginForm @login-done="closeLoginDialog"/>
            </Dialog>

            <Button class="p-button-warning" label="Register" icon="pi pi-fw pi-user-plus" @click="openRegisterDialog"/>
            <Dialog header="Register" v-model:visible="isShowRegister" :style="{width: '50vw'}">
              <RegisterForm @register-done="closeLoginDialog"/>
            </Dialog>
          </template>
        </div>
      </template>
    </Menubar>
  </div>
</template>

<script>
import {computed, ref, watch} from 'vue'
import Menubar from 'primevue/menubar'
import Dialog from 'primevue/dialog'
import Button from 'primevue/button'
import Menu from 'primevue/menu'
import Divider from 'primevue/divider'
import LoginForm from '@/components/form/LoginForm'
import RegisterForm from '@/components/form/RegisterForm'
import {useStore} from 'vuex'
import useLogout from '@/composables/useLogout'

export default {
  name: 'Header',
  components: {
    Menubar, Dialog, Button, Menu, Divider,
    LoginForm,
    RegisterForm,
  },

  setup() {
    const store = useStore()
    // Login
    const isLogin = computed(() => store.getters.isUserLogin)
    const userInfo = computed(() => store.getters.getUserInfo)
    const needToShowLoginForm = computed(() => store.getters.needToShowLoginForm)
    watch(needToShowLoginForm, (newValue, oldValue) => {
      if (newValue !== oldValue && newValue) {
        openLoginDialog()
      }
    })

    const userMenu = ref()
    const userMenuItems = ref([
      {
        label: 'user',
        icon: 'pi-user-edit',
        items: [
          {
            label: 'Settings',
            icon: 'pi pi-cog',
            command: () => {

            }
          },
          {
            separator: true
          },
          {
            label: 'Logout',
            icon: 'pi pi-sign-out',
            command: () => {
              handleLogout()
            }
          }
        ]
      },
    ])
    const openUserMenu = (event) => {
      userMenu.value.toggle(event)
    }
    // Logout
    const {logout} = useLogout()
    const handleLogout = () => {
      logout()
    }

    const isShowLogin = ref(false)
    const isShowRegister = ref(false)
    const openLoginDialog = () => {
      closeRegisterDialog()
      isShowLogin.value = true
    }
    const openRegisterDialog = () => {
      closeLoginDialog()
      isShowRegister.value = true
    }

    const closeLoginDialog = () => {
      store.commit('NEED_SHOW_LOGIN_FORM', false)
      isShowLogin.value = false
    }
    const closeRegisterDialog = () => {
      isShowRegister.value = false
    }

    // menu
    const menus = ref([
      {
        label: 'Home',
        icon: 'pi pi-home',
        to: '/'
      },
      {
        label: '',
        icon: 'pi pi-fw pi-shopping-cart',
        to: '/cart'
      },
    ])


    return {
      menus,
      isShowLogin, openLoginDialog, closeLoginDialog,
      isShowRegister, openRegisterDialog, closeRegisterDialog,
      isLogin, userInfo,
      handleLogout,
      userMenuItems, userMenu, openUserMenu
    }
  }
}
</script>

<style lang="scss">
.app-header {
  .p-menubar {
    .p-menubar-end {
      margin-left: unset;
    }

    .p-menubar-root-list {
      margin-left: auto;
    }
  }
}
</style>

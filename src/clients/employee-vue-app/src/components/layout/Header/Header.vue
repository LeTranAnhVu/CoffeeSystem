<template>
  <div class="app-header">
    <Menubar :model="menus">
      <template #start>
        <p>Brian's coffee Employee</p>
      </template>
      <template #end>
        <div class="p-d-flex p-ai-center">
          <Divider layout="vertical"/>

          <template v-if="isLogin">
            <NotificationBadge></NotificationBadge>
            <Button class="p-button-text p-button-info" type="button" :label="userInfo.username" @click="openUserMenu"
                    aria-haspopup="true"
                    aria-controls="user_menu"/>
            <Menu id="user_menu" ref="userMenu" :model="userMenuItems" :popup="true"/>
          </template>
          <template v-else>
            <Button class="p-mr-3 p-button-primary p-button-text" label="Login" icon="pi pi-fw pi-user" to="/login"/>
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
import OverlayPanel from 'primevue/overlaypanel'

import {useStore} from 'vuex'
import useLogout from '@/composables/useLogout'
import NotificationBadge from '@/components/Notification/NotificationBadge'

export default {
  name: 'Header',
  components: {
    Menubar, Dialog, Button, Menu, Divider, OverlayPanel,
    NotificationBadge
  },

  setup() {
    const store = useStore()
    // Login
    const isLogin = computed(() => store.getters.isUserLogin)
    const userInfo = computed(() => store.getters.getUserInfo)

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

    // menu
    const menus = ref([
      {
        label: 'Home',
        icon: 'pi pi-home',
        to: '/'
      },
    ])


    return {
      menus,
      isLogin, userInfo,
      handleLogout,
      userMenuItems, userMenu, openUserMenu,
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

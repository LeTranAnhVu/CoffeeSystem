<template>
  <div>
    <Button class="p-button-icon p-button-text" type="button" @click="toggle">
      <i class="pi pi-bell p-text-secondary" style="font-size: 1.5rem"
         v-badge="notifications.length ? notifications.length : 0 "></i>
    </Button>
    <OverlayPanel ref="op" :showCloseIcon="true" style="width: 650px" :breakpoints="{'960px': '75vw'}">
      <NotificationList/>
    </OverlayPanel>
  </div>
</template>

<script>
import Button from 'primevue/button'
import OverlayPanel from 'primevue/overlaypanel'
import NotificationList from '@/components/Notification/NotificationList'
import {ref} from 'vue'
import {useStore} from 'vuex'
import useNotification from '@/composables/useNotification'

export default {
  name: 'NotificationBadge',
  components: {
    Button, OverlayPanel,
    NotificationList
  },
  setup() {
    const store = useStore()
    const {notifications} = useNotification(store)
    // OP
    const op = ref()
    const toggle = (event) => {
      op.value.toggle(event)
    }

    return {
      toggle,
      op, notifications
    }
  }
}
</script>

<style scoped>

</style>

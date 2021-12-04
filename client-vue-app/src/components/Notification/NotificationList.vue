<template>
  <Card>
    <template #title>
      Notification
    </template>
    <template #content>
      <template v-if="notifications.length">
        <div class="notification-item" v-for="noti in notifications" :key="noti.id">
          <div class="content">
            <span>{{noti.message}}</span>
            <span class="time">{{formatFromNow(noti.sentAt)}}</span>
            </div>
          <Divider/>
        </div>
      </template>
      <template v-else>
        <span>No notifications!</span>
      </template>
    </template>
  </Card>
</template>

<script>
import Card from 'primevue/card'
import Divider from 'primevue/divider'
import Button from 'primevue/button'
import useNotification from '@/composables/useNotification'
import {useStore} from 'vuex'
import moment from 'moment'
export default {
  name: 'NotificationList',
  components: {
    Card, Divider, Button,
  },

  setup() {
    const store = useStore()
    const {notifications} = useNotification(store)

    const formatFromNow = (dateTime) => {
      return moment(dateTime).fromNow()
    }
    return {
      notifications,
      formatFromNow
    }
  }
}
</script>

<style lang="scss" scoped>
.notification-item .content{
  display: flex;
  align-items: center;
  justify-content: space-between;
}
</style>

import {useStore} from 'vuex'
import {computed} from 'vue'

export default function useNotification(store) {
  store = store ? store : useStore()
  const addNotification = (noti) => {store.commit('UPSERT_NOTIFICATION', noti)}
  const deleteNotification = (noti) => {store.commit('UPSERT_NOTIFICATION', noti)}
  const notifications = computed(() => store.getters.getNotifications)

  return {
    notifications,
    addNotification, deleteNotification
  }
}

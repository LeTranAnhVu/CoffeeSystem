import {v4 as uuid} from 'uuid'

const notification = {
  state: () => ({
    notifications: [],
    queueSize: 10
  }),
  mutations: {
    UPSERT_NOTIFICATION(state, notification) {
      // notification example
      // {id: uuid, message: '', link?: '', sentAt: date}
      const {id, message, link, sentAt} = notification
      if (message) {
        if (id) {
          const idx = state.notifications.findIndex(savedItem => savedItem.id === id)
          if (idx !== -1) {
            // Exits
            state.notifications.splice(idx, 1, notification)
          }
        } else {
          notification.id = uuid()
          notification.sentAt = sentAt || Date.now()
          // Newest information is on top of the list.
          state.notifications = [notification, ...state.notifications]
        }
      }

      // Check the queue size
      // If it exceeds, then pop the oldest one.
      while (notification.length > state.queueSize) {
        notification.pop()
      }
    },

    DELETE_NOTIFICATION(state, id) {
      if (id) {
        const idx = state.notifications.findIndex(savedItem => savedItem.id === id)
        if (idx !== -1) {
          // Exits
          state.notifications.splice(idx, 1)
        }
      }
    },
    CLEAR_NOTIFICATIONS(state) {
      state.notifications = []
    }
  },
  getters: {
    getNotifications: (state) => {
      return state.notifications
    },
  }
}

export default notification

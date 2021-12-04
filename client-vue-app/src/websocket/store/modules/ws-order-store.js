import methodContracts from '@/websocket/methodContracts'
import {joinGroup, listenTo} from '@/websocket/helpers/groupHelper'

const wsOrder = {
  actions: {
    async joinOrderGroup(context) {
      // Get current user email
      if(!context.getters.isUserLogin) {
        console.warn("User is not login, cannot process join signalR group")
        return
      }

      const {email} = context.getters.getUserInfo;
      const groupName = 'orders.' + email;
      await joinGroup(groupName);

      await listenTo(methodContracts.ChangeOrderStatus, (changedOrderDto) => {
        const {orderId, statusCode, statusName } = changedOrderDto
        context.commit('UPDATE_ORDER_STATUS', {id: orderId, statusCode, statusName })

        // Ready
        if(statusCode === 3){
          console.log('no day')
          const notification = {message: `The Order #${orderId} is ready to pickup`}
          context.commit('UPSERT_NOTIFICATION', notification)
        }
      })
    },

    async leaveOrderGroup(context) {
      // Get current user email
      if(!context.getters.isUserLogin) {
        console.warn("User is not login, cannot process join signalR group")
        return
      }
      const {email} = context.getters.getUserInfo;
      const groupName = 'orders.' + email;
      await joinGroup(groupName);

    }
  },
}

export default wsOrder

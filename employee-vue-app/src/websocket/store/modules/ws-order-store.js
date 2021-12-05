import methodContracts from '@/websocket/methodContracts'
import {joinGroup, listenTo} from '@/websocket/helpers/groupHelper'
import {OrderCodes} from '@/constants'

const wsOrder = {
  actions: {
    async joinOrderGroup(context) {
      // Get current user email
      if (!context.getters.isUserLogin) {
        console.warn('User is not login, cannot process join signalR group')
        return
      }

      // const {email} = context.getters.getUserInfo;
      const groupName = 'orders.create'
      await joinGroup(groupName)

      await listenTo(methodContracts.CreateNewOrder, async (changedOrderDto) => {
        const {orderId} = changedOrderDto
        await context.dispatch('fetchOrderById', orderId)
        const notification = {message: `New Order! The Order #${orderId} is just created.`}
        context.commit('UPSERT_NOTIFICATION', notification)
      })
    },

    async leaveOrderGroup(context) {
      // Get current user email
      if (!context.getters.isUserLogin) {
        console.warn('User is not login, cannot process join signalR group')
        return
      }
      // const {email} = context.getters.getUserInfo;
      const groupName = 'orders.create'
      await joinGroup(groupName)

    }
  },
}

export default wsOrder

import methodContracts from '@/websocket/methodContracts'
import {joinGroup, leaveGroup, listenTo} from '@/websocket/helpers/groupHelper'
import {OrderCodes} from '@/constants'

const wsOrder = {
  actions: {
    async joinOrderGroupAsEmployee(context) {
      // Get current user email
      if (!context.getters.isUserLogin) {
        console.warn('User is not login, cannot process join signalR group')
        return
      }

      // const {email} = context.getters.getUserInfo;
      const orderGroup = 'orders.paid'
      await joinGroup(orderGroup)

      await listenTo(methodContracts.CreateNewOrder, async (changedOrderDto) => {
        const {orderId} = changedOrderDto
        await context.dispatch('fetchOrderById', orderId)
        const notification = {message: `New Order! The Order #${orderId} is just created.`}
        context.commit('UPSERT_NOTIFICATION', notification)
      })

      const cancelGroup = 'orders.cancel'
      await joinGroup(cancelGroup)

      await listenTo(methodContracts.ChangeOrderStatus, (changedOrderDto) => {
        const {orderId, statusCode, statusName } = changedOrderDto
        context.commit('UPDATE_ORDER_STATUS', {id: orderId, statusCode, statusName })


        if(statusCode === OrderCodes.Cancelled){
          const notification = {message: `The Order #${orderId} is cancelled`}
          context.commit('UPSERT_NOTIFICATION', notification)
        }
      })
    },

    async leaveOrderGroupAsEmployee(context) {
      // Get current user email
      if (!context.getters.isUserLogin) {
        console.warn('User is not login, cannot process leave signalR group')
        return
      }
      // const {email} = context.getters.getUserInfo;
      const orderGroup = 'orders.paid'
      await leaveGroup(orderGroup)

      const cancelGroup = 'orders.cancel'
      await leaveGroup(cancelGroup)

    }
  },
}

export default wsOrder

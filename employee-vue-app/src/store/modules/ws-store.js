import signalRSingleton from '@/websocket/signalRSingleton'
import methodContracts from '@/websocket/methodContracts'

const websocketModule = {
  state: () => ({
    message: null

  }),
  mutations: {
    SET_WEB_SOCKET_TEST_MESSAGE: (state, message) => {
      state.message = message
    }
  },
  actions: {
    async listenToTestMessage(context) {
      const signalR = await signalRSingleton.getConnection()
      signalR.on(methodContracts.TestMessage, (message) => {
        context.commit('SET_WEB_SOCKET_TEST_MESSAGE', message)
      })
    },
    async joinGroup(context) {
      const signalR = await signalRSingleton.getConnection()
      const groupName = 'order-group'
      await signalR.invoke(methodContracts.JoinGroup, groupName)
      console.log(`Start listen on ${groupName} status in websocket`)
      // ... on some channel
      // signalR.on(methodContracts.TestMessage, (message) => {
      //   context.commit('SET_WEB_SOCKET_TEST_MESSAGE', message)
      // })
    },
    async leaveGroup(context) {
      const signalR = await signalRSingleton.getConnection()
      const groupName = 'order-group'
      console.log(`Leave ${groupName} status in web socket`)
      await signalR.invoke(methodContracts.LeaveGroup, groupName)
    }
  },
  getters: {
    getTestMessage(state) {
      return state.message
    }
  }
}

export default websocketModule

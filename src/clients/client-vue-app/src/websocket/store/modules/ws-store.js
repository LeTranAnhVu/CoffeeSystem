import signalRSingleton from '@/websocket/signalRSingleton'
import methodContracts from '@/websocket/methodContracts'

const wsModule = {
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
  },

  getters: {
    getTestMessage(state) {
      return state.message
    }
  }
}

export default wsModule

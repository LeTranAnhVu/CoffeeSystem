import signalRSingleton from '@/websocket/signalRSingleton'

const websocketModule = {
  state: () => ({
    temperature: null

  }),
  mutations: {
    WEB_SOCKET_RECEIVE_MESSAGE: (state, {who, report}) => {
      state.temperature = `${who} said that: ${report}`
    }
  },
  actions: {
    async joinGroup(context){
      const signalR = await signalRSingleton.getConnection()
      await signalR.invoke('JoinQuestionGroup', 'private-group')
      signalR.on('ReceiveMessage', (who, report) => {
        console.log('Start listen on private-group status in web socket')
        context.commit('WEB_SOCKET_RECEIVE_MESSAGE', {who, report})
      })
    },
    async leaveGroup(context){
      const signalR = await signalRSingleton.getConnection()
      console.log('Leave private-group status in web socket')
      await signalR.invoke('LeaveQuestionGroup', 'private-group')
    }
  },
  getters: {
    getTemp(state) {
      return state.temperature
    }
  }
}

export default websocketModule

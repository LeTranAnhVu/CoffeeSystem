import websocketModule from '@/store/modules/ws-store'
import signalRSingleton from '@/websocket/signalRSingleton'

export default {
  install(app) {
    // Establish web socket connection
    const baseUrl = process.env.VUE_APP_BASE_URL
    const connectUrl = 'http://localhost:7200/realtime/commonHub'
    signalRSingleton.init(connectUrl)
    // Assign websocket store module
    app.config.globalProperties.$store.registerModule('ws', websocketModule)
  }
}

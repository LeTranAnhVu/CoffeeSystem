import signalRSingleton from '@/websocket/signalRSingleton'
import wsModule from '@/websocket/store/modules/ws-store'
import wsOrder from '@/websocket/store/modules/ws-order-store'

export default {
  install(app) {
    // Establish web socket connection
    const baseUrl = process.env.VUE_APP_BASE_URL
    // const connectUrl = 'http://localhost:7200/realtime/commonHub'
    const connectUrl = `${baseUrl}/signalr/commonHub`

    signalRSingleton.init(connectUrl)
    // Assign websocket store module
    app.config.globalProperties.$store.registerModule('ws', wsModule)
    app.config.globalProperties.$store.registerModule('wsStore', wsOrder)
  }
}

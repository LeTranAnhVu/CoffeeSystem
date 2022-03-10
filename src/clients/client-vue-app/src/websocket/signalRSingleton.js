const {HubConnectionBuilder, LogLevel} = require('@microsoft/signalr')

class SignalRSingleton {
  constructor() {
    this.instance = null
  }

  getInstance() {
    return this.instance
  }

  async getConnection() {
    const signalR = this.getInstance()
    if (signalR) {
      if (signalR.state === 'Connected') {
        return signalR
      } else {
        // wait to 1 sec to re check
        const promise = new Promise((resolve) => {
          setTimeout(async () => {
            const signalRConnection = await this.getConnection()
            resolve(signalRConnection)
          }, 1000)
        })

        return await promise
      }
    } else {
      console.error('cannot get signalR instance')
    }
    return null
  }

  init(connectUrl) {
    console.log('start createSignalrConnection')
    const connection = new HubConnectionBuilder()
        .withUrl(connectUrl)
        .configureLogging(LogLevel.Information)
        .withAutomaticReconnect()
        .build()
    let startedPromise = null

    async function start() {
      try {
        startedPromise = await connection.start()
        console.log(`SignalR: Connection started with state: ${connection.state}`)
      } catch (err) {
        console.error('Failed to connect with hub', err)
        return new Promise((resolve, reject) =>
            setTimeout(() => start().then(resolve).catch(reject), 5000))
      }

      return startedPromise
    }

    connection.onclose(async () => await start())

    start()

    // Set to singleton to access globally
    this.instance = connection
  }
}

const signalRSingleton = new SignalRSingleton()
module.exports = signalRSingleton

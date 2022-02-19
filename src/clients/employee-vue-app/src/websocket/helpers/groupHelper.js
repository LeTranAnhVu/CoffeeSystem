import signalRSingleton from '@/websocket/signalRSingleton'
import methodContracts from '../methodContracts'

export async function joinGroup(groupName) {
  const signalR = await signalRSingleton.getConnection()
  await signalR.invoke(methodContracts.JoinGroup, groupName)
  console.log(`Start listen on ${groupName} status in websocket`)
}

export async function leaveGroup(groupName) {
  const signalR = await signalRSingleton.getConnection()
  await signalR.invoke(methodContracts.LeaveGroup, groupName)
  console.log(`Leave ${groupName} status in web socket`)
}

export async function listenTo(methodContract, cb) {
  const signalR = await signalRSingleton.getConnection()
  signalR.on(methodContract, cb)
}

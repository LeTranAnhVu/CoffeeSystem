import {useStore} from 'vuex'

export default function useWebsocket (store){
  store = store ? store : useStore()
  const joinOrderGroup = async () => {
    await store.dispatch('joinOrderGroupAsEmployee')
  }

  const leaveOrderGroup = async () => {
    await store.dispatch('leaveOrderGroupAsEmployee')
  }
  return {
    joinOrderGroup, leaveOrderGroup
  }
}

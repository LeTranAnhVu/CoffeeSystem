import {useStore} from 'vuex'

export default function useWebsocket(store) {
  store = store ? store : useStore()
  const joinOrderGroup = async () => {
    await store.dispatch('joinOrderGroup')
  }

  const leaveOrderGroup = async () => {
    await store.dispatch('leaveOrderGroup')
  }
  return {
    joinOrderGroup, leaveOrderGroup
  }
}

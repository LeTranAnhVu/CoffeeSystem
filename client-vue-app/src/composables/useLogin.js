import {useStore} from 'vuex'
import {computed, watch} from 'vue'
import useWebsocket from '@/composables/useWebsocket'
import {leaveGroup} from '@/websocket/helpers/groupHelper'

export default function useLogin(){
  const store = useStore()
  const login = async (email, password) => {await store.dispatch('login', {email, password})}
  const checkUserLogin = async () => {await store.dispatch('checkUserLogin')}
  const triedLogin = computed(() => store.getters.hasTriedLogin);
  const isUserLogin = computed( () => store.getters.isUserLogin)

  // Actions after login
  watch(isUserLogin, async (newValue) => {
    console.log('isUserLogin', newValue)
    const {joinOrderGroup, leaveOrderGroup} = useWebsocket(store)
    if(newValue) {
      await joinOrderGroup()
    }else{
      await leaveOrderGroup()
    }
  })

  return {
    login,
    checkUserLogin,
    triedLogin
  }
}

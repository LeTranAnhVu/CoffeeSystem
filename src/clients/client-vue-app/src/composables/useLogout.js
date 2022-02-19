import {useStore} from 'vuex'

export default function useLogout(){
  const store = useStore()
  const logout = async () => {await store.dispatch('logout')}

  return {
    logout
  }
}

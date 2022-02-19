import {useStore} from 'vuex'
import {useRouter} from 'vue-router'

export default function useLogout(){
  const store = useStore()
  const router = useRouter()
  const logout = async () => {
    await store.dispatch('logout')
    await router.push({name: 'login'})
  }

  return {
    logout
  }
}

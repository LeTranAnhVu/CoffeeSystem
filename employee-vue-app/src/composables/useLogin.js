import {useStore} from 'vuex'

export default function useLogin(){
  const store = useStore()
  const login = async (email, password) => {await store.dispatch('login', {email, password})}
  const checkUserLogin = async () => {await store.dispatch('checkUserLogin')}

  return {
    login,
    checkUserLogin
  }
}

import {useStore} from 'vuex'

export default function useRegister(){
  const store = useStore()
  const register = async (username, email, password) => {
    const succeeded = await store.dispatch('register', {username, email, password})
    if(succeeded) {
      store.commit('NEED_SHOW_LOGIN_FORM', true)
    }
  }
  return {
    register
  }
}

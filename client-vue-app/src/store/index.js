import {createStore} from 'vuex'
import user from './modules/user-store'

const store = createStore({
  modules: {
    user
  },
  state () {
    return {
      count: 0
    }
  },
  mutations: {
    increment (state) {
      state.count++
    }
  }
})

export default store

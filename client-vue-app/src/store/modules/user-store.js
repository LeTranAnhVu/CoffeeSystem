import {deleteAccessToken, getAccessToken, saveAccessToken} from '@/helpers/tokenHelpers'
import authApi from '@/api/auth'

const createAnonymous = () => ({
  email: null,
  username: null
})

const user = {
  state: () => ({
    user: createAnonymous(),
    isLogin: false,
    triedLogin: false,
    showLoginForm: false
  }),
  mutations: {
    NEED_SHOW_LOGIN_FORM(state, show) {
      state.showLoginForm = show
    },

    UPDATE_USER(state, data) {
      if (!data) {
        state.user = createAnonymous()
      } else {
        state.user = {username: data.username, email: data.email}
      }
    },

    SET_LOGIN_STATUS(state, isLogin) {
      state.isLogin = isLogin
    },

    TRIED_TO_LOGIN(state) {
      state.triedLogin = true
    }
  },
  actions: {
    async login(context, {email, password}) {
      try {
        // add validate
        if (!email || !password) {
          throw new Error('Invalid login credentials')
        }

        const {username, email: loginEmail, accessToken} = await authApi.login(email, password)

        context.commit('UPDATE_USER', {username, email: loginEmail})
        context.commit('SET_LOGIN_STATUS', true)
        saveAccessToken(accessToken)
      } catch (e) {
        await context.dispatch('logout')
      } finally {
        context.commit('TRIED_TO_LOGIN')
      }
    },

    async register(context, {username, email, password}) {
      try {
        // add validate
        if (!username || !email || !password) {
          throw new Error('username, email, and password are required!')
        }

        await authApi.register(username, email, password)
        return true
      } catch (e) {
        return false
      }
    },

    async checkUserLogin(context) {
      try {
        // Try to get token
        const savedToken = getAccessToken()
        if (!savedToken) {
          await context.dispatch('logout')
        }

        const {succeeded, user} = await authApi.validateAccessToken()
        if (!succeeded) {
          await context.dispatch('logout')
        }

        const {username, email} = user
        context.commit('UPDATE_USER', {username, email})
        context.commit('SET_LOGIN_STATUS', true)
      } catch (e) {
        await context.dispatch('logout')
      } finally {
        context.commit('TRIED_TO_LOGIN')
      }

    },

    async logout(context) {
      context.commit('UPDATE_USER', null)
      context.commit('SET_LOGIN_STATUS', false)
      // clear access token
      deleteAccessToken()
    }
  },
  getters: {
    isUserLogin(state) {
      return state.isLogin
    },

    getUserInfo(state) {
      return {...state.user}
    },

    needToShowLoginForm(state) {
      return state.showLoginForm
    },

    hasTriedLogin(state) {
      return state.triedLogin
    }
  }
}

export default user

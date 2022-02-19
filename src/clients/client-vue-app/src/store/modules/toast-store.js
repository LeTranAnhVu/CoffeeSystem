const cart = {
  state: () => ({
    toast: null,
  }),
  mutations: {
    ADD_NEW_TOAST(state, toast) {
      // Toast object example
      // {severity:'info', summary: 'Info Message', detail:'Message Content', life: 1000}
      state.toast = toast
    },

    CLEAN_TOAST(state) {
      state.toast = null
    },
  },
  actions: {
    AddNewToast(context, toastContent) {
      // Toast object example
      // {severity:'info', summary: 'Info Message', detail:'Message Content', life: 1000}
      context.commit('ADD_NEW_TOAST', toastContent)

    }
  },
  getters: {
    getToast: (state) => {
      return state.toast
    },
  }
}

export default cart

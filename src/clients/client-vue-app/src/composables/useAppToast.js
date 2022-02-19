import {useToast} from 'primevue/usetoast'
import {useStore} from 'vuex'
import {computed, watch} from 'vue'

export default function useAppToast() {
  const store = useStore()
  const toast = useToast()

  const comingToast = computed(() => store.getters.getToast)

  // Toast object example
  // {severity:'info', summary: 'Info Message', detail:'Message Content', life: 1000}
  // toast.add({severity:'info', summary: 'Info Message', detail:'Message Content', life: 1000});

  watch(comingToast, (newToast, oldToast) => {
    if(!newToast) return

    console.log('comingToast', newToast.summary)
    toast.add(newToast)
    // Clean the toast
    store.commit('CLEAN_TOAST')
  })
}

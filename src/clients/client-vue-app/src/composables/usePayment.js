// Stripe payment
import {createCheckoutSession, getPaymentPublicKey} from '@/api/payment'
import {loadStripe} from '@stripe/stripe-js'
import {useStore} from 'vuex'
import {ref} from 'vue'

export const usePayment = () => {
  const store = useStore()
  const stripeObj = ref(null)

  const handleCheckout = async (orderId) => {
    const {paymentPublicKey} = await getPaymentPublicKey()
    if (!paymentPublicKey) {
      // Show login tooltip
      const errorToast = {
        severity: 'error',
        summary: 'Checkout failed',
        detail: 'Cannot retrieve payment info',
        life: 3000
      }
      
      await store.dispatch('AddNewToast', errorToast)
    }
    // load stripe
    stripeObj.value = await loadStripe(paymentPublicKey)
    const {checkoutSessionId} = await createCheckoutSession(orderId)
    // When the customer clicks on the button, redirect them to check out.
    const {error} = await stripeObj.value.redirectToCheckout({
      sessionId: checkoutSessionId,
    })

    // If `redirectToCheckout` fails due to a browser or network
    // error, display the localized error message to your customer
    // using `error.message`.
    if (error) {
      // Show login tooltip
      const errorToast = {
        severity: 'error',
        summary: 'Checkout failed',
        detail: 'Checkout is failed, please try again!',
        life: 3000
      }
      await store.dispatch('AddNewToast', errorToast)
    }
  }

  return {
    handleCheckout
  }
}
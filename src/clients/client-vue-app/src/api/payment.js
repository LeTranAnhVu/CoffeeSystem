import {createDefaultHeader} from '@/api/apiHelpers'

const baseUrl = process.env.VUE_APP_BASE_URL

export async function createCheckoutSession(orderId) {
  const api = `/payments/createCheckoutSession/${orderId}`

  const response = await fetch(baseUrl + api, {
    method: 'POST',
    headers: createDefaultHeader(),
    body: {}
  })
  return await response.json()
}

export async function getPaymentPublicKey() {
  const api = `/payments/paymentPublicKey`
  const response = await fetch(baseUrl + api, {
    method: 'GET',
    headers: createDefaultHeader()
  })
  return await response.json()
}

export default {
  createCheckoutSession,
  getPaymentPublicKey
}

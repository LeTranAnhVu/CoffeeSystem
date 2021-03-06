import {createDefaultHeader} from '@/api/apiHelpers'

const baseUrl = process.env.VUE_APP_BASE_URL

export async function fetchOrders() {
  try {
    const api = '/orders'
    const response = await fetch(baseUrl + api, {
      method: 'GET',
      headers: createDefaultHeader()
    })
    return await response.json()
  } catch (e) {
    return []
  }
}

export async function fetchOrderStatuses() {
  try {
    const api = '/orders/statuses'
    const response = await fetch(baseUrl + api, {
      method: 'GET',
      headers: createDefaultHeader()
    })
    return await response.json()
  } catch (e) {
    return []
  }
}

export async function createOrder(order) {
  const api = '/orders'
  const response = await fetch(baseUrl + api, {
    method: 'POST',
    headers: createDefaultHeader(),
    body: JSON.stringify(order)
  })
  return await response.json()
}

export async function cancelOrder(orderId) {
  const api = `/orders/${orderId}/cancel`
  const response = await fetch(baseUrl + api, {
    method: 'PATCH',
    headers: createDefaultHeader(),
  })
  return await response.json()
}

export default {
  createOrder,
  fetchOrders,
  fetchOrderStatuses,
  cancelOrder
}

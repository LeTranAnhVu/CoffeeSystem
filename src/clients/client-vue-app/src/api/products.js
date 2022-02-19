const baseUrl = process.env.VUE_APP_BASE_URL
export async function fetchProducts() {
  try {
    const api = '/products'
    const response = await fetch(baseUrl + api)
    return await response.json()
  } catch (e) {
    return []
  }

}

export default {
  fetchProducts
}

import {createDefaultHeader} from '@/api/apiHelpers'

const baseUrl = process.env.VUE_APP_BASE_URL

async function login(email, password) {
  const api = '/auth/login'
  const response = await fetch(baseUrl + api, {
    method: 'POST',
    headers: createDefaultHeader(),
    body: JSON.stringify({email, password})
  })

  const statusCode = response.statusCode
  if (statusCode === 400) {
    const failData = await response.json()
    console.error(failData.message || 'login fail')
    throw Error(failData.message || 'login fail')
  }

  return await response.json()
}

async function register(username, email, password) {
  const api = '/auth/register'
  const response = await fetch(baseUrl + api, {
    method: 'POST',
    headers: createDefaultHeader(),
    body: JSON.stringify({username, email, password})
  })

  const statusCode = response.statusCode
  if (statusCode === 400) {
    const failData = await response.json()
    console.error(failData.message || 'Bad request')
    throw Error(failData.message || 'Bad request')
  }

  return await response.json()
}

async function validateAccessToken(accessToken) {
  const api = '/auth/validateToken'
  const response = await fetch(baseUrl + api, {
    method: 'GET',
    headers: createDefaultHeader()
  })

  const statusCode = response.statusCode
  if (statusCode === 401) {
    return {succeeded: false}
  }

  return await response.json()
}

export default {
  login,
  register,
  validateAccessToken
}

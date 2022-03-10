import {getAccessToken} from '@/helpers/tokenHelpers'

export function createDefaultHeader() {
  const accessToken = getAccessToken()
  return {
    'Content-Type': 'application/json; charset=utf-8',
    'Accept': 'application/json',
    'Authorization': accessToken ? `Bearer ${accessToken}` : null
  }
}

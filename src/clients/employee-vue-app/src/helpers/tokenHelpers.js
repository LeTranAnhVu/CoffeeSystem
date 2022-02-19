import {ACCESS_TOKEN} from '@/constant'

export function saveAccessToken(value){
  localStorage.setItem(ACCESS_TOKEN, value)
}

export function deleteAccessToken(value){
  localStorage.removeItem(ACCESS_TOKEN)
}

export function getAccessToken(){
  return localStorage.getItem(ACCESS_TOKEN)
}


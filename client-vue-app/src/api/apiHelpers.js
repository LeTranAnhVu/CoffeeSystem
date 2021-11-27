export function createDefaultHeader(accessToken){
  return {
    'Content-Type': 'application/json; charset=utf-8',
    'Accept':'application/json',
    'Authorization': accessToken ? `Bearer ${accessToken}` : null
  }
}

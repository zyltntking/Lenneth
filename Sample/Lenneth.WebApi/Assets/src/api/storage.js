export const SetToken = function (token) {
  sessionStorage.setItem('Token', token)
}

export const GetToken = function () {
  return sessionStorage.getItem('Token')
}

export const SetExpire = function (expire) {
  sessionStorage.setItem('Expire', expire)
}

export const GetExpire = function () {
  return sessionStorage.getItem('Expire')
}

export const GetApiKey = function () {
  return window.ApiKey
}

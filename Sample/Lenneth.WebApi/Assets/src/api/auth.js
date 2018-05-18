import Utility from '../utility'
import * as storage from './storage'

/**
 * @return {boolean}
 */
const TokenString = function (token) {
  return Utility.String.isValid(token)
}

/**
 * @return {boolean}
 */
const TokenNotExpire = function (expire) {
  return !Utility.Date.isExpire(expire)
}

export const InitAuthInfo = function () {
  storage.SetToken('')
  storage.SetExpire(new Date())
}

/**
 * @return {boolean}
 */
export const IsTokenVali = function () {
  return TokenString(storage.GetToken()) && TokenNotExpire(storage.GetExpire())
}

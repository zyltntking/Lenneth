export const isEmpty = function (string) {
  return string.length === 0
}

export const isWhiteSpace = function (string) {
  return !!string.match(/^\s*$/)
}

export const isNull = function (string) {
  return string === null
}

export const isEmptyOrWhiteSpace = function (string) {
  return string === null || string.match(/^ *$/) !== null
}

export const isUndefind = function (string) {
  return typeof (string) === 'undefined'
}

export const isValid = function (string) {
  if (!isUndefind(string)) {
    if (!isEmptyOrWhiteSpace(string)) {
      return true
    }
  }
  return false
}

export const getData = function (string) {
  return new Date(string)
}

export const getTimeStamp = function (string) {
  if (string === null) {
    return Math.round(new Date().getTime() / 1000)
  }
  return Math.round(getData(string).getTime() / 1000)
}

export const isExpire = function (string) {
  return getTimeStamp(null) >= getTimeStamp(string)
}

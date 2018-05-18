import axios from 'axios'
import * as storage from './storage'

export const get = function (request) {
  axios({
    method: 'get',
    url: request.url,
    params: request.data,
    headers: {
      'api_key': storage.GetApiKey(),
      'token': storage.GetToken()
    },
    responseType: 'json',
    responseEncoding: 'utf8'
  }).then(function (response) {
    request.success(response.data)
  }).catch(function (error) {
    request.error(error)
  })
}

export const getPromise = function (request) {
  return new Promise((resolve, reject) => {
    axios({
      method: 'get',
      url: request.url,
      params: request.data,
      headers: {
        'api_key': storage.GetApiKey(),
        'token': storage.GetToken()
      },
      responseType: 'json',
      responseEncoding: 'utf8'
    }).then(function (response) {
      resolve(response.data)
    }).catch(function (error) {
      reject(error)
    })
  })
}

export const post = function (request) {
  axios({
    method: 'post',
    url: request.url,
    data: request.data,
    headers: {
      'ApiKey': storage.GetApiKey(),
      'Token': storage.GetToken()
    },
    responseType: 'json',
    responseEncoding: 'utf8'
  }).then(function (response) {
    request.success(response.data)
  }).catch(function (error) {
    request.error(error)
  })
}

export const postPromise = function (request) {
  return new Promise((resolve, reject) => {
    axios({
      method: 'post',
      url: request.url,
      data: request.data,
      headers: {
        'ApiKey': storage.GetApiKey(),
        'Token': storage.GetToken()
      },
      responseType: 'json',
      responseEncoding: 'utf8'
    }).then(function (response) {
      resolve(response.data)
    }).catch(function (error) {
      reject(error)
    })
  })
}

/**
 * 导入状态树组件
 */
import Vue from 'vue'
import Vuex from 'vuex'

import state from './state'
import getters from './getters'
import mutations from './mutations'

// 加载状态树组件
Vue.use(Vuex)

// 创建状态树
const store = new Vuex.Store({
  // strict: true,
  state,
  getters,
  mutations
})

export default store

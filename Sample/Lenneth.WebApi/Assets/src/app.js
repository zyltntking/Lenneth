/* eslint-disable no-new */
// 导入UI组件
import Vue from 'vue'
import ElementUI from 'element-ui'
import locale from 'element-ui/lib/locale/lang/zh-CN'

import DataTables, { DataTablesServer } from 'vue-data-tables'

import store from './store'
import router from './router'
import i18n from './i18n'

import App from './views/App'
// 加载UI组件
Vue.use(ElementUI, { locale })
Vue.use(DataTables)
Vue.use(DataTablesServer)

// 构建程序实例
new Vue({
  el: '#app',
  store,
  router,
  i18n,
  render: h => h(App)
})

import Vue from 'vue'
import VueRouter from 'vue-router'

import routes from './routes'

import Common from '../common'

Vue.use(VueRouter)

const router = new VueRouter({
  routes
})

// 路由守卫
// 屏蔽未匹配路由
router.beforeEach((to, from, next) => {
  if (to.matched.length === 0) {
    // 如果未匹配到路由
    // 如果上级也未匹配到路由则跳转到根路由，如果上级能匹配到则转上级路由
    from.name ? next({
      name: from.name
    }) : next({
      name: Common.RouteMap.Root.Cover.name
    })
  } else {
    // 如果匹配到正确跳转
    next()
  }
})

export default router

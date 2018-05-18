<template>
  <div v-cloak :style="{width: $store.state.layout.app.width + 'px',height : $store.state.layout.app.height + 'px'}">
    <router-view></router-view>
  </div>
</template>

<script>
import api from '../api'
import Common from '../common'
export default {
  name: 'App',
  created () {
    const that = this
    const isCover = that.$router.currentRoute.name === Common.RouteMap.Root.Cover.name
    api.request.post({
      url: '/WebApi/v1/Auth/SilentAuth',
      success: function (data) {
        if (data.Code === 0) {
          api.storage.SetToken(data.Data.Token)
          api.storage.SetExpire(data.Data.Expire)
          if (isCover) {
            that.$message.info({
              showClose: true,
              message: '静默授权成功'
            })
            that.$router.push({
              name: Common.RouteMap.Root.Data.name
            })
          }
        } else {
          api.auth.InitAuthInfo()
          if (!isCover) {
            that.$message.error({
              showClose: true,
              message: '可用凭证已失效或过期，请重新登陆'
            })
            that.$router.push({
              name: Common.RouteMap.Root.Cover.name
            })
          }
        }
      },
      error: function (data) {
        api.auth.InitAuthInfo()
        if (!isCover) {
          that.$message.error({
            showClose: true,
            message: '可用凭证已失效或过期，请重新登陆'
          })
          that.$router.push({
            name: Common.RouteMap.Root.Cover.name
          })
        }
      }
    })
  },
  mounted () {
    let that = this
    window.onresize = function () {
      that.$store.commit(Common.MutationMap.Layout.AppSizeResponsive, {
        width: document.body.offsetWidth,
        height: document.body.offsetHeight
      })
    }
  }
}
</script>

<style>
  [v-cloak] {
    display: none
  }
</style>

<template>
  <div class="cover">
    <el-card v-if="!isTokenValid" style="background: rgba(255,255,255,0.2)" shadow="always" class="fix-center">
      <div class="form">
        <div class="form-horizontal col-md-offset-2">
          <h3 style="color: white">{{$t('message.banner')}}</h3>
          <div class="col-md-9">
            <div class="form-group">
              <i class="fa fa-user fa-lg"></i>
              <input class="form-control required"
                     type="text"
                     placeholder="请输入用户名"
                     autofocus="autofocus"
                     maxlength="20"
                     v-model="username"/>
            </div>
            <div class="form-group">
              <i class="fa fa-lock fa-lg"></i>
              <input class="form-control required"
                     type="password"
                     placeholder="请输入登陆密码"
                     maxlength="8"
                     v-model="password"/>
            </div>
            <div class="form-group col-md-offset-9">
              <button class="btn btn-info pull-right" @click="login">登录</button>
            </div>
          </div>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script>
import api from '../api'
import common from '../common'
export default {
  name: 'Cover',
  data () {
    return {
      username: 'super',
      password: '123456',
      isTokenValid: api.auth.IsTokenVali()
    }
  },
  methods: {
    login: function () {
      const that = this
      api.request.post({
        url: '/WebApi/v1/Auth/Login',
        data: {
          UserName: that.username,
          PassWord: that.password
        },
        success: function (data) {
          if (data.Code === 0) {
            api.storage.SetToken(data.Data.Token)
            api.storage.SetExpire(data.Data.Expire)
            that.$message.success({
              showClose: true,
              message: '登陆成功'
            })
            that.$router.push({
              name: common.RouteMap.Root.Data.name
            })
          } else {
            that.$message.error(data.Message)
          }
        },
        error: function (data) {
          alert(data.message)
        }
      })
    }
  }
}
</script>

<style lang="scss">
  .cover {
    width: 100%;
    height: 100%;
    margin: 0 auto;
    background: url("../assets/img/cover.jpg") no-repeat fixed center;
    background-size: cover;
  }

  .fix-center {
    position: absolute;
    left: 50%;
    top: 50%;
    transform: translate(-50%, -50%);
  }

  .form {
    width:360px;
  }

  /*阴影*/
  .fa {
    display: inline-block;
    top: 27px;
    left: 6px;
    position: relative;
    color: #ccc;
  }

  input[type="text"], input[type="password"] {
    padding-left: 26px;
  }

  .checkbox {
    padding-left: 21px;
  }
</style>

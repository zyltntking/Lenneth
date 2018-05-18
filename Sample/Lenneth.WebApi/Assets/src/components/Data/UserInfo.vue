<template>
  <data-tables-server
    :data="data"
    :total="total"
    :loading="loading"
    :search-def="searchDef"
    :pagination-def="paginationDef"
    :table-props="tableProps"
    @query-change="loadData">
    <el-table-column
      type="index"
      width="50"
      fixed>
    </el-table-column>
    <el-table-column
      prop="Type"
      label="用户类型"
      :formatter="typeFormat"
      width="120">
    </el-table-column>
    <el-table-column
      v-for="title in titles"
      :key="title.prop"
      :prop="title.prop"
      :label="title.label"
      width = "180"
      show-overflow-tooltip>
    </el-table-column>
  </data-tables-server>
</template>

<script>
import Common from '../../common'
import api from '../../api'

export default {
  name: 'UserInfo',
  created () {
    this.$store.commit(Common.MutationMap.UserInterface.UpdateSideBarActiveItem, Common.RouteMap.Data.UserInfo.path)
  },
  data () {
    return {
      data: [],
      total: 0,
      loading: false,
      searchDef: {
        props: 'MobileNo',
        colProps: {
          span: 14
        },
        inputProps: {
          placeholder: '输入用户手机号搜索'
        }
      },
      paginationDef: {
        pageSize: 20,
        pageSizes: [20],
        currentPage: 1
      },
      tableProps: {
        border: true,
        stripe: true,
        maxHeight: 400
      },
      titles: [
        { prop: 'Uid', label: '用户标识' },
        { prop: 'UserName', label: '用户名' },
        { prop: 'IdNo', label: '身份证号' },
        { prop: 'Email', label: '邮箱地址' },
        { prop: 'MobileNo', label: '手机号码' },
        { prop: 'Gender', label: '用户性别' },
        { prop: 'Birthday', label: '用户生日' },
        { prop: 'Province', label: '用户省份' },
        { prop: 'City', label: ' 用户城市' },
        { prop: 'Address', label: '用户地址' },
        { prop: 'Spell', label: '姓名拼音' },
        { prop: 'BankCard', label: '默认银行卡' },
        { prop: 'IdType', label: '证件类型' },
        { prop: 'Money', label: '余额' }
      ]
    }
  },
  methods: {
    loadData (queryInfo) {
      let that = this
      that.loading = true
      api.request.post({
        url: '/WebApi/v1/MassiveData/UserInfo',
        data: queryInfo,
        success: function (data) {
          if (data.Code === 0) {
            that.data = data.Data.data
            that.total = data.Data.total
            that.loading = false
          } else {
            that.loading = false
            that.$message.error(data.Message)
          }
        },
        error: function (data) {
          alert(data.message)
          that.loading = false
        }
      })
    },
    typeFormat (row, column, cellValue, index) {
      return this.$t('message.Data.UserInfo.Type.' + cellValue)
    }
  }
}
</script>

<style scoped>

</style>

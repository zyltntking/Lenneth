<template>
  <el-row>
    <el-col :span="24">
      <el-menu
        :default-active="$store.state.userInterface.sidebar.itemActive"
        :style = "{height: $store.state.layout.app.height - $store.state.layout.navbar.height + 'px'}"
        style="overflow: auto"
        @select="handleSelect"
        router
        background-color="#545c64"
        text-color="#fff"
        active-text-color="#ffd04b">
        <el-menu-item v-for="item in $store.getters.sideBarMenu"
                      :key="item.name"
                      :index="item.path">
          <i class="el-icon-caret-right"></i>
          <span slot="title">{{item.alias}}</span>
        </el-menu-item>
      </el-menu>
    </el-col>
  </el-row>
</template>

<script>
import Common from '../../../common'
export default {
  name: 'Sidebar',
  methods: {
    handleSelect (key, keyPath) {
      this.$store.commit(Common.MutationMap.UserInterface.UpdateSideBarActiveItem, key)
    }
  }
}
</script>

<style scoped>

</style>

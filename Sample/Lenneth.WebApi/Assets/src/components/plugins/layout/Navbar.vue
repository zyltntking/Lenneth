<template>
  <el-row>
    <el-col :span="24">
      <el-menu
        :default-active="$store.state.userInterface.navActive"
        @select="handleSelect"
        mode="horizontal"
        router
        background-color="#545c64"
        text-color="#fff"
        active-text-color="#ffd04b">
        <el-menu-item :style="{width: $store.state.layout.sidebar.width + 'px'}" index="/">
          <i class="el-icon-menu"></i><span>{{ $t("message.banner") }}</span>
        </el-menu-item>
        <el-menu-item v-for="item in $store.state.userInterface.navMenu"
                      :key="item.name"
                      :index="item.path">
          {{item.alias}}
        </el-menu-item>
      </el-menu>
    </el-col>
  </el-row>
</template>

<script>
import Common from '../../../common'
export default {
  name: 'navbar',
  methods: {
    handleSelect (key, keyPath) {
      this.$store.commit(Common.MutationMap.UserInterface.UpdateNavActiveItem, key)
    }
  }
}
</script>

<style scoped>

</style>

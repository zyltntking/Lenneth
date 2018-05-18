<template>
  <el-container>
    <el-header :height="$store.state.layout.navbar.height + 'px'" class="clear-box">
      <navbar/>
    </el-header>
    <el-container>
      <el-aside class="clear-box aside-menu" :width="$store.state.layout.sidebar.width + 'px'">
        <sidebar/>
      </el-aside>
      <el-main class="clear-box main-content"
               style="overflow: auto;padding: 4px"
               :style="{
               width: $store.getters.mainSize.width + 'px' ,
               height: $store.getters.mainSize.height + 'px'}">
        <el-card style="width: 100%;height: 100%" shadow="hover">
          <slot name="content"></slot>
        </el-card>
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
import Navbar from './plugins/layout/Navbar'
import Sidebar from './plugins/layout/Sidebar'
export default {
  name: 'Layout',
  components: {
    Navbar,
    Sidebar
  }
}
</script>

<style scoped lang="scss">
.aside-menu, .main-content{
  overflow: auto;
}
</style>

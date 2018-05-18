<template>
  <layout>
    <h1 slot="content">System模板</h1>
  </layout>
</template>

<script>
import Layout from './Layout'
import Common from '../common'
export default {
  name: 'System',
  created () {
    const System = Common.RouteMap.Root.System
    this.$store.commit(Common.MutationMap.UserInterface.UpdateNavActiveItem, System.path)
    this.$store.commit(Common.MutationMap.UserInterface.UpdateActiveSideBar, System.name)
  },
  components: {
    Layout
  }
}
</script>

<style scoped>

</style>

<template>
    <layout>
      <div slot="content">
        <router-view></router-view>
      </div>
    </layout>
</template>

<script>
import Layout from './Layout'
import Common from '../common'
export default {
  name: 'Data',
  created () {
    const Data = Common.RouteMap.Root.Data
    this.$store.commit(Common.MutationMap.UserInterface.UpdateNavActiveItem, Data.path)
    this.$store.commit(Common.MutationMap.UserInterface.UpdateActiveSideBar, Data.name)
  },
  components: {
    Layout
  }
}
</script>

<style scoped>

</style>

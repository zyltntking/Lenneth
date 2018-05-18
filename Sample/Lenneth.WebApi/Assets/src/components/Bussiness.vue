<template>
  <layout>
    <h1 slot="content">Business模板</h1>
  </layout>
</template>

<script>
import Layout from './Layout'
import Common from '../common'
export default {
  name: 'Bussiness',
  created () {
    const Bussiness = Common.RouteMap.Root.Bussiness
    this.$store.commit(Common.MutationMap.UserInterface.UpdateNavActiveItem, Bussiness.path)
    this.$store.commit(Common.MutationMap.UserInterface.UpdateActiveSideBar, Bussiness.name)
  },
  components: {
    Layout
  }
}
</script>

<style scoped>

</style>

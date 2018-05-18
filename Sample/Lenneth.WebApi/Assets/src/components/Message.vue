<template>
  <layout>
    <h1 slot="content">Message模板</h1>
  </layout>
</template>

<script>
import Layout from './Layout'
import Common from '../common'
export default {
  name: 'Message',
  created () {
    const Message = Common.RouteMap.Root.Message
    this.$store.commit(Common.MutationMap.UserInterface.UpdateNavActiveItem, Message.path)
    this.$store.commit(Common.MutationMap.UserInterface.UpdateActiveSideBar, Message.name)
  },
  components: {
    Layout
  }
}
</script>

<style scoped>

</style>

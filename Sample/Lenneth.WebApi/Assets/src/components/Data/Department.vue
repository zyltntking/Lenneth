<template>
  <div>
    <h1>Department View</h1>
  </div>
</template>

<script>
import Common from '../../common'
export default {
  name: 'Department',
  created () {
    this.$store.commit(Common.MutationMap.UserInterface.UpdateSideBarActiveItem, Common.RouteMap.Data.Department.path)
  }
}
</script>

<style scoped>

</style>

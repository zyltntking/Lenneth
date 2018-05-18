import Common from '../../common'

const mutations = {
  // Layout
  [Common.MutationMap.Layout.AppSizeResponsive] (state, size) {
    state.layout.app.width = size.width
    state.layout.app.height = size.height
  },
  // UserInterface
  [Common.MutationMap.UserInterface.UpdateNavActiveItem] (state, active) {
    state.userInterface.navActive = active
  },
  [Common.MutationMap.UserInterface.UpdateSideBarActiveItem] (state, active) {
    state.userInterface.sidebar.itemActive = active
  },
  [Common.MutationMap.UserInterface.UpdateActiveSideBar] (state, active) {
    state.userInterface.activeSideBar = active
  }
}

export default mutations

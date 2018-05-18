const getters = {
  sideBarMenu: function (state) {
    const menu = state.userInterface.activeSideBar
    return state.userInterface.sidebar[menu]
  },
  mainSize: function (state) {
    return {
      width: state.layout.app.width - state.layout.sidebar.width,
      height: state.layout.app.height - state.layout.navbar.height
    }
  }
}

export default getters

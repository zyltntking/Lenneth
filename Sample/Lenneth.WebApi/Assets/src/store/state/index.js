import Common from '../../common'

const state = {
  // unit: px
  layout: {
    app: {
      width: document.body.offsetWidth,
      height: document.body.offsetHeight
    },
    navbar: {
      height: 60
    },
    sidebar: {
      width: 200
    }
  },
  userInterface: {
    navActive: Common.RouteMap.Root.Data.path,
    navMenu: [
      Common.RouteMap.Root.Data,
      Common.RouteMap.Root.Bussiness,
      Common.RouteMap.Root.System,
      Common.RouteMap.Root.Message
    ],
    activeSideBar: Common.RouteMap.Root.Data.name,
    sidebar: {
      itemActive: Common.RouteMap.Data.UserInfo.path,
      Data: [
        Common.RouteMap.Data.UserInfo,
        Common.RouteMap.Data.Department
      ],
      System: [
      ],
      Message: [
      ]
    }
  }
}

export default state

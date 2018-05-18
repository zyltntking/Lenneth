// root
import Cover from '../components/Cover'
import Data from '../components/Data'
import Business from '../components/Bussiness'
import System from '../components/System'
import Message from '../components/Message'

// data
import UserInfo from '../components/Data/UserInfo'
import Department from '../components/Data/Department'

import Common from '../common'

const routes = [
  {
    path: Common.RouteMap.Root.Cover.path,
    name: Common.RouteMap.Root.Cover.name,
    component: Cover

  },
  {
    path: Common.RouteMap.Root.Data.path,
    name: Common.RouteMap.Root.Data.name,
    component: Data,
    redirect: {
      name: Common.RouteMap.Data.UserInfo.name
    },
    children: [
      {
        path: Common.RouteMap.Data.UserInfo.path,
        name: Common.RouteMap.Data.UserInfo.name,
        component: UserInfo
      },
      {
        path: Common.RouteMap.Data.Department.path,
        name: Common.RouteMap.Data.Department.name,
        component: Department
      }
    ]
  },
  {
    path: Common.RouteMap.Root.Bussiness.path,
    name: Common.RouteMap.Root.Bussiness.name,
    component: Business
  },
  {
    path: Common.RouteMap.Root.System.path,
    name: Common.RouteMap.Root.System.name,
    component: System
  },
  {
    path: Common.RouteMap.Root.Message.path,
    name: Common.RouteMap.Root.Message.name,
    component: Message
  }
]

export default routes

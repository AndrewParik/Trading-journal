import { createRouter, createWebHistory } from 'vue-router'
import Lobby from '../views/Lobby.vue'
import Profile from '../views/Profil.vue'
import TradeDetail from '../views/TradeDetail.vue'
import MainPage from '../views/MainPage.vue'
import Edit from '../views/Edit.vue'

const routes = [
  { path: '/', component: MainPage },
  { path: '/lobby', component: Lobby },
  { path: '/profile/id', component: Profile },
  { path: '/trades/id', component: TradeDetail, props: true },
  { path: '/edit/id', component: Edit}
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router

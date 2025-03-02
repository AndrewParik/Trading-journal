import { createRouter, createWebHistory } from 'vue-router'
import Lobby from '../views/Lobby.vue'
import Profile from '../views/Profil.vue'
import TradeDetail from '../views/TradeDetail.vue'
import MainPage from '../views/MainPage.vue'
import Edit from '../views/Edit.vue'

const routes = [
  { path: '/', component: MainPage },
  { path: '/lobby', component: Lobby },
  { path: '/profile/0', component: Profile, props: true }, 
  { path: '/trades/0', component: TradeDetail, props: true }, 
  { path: '/edit/0', component: Edit, props: true } 
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router

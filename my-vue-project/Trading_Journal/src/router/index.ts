import { createRouter, createWebHistory } from 'vue-router'
import Lobby from '../views/Lobby.vue'
import Profile from '../views/Profil.vue'
import TradeDetail from '../views/TradeDetail.vue'
import MainPage from '../views/MainPage.vue'

const routes = [
  { path: '/', component: MainPage },
  { path: '/lobby', component: Lobby },
  { path: '/profile', component: Profile },
  { path: '/trade/:id', component: TradeDetail, props: true }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router

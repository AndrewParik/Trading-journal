import { createRouter, createWebHistory } from 'vue-router'
import MainPage from '../views/MainPage.vue'
import Lobby from '../views/Lobby.vue'

const routes = [
  { path: '/', component: MainPage },
  { path: '/lobby', component: Lobby }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router

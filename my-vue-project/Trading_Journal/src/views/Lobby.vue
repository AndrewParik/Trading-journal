<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '../api/axiosInstance'

const router = useRouter()
const userId = localStorage.getItem('userId')

const user = ref<{
  id: number
  firstName: string
  lastName: string
  email: string
  avatar: string
  portfolioValue: number
} | null>(null)

const trades = ref<Array<{
  id: number
  coinType: string
  worth: number
  dateCreated: string
  idTrader: number
}>>([])

const errorMessage = ref('')

const fetchUserData = async () => {
  if (!userId) {
    errorMessage.value = 'Uživatel není přihlášen.'
    return
  }
  try {
    const response = await api.get(`/users/${userId}`)
    user.value = response.data
  } catch (error) {
    errorMessage.value = 'Nepodařilo se načíst uživatelská data.'
  }
}

const fetchTrades = async () => {
  if (!userId) return
  try {
    const response = await api.get(`/trades/user/${userId}`)
    trades.value = response.data
  } catch (error) {
    errorMessage.value = 'Nepodařilo se načíst obchody z API.'
  }
}

onMounted(async () => {
  await fetchUserData()
  await fetchTrades()
})
</script>

<template>
  <div class="lobby">
    <nav class="navbar">
      <button @click="router.push('/lobby')">🏠 Lobby</button>
      <button @click="router.push('/profile')">👤 Profil</button>
      <button @click="router.push('/trades')">📈 Obchody</button>
      <button @click="router.push('/')">🚪 Odhlásit</button>
    </nav>

    <h1>📊 Přehled portfolia</h1>

    <div v-if="user">
      <div class="portfolio">
        <p>💰 Aktuální hodnota portfolia:</p>
        <h2>{{ user.portfolioValue.toLocaleString() }} Kč</h2>
      </div>

      <div class="user-info">
        <img :src="user.avatar || 'https://via.placeholder.com/150'" alt="Profilový obrázek" class="avatar" />
        <div class="details">
          <p><strong>👤 Jméno:</strong> {{ user.firstName }} {{ user.lastName }}</p>
          <p><strong>📧 Email:</strong> {{ user.email }}</p>
        </div>
      </div>
    </div>

    <div class="trades">
      <h3>📈 Moje obchody</h3>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Coin</th>
            <th>Hodnota</th>
            <th>Datum</th>
            <th>Akce</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="trade in trades" :key="trade.id">
            <td>{{ trade.id }}</td>
            <td>{{ trade.coinType }}</td>
            <td>{{ trade.worth.toLocaleString() }} Kč</td>
            <td>{{ new Date(trade.dateCreated).toLocaleDateString() }}</td>
            <td>
              <button @click="router.push(`/trade/${trade.id}`)">📄 Detail</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>

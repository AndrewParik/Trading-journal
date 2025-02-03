<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '../api/axiosInstance'

const router = useRouter()
const userId = localStorage.getItem('userId')

interface User {
  id: number
  firstName: string
  lastName: string
  avatar: string
  portfolioValue: number
  trades: Trade[]
}

interface Trade {
  id: number
  coinType: string
  worth: number
  dateCreated: string
  idTrader: number
}

const user = ref<User | null>(null)
const trades = ref<Trade[]>([])
const errorMessage = ref<string>('')

const fetchUserData = async () => {
  if (!userId) {
    errorMessage.value = '❌ Uživatel není přihlášen.'
    return
  }
  try {
    const response = await api.get(`/api/trader/${userId}`)
    user.value = response.data
    trades.value = response.data.trades 
  } catch (error) {
    console.error('Chyba při načítání uživatele:', error)
    errorMessage.value = '❌ Nepodařilo se načíst uživatelská data.'
  }
}

// const fetchTrades = async () => {
//   if (!userId) return
//   try {
//     const response = await api.get(`/api/trader/${userId}`)
//     trades.value = response.data.trades
//   } catch (error) {
//     console.error('Chyba při načítání obchodů:', error)
//     errorMessage.value = '❌ Nepodařilo se načíst obchody z API.'
//   }
// }

onMounted(fetchUserData)
</script>

<template>
  <div class="background-container">
    <div class="overlay"></div>

    <div class="lobby">
      <nav class="navbar">
        <router-link to="/lobby">🏠 Lobby</router-link>
        <router-link to="/profil/">👤 Profil</router-link>
        <router-link to="/trades/">📈 Obchody</router-link>
        <router-link to="/">🚪 Odhlásit</router-link>
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
            </tr>
          </thead>
          <tbody>
            <tr v-for="trade in trades" :key="trade.id">
              <td>{{ trade.id }}</td>
              <td>{{ trade.coinType }}</td>
              <td>{{ trade.worth.toLocaleString() }} Kč</td>
              <td>{{ new Date(trade.dateCreated).toLocaleDateString() }}</td>
              <td>
                <router-link :to="`/trade/${trade.id}`" class="detail-button">📄 Detail</router-link>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
    </div>
  </div>
</template>

<style scoped>
.background-container {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
}

.overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  background: rgba(0, 0, 0, 0.5);
  backdrop-filter: blur(8px);
}

.lobby {
  position: relative;
  background: gray;
  padding: 30px;
  border-radius: 12px;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
  text-align: center;
  width: 80%;
  color: white;
  max-width: 800px;
  animation: fadeIn 0.6s ease-in-out;
}

.navbar {
  display: flex;
  justify-content: space-around;
  background: #3498db;
  padding: 10px;
  border-radius: 5px;
  margin-bottom: 20px;
}

.navbar a {
  text-decoration: none;
  color: white;
  font-weight: bold;
  padding: 10px 15px;
  border-radius: 5px;
  transition: 0.3s;
}

.navbar a:hover {
  background: rgba(255, 255, 255, 0.2);
}

.portfolio {
  font-size: 22px;
  font-weight: bold;
  background: white;
  padding: 20px;
  border-radius: 10px;
  margin-bottom: 20px;
}

h2 {
  font-size: 36px;
  color: white;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 20px;
  background: black;
  padding: 20px;
  border-radius: 10px;
  margin-bottom: 20px;
}

.avatar {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  object-fit: cover;
}

.details {
  text-align: left;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 10px;
  background: white;
  border-radius: 10px;
  overflow: hidden;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.1);
}

th, td {
  border: 1px solid #ddd;
  color: white;
  background-color: lightblue;
  padding: 12px;
  text-align: center;
}

th {
  background: black;
}

.detail-button {
  text-decoration: none;
  background: #3498db;
  color: white;
  padding: 5px 10px;
  border-radius: 5px;
  transition: 0.3s;
}

.detail-button:hover {
  background: #2980b9;
}

.error {
  color: red;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>

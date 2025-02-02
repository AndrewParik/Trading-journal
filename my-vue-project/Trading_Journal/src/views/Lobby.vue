<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '../api/axiosInstance'

const router = useRouter()
const userId = localStorage.getItem('userId') || '1'

const user = ref<{
  id: number,
  firstName: string,
  lastName: string,
  email: string,
  avatar: string,
  portfolioValue: number
} | null>(null)

const trades = ref<Array<{
  id: number,
  coinType: string,
  worth: number,
  dateCreated: string,
  idTrader: number
}>>([])

const errorMessage = ref('')

const fetchUserData = async () => {
  try {
    const response = await api.get(`/user/${userId}`)
    user.value = response.data
  } catch (error) {
    errorMessage.value = 'Nepodařilo se načíst uživatelská data.'
  }
}

const fetchTrades = async () => {
  if (!user.value) return
  try {
    const response = await api.get(`/trades/${userId}`)
    trades.value = response.data
  } catch (error) {
    errorMessage.value = 'Nepodařilo se načíst obchody z API.'
  }
}

const goToTradeDetail = (tradeId: number) => {
  router.push(`/trade/${tradeId}`)
}

onMounted(async () => {
  await fetchUserData()
  await fetchTrades()
})
</script>

<template>
  <div class="lobby">
    <h1>📊 Přehled portfolia</h1>

    <div v-if="user">
      <div class="user-info">
        <img :src="user.avatar || 'https://via.placeholder.com/150'" alt="Profilový obrázek" class="avatar" />
        <div class="details">
          <p><strong>👤 Jméno:</strong> {{ user.firstName }} {{ user.lastName }}</p>
          <p><strong>📧 Email:</strong> {{ user.email }}</p>
          <p><strong>💰 Portfolio:</strong> {{ user.portfolioValue.toLocaleString() }} Kč</p>
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
              <button @click="goToTradeDetail(trade.id)">📄 Detail</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>

<style scoped>
.lobby {
  max-width: 800px;
  margin: 50px auto;
  text-align: center;
  font-family: Arial, sans-serif;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 20px;
  background: #f9f9f9;
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

.trades {
  text-align: center;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 10px;
}

th, td {
  border: 1px solid #ddd;
  padding: 8px;
}

th {
  background: #f4f4f4;
}

button {
  padding: 5px 10px;
  font-size: 14px;
  cursor: pointer;
  background: #3498db;
  color: white;
  border: none;
  border-radius: 5px;
}

button:hover {
  background: #2980b9;
}

.error {
  color: red;
}
</style>

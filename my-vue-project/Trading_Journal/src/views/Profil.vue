<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../api/axiosInstance'

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
    const response = await api.get('/user/1') 
    user.value = response.data
  } catch (error) {
    console.error('Chyba při načítání uživatelských dat:', error)
    errorMessage.value = 'Nepodařilo se načíst uživatelská data.'
  }
}

const fetchTrades = async () => {
  if (!user.value) return
  try {
    const response = await api.get(`/trades/${user.value.id}`)
    trades.value = response.data
  } catch (error) {
    console.error('Chyba při načítání obchodů:', error)
    errorMessage.value = 'Nepodařilo se načíst obchody z API.'
  }
}

onMounted(async () => {
  await fetchUserData()
  await fetchTrades()
})
</script>

<template>
  <div class="profile">
    <h1>Můj profil</h1>

    <div v-if="user">
      <div class="avatar-container">
        <img :src="user.avatar || 'https://via.placeholder.com/150'" alt="Profilový obrázek" class="avatar" />
      </div>

      <div class="profile-info">
        <p><strong>Jméno:</strong> {{ user.firstName }} {{ user.lastName }}</p>
        <p><strong>Email:</strong> {{ user.email }}</p>
        <p><strong>💰 Stav portfolia:</strong> {{ user.portfolioValue.toLocaleString() }} Kč</p>
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
          </tr>
        </tbody>
      </table>
    </div>

    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>

<style scoped>
.profile {
  max-width: 600px;
  margin: 50px auto;
  text-align: center;
  font-family: Arial, sans-serif;
}

.avatar-container {
  display: flex;
  justify-content: center;
  margin-bottom: 20px;
}

.avatar {
  width: 150px;
  height: 150px;
  border-radius: 50%;
  object-fit: cover;
}

.profile-info {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-bottom: 20px;
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

.error {
  color: red;
}
</style>

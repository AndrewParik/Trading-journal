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

onMounted(async () => {
  await fetchUserData()
})
</script>

<template>
  <div class="profile-container">
    <div class="profile-banner">
      <div class="overlay"></div>
      <div class="profile-header">
        <img :src="user?.avatar || 'https://via.placeholder.com/150'" alt="Profilový obrázek" class="avatar" />
        <h1>{{ user?.firstName }} {{ user?.lastName }}</h1>
      </div>
    </div>

    <div class="profile-content">
      <div class="info-card">
        <h2>📊 Informace o uživateli</h2>
        <p><strong>👤 Jméno:</strong> {{ user?.firstName }} {{ user?.lastName }}</p>
        <p><strong>📧 Email:</strong> {{ user?.email }}</p>
        <p><strong>💰 Stav portfolia:</strong> {{ user?.portfolioValue.toLocaleString() }} Kč</p>
      </div>

      <div class="trades-card">
        <h2>📈 Moje obchody</h2>
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
    </div>

    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>

<style scoped>
.profile-container {
  background: #121212;
  color: white;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  font-family: 'Arial', sans-serif;
}

.profile-banner {
  width: 100%;
  height: 250px;
  background: url('https://source.unsplash.com/1600x500/?technology,finance') no-repeat center/cover;
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
}

.overlay {
  position: absolute;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.6);
}

.profile-header {
  position: relative;
  display: flex;
  align-items: center;
  gap: 20px;
}

.avatar {
  width: 120px;
  height: 120px;
  border-radius: 50%;
  object-fit: cover;
  border: 4px solid white;
}

.profile-content {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  gap: 20px;
  margin-top: -40px;
  width: 90%;
  max-width: 1000px;
}

.info-card, .trades-card {
  background: #1e1e1e;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
  width: 100%;
  max-width: 600px;
}

h2 {
  margin-bottom: 10px;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 10px;
  background: #2a2a2a;
  border-radius: 10px;
  overflow: hidden;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.1);
}

th, td {
  padding: 12px;
  text-align: center;
}

th {
  background: #333;
}

td {
  border-bottom: 1px solid #444;
}

.error {
  color: red;
  margin-top: 10px;
}

table tr:hover {
  background: rgba(255, 255, 255, 0.1);
  transition: 0.3s;
}
</style>

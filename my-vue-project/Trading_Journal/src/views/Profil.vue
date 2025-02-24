<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../api/axiosInstance'

const profil = ref(JSON.parse(localStorage.getItem('user') || '{}'));

onMounted(() => {
  if (!profil.value.userId) {
    errorMessage.value = '❌ Uživatel není přihlášen.';
    router.push('/');
  }
});


const route = useRoute()
const router = useRouter()
const userId = route.params.id

interface User {
  id: number
  firstName: string
  lastName: string
  portfolioValue: number
  trades: Trade[]
}

interface Trade {
  id: number
  coinType: string
  worth: number
  dateCreated: string
}

const user = ref<User | null>(null)
const errorMessage = ref('')

const fetchUserData = async () => {
  if (!userId) {
    errorMessage.value = '❌ Uživatel není přihlášen.'
    return
  }

  try {
    const response = await api.get(`/trader/${userId}`)
    user.value = response.data
  } catch (error) {
    console.error('❌ Chyba při načítání uživatele:', error)
    errorMessage.value = '❌ Nepodařilo se načíst uživatelská data.'
  }
}

onMounted(fetchUserData)
</script>

<template>
  <div class="profile-container">
    <div class="profile-banner">
      <div class="overlay"></div>
      <div class="profile-header">
        <h1 v-if="user">{{ user.firstName }} {{ user.lastName }}</h1>
        <h1 v-else>⏳ Načítání...</h1>
      </div>
    </div>

    <div class="profile-content">
      <div class="info-card" v-if="user">
        <h2>📊 Informace o uživateli</h2>
        <p><strong>👤 Jméno:</strong> {{ user.firstName }} {{ user.lastName }}</p>
        <p><strong>💰 Stav portfolia:</strong> {{ user.portfolioValue.toLocaleString() }} Kč</p>
      </div>

      <div class="trades-card" v-if="user && user.trades.length">
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
            <tr v-for="trade in user.trades" :key="trade.id">
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

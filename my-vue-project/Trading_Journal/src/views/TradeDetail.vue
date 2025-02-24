<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../api/axiosInstance'

const route = useRoute()
const router = useRouter()
const userId = route.params.id
const user1 = ref(JSON.parse(localStorage.getItem('user') || '{}'));

interface Trade {
  id: number
  coinType: string
  worth: number
  dateCreated: string
  idTrader: number
}

const trade = ref<Trade | null>(null)
const errorMessage = ref<string>('')

const fetchTrade = async () => {
  try {
    const response = await api.get('/trader/login',   
    {
      
    }
  )
    } catch (error) {
    console.error('Chyba při načítání detailu obchodu:', error)
    errorMessage.value = '❌ Nepodařilo se načíst obchod.'
  }
}

const deleteTrade = async () => {
  if (!trade.value) return

  if (!confirm('⚠️ Opravdu chcete odstranit tento obchod?')) return

  try {
    await api.delete(`/trades/${trade.value.id}`)
    alert('✅ Obchod úspěšně odstraněn!')
    router.push(`/trades/${userId}`)
  } catch (error) {
    console.error('Chyba při mazání obchodu:', error)
    errorMessage.value = '❌ Nepodařilo se odstranit obchod.'
  }
}

onMounted(fetchTrade)
</script>

<template>
  <div class="background-container">
    <div class="overlay"></div>
    <div class="trade-detail">
      <nav class="navbar">
        <router-link :to="`/lobby/${userId}`">🏠 Lobby</router-link>
        <router-link :to="`/profile/${userId}`">👤 Profil</router-link>
        <router-link :to="`/trades/${userId}`">📈 Obchody</router-link>
        <router-link to="/">🚪 Odhlásit</router-link> 
      </nav>

      <div v-if="trade">
        <h1>📜 Detail obchodu: {{ trade.coinType }}</h1>
        <p><strong>🆔 ID:</strong> {{ trade.id }}</p>
        <p><strong>📅 Datum:</strong> {{ new Date(trade.dateCreated).toLocaleDateString() }}</p>
        <p><strong>💰 Hodnota:</strong> {{ trade.worth.toLocaleString() }} Kč</p>
        <p><strong>👤 ID obchodníka:</strong> {{ trade.idTrader }}</p>

        <button class="delete" @click="deleteTrade">🗑️ Smazat obchod</button>
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
  background: url('https://source.unsplash.com/1600x900/?business,finance,technology') no-repeat center center/cover;
  display: flex;
  justify-content: center;
  align-items: center;
}

.overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  backdrop-filter: blur(8px);
}

.trade-detail {
  position: relative;
  background: rgba(255, 255, 255, 0.95);
  padding: 30px;
  border-radius: 12px;
  text-align: center;
  width: 80%;
  max-width: 800px;
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
}

.delete {
  background: #e74c3c;
  color: white;
  padding: 10px 15px;
  border-radius: 5px;
  cursor: pointer;
}

.delete:hover {
  background: #c0392b;
}

.error {
  color: red;
}
</style>

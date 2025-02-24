<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../api/axiosInstance'

const route = useRoute()
const router = useRouter()
const userId = route.params.id

const user = ref(JSON.parse(localStorage.getItem('user') || '{}'));

interface Trade {
  id: number
  coinType: string
  worth: number
  dateCreated: string
  idTrader: number
}

const trade = ref<Trade | null>(null)
const trades = ref<Trade[]>([]) 
const errorMessage = ref<string>('')

const newTrade = ref({
  coinType: '',
  worth: 0,
  dateCreated: new Date().toISOString(),
  idTrader: user.value.userId
})

const fetchTrades = async () => {
  try {
    const response = await api.get(`/trader/${userId}`)
    trades.value = response.data.trades || []
  } catch (error) {
    console.error('Chyba při načítání obchodů:', error)
    errorMessage.value = '❌ Nepodařilo se načíst obchody.'
  }
}

const addTrade = async () => {
  if (!newTrade.value.coinType || newTrade.value.worth <= 0) {
    errorMessage.value = '❌ Vyplňte všechny údaje správně.'
    return
  }

  try {
    const response = await api.post('/trade/add', newTrade.value)

    trades.value = response.data 
    newTrade.value = { coinType: '', worth: 0, dateCreated: new Date().toISOString(), idTrader: user.value.userId }
    errorMessage.value = ''
    alert('✅ Obchod úspěšně přidán!')
  } catch (error) {
    errorMessage.value = '❌ Nepodařilo se přidat obchod.'
  }
}

const deleteTrade = async (tradeId: number) => {
  if (!confirm('⚠️ Opravdu chcete odstranit tento obchod?')) return

  try {
    const response = await api.delete(`/trade/del/${tradeId}`)
    trades.value = response.data 
    alert('✅ Obchod úspěšně odstraněn!')
  } catch (error) {
    console.error('Chyba při mazání obchodu:', error)
    errorMessage.value = '❌ Nepodařilo se odstranit obchod.'
  }
}

onMounted(fetchTrades)
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

      <h1>📜 Moje obchody</h1>

      <div class="add-trade-form">
        <h2>➕ Přidat nový obchod</h2>
        <div class="form-group">
          <label for="coinType">Coin Type</label>
          <input type="text" id="coinType" v-model="newTrade.coinType" placeholder="Např. BTC" />
        </div>
        <div class="form-group">
          <label for="worth">Hodnota (Kč)</label>
          <input type="number" id="worth" v-model="newTrade.worth" placeholder="Např. 10000" />
        </div>
        <button @click="addTrade" class="primary-btn">💾 Přidat obchod</button>
      </div>

      <div v-if="trades.length > 0">
        <h2>📈 Seznam obchodů</h2>
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
                <button class="delete" @click="deleteTrade(trade.id)">🗑️ Smazat</button>
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

.add-trade-form {
  background: #f4f4f4;
  padding: 20px;
  border-radius: 10px;
  margin-bottom: 20px;
}

.form-group {
  margin-bottom: 10px;
  text-align: left;
}

input {
  width: 100%;
  padding: 10px;
  border-radius: 5px;
  border: 1px solid #ddd;
}

.primary-btn {
  background: #27ae60;
  color: white;
  padding: 10px 15px;
  border-radius: 5px;
  cursor: pointer;
}

.primary-btn:hover {
  background: #219150;
}

.delete {
  background: #e74c3c;
  color: white;
  padding: 5px 10px;
  border-radius: 5px;
  cursor: pointer;
}

.delete:hover {
  background: #c0392b;
}

.error {
  color: red;
  margin-top: 10px;
}
</style>

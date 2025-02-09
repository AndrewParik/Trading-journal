<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../api/axiosInstance'

const route = useRoute()
const router = useRouter()

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
    const response = await api.get(`api/trades/${route.params.id}`)
    trade.value = response.data
  } catch (error) {
    console.error('Chyba při načítání detailu obchodu:', error)
    errorMessage.value = '❌ Nepodařilo se načíst obchod.'
  }
}

const deleteTrade = async () => {
  if (!trade.value) return

  if (!confirm('⚠️ Opravdu chcete odstranit tento obchod?')) return

  try {
    await api.delete(`api/trades/${trade.value.id}`)
    alert('✅ Obchod úspěšně odstraněn!')
    router.push('/trades')
  } catch (error) {
    console.error('Chyba při mazání obchodu:', error)
    errorMessage.value = '❌ Nepodařilo se odstranit obchod.'
  }
}

const editTrade = async () => {
  if (!trade.value) return

  const newWorth = prompt('💰 Zadejte novou hodnotu obchodu (Kč):', trade.value.worth.toString())
  if (newWorth === null) return

  try {
    await api.put(`/trades/${trade.value.id}`, { ...trade.value, worth: Number(newWorth) })
    alert('✅ Obchod úspěšně aktualizován!')
    fetchTrade()
  } catch (error) {
    console.error('Chyba při úpravě obchodu:', error)
    errorMessage.value = '❌ Nepodařilo se upravit obchod.'
  }
}

const addTrade = async () => {
  const coinType = prompt('🔄 Zadejte typ coinu:')
  const worth = prompt('💰 Zadejte hodnotu (Kč):')

  if (!coinType || !worth) return

  try {
    await api.post('/trades', {
      coinType,
      worth: Number(worth),
      dateCreated: new Date().toISOString(),
      idTrader: trade.value?.idTrader || 1
    })
    alert('✅ Obchod úspěšně přidán!')
    fetchTrade()
  } catch (error) {
    console.error('Chyba při přidání obchodu:', error)
    errorMessage.value = '❌ Nepodařilo se přidat obchod.'
  }
}

onMounted(fetchTrade)
</script>

<template>
  <div class="trade-detail">
    <div v-if="trade">
      <h1>📜 Detail obchodu: {{ trade.coinType }}</h1>
      <p><strong>🆔 ID:</strong> {{ trade.id }}</p>
      <p><strong>📅 Datum:</strong> {{ new Date(trade.dateCreated).toLocaleDateString() }}</p>
      <p><strong>💰 Hodnota:</strong> {{ trade.worth.toLocaleString() }} Kč</p>
      <p><strong>👤 ID obchodníka:</strong> {{ trade.idTrader }}</p>

      <div class="button-group">
        <button class="add" @click="addTrade">+ Přidat obchod</button>
        <button class="edit" @click="editTrade">- Upravit obchod</button>
        <button class="delete" @click="deleteTrade">🗑️ Smazat obchod</button>
      </div>
    </div>
    
    <div v-else>
      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
    </div>
  </div>
</template>

<style scoped>
.trade-detail {
  max-width: 600px;
  margin: 50px auto;
  text-align: center;
  font-family: Arial, sans-serif;
  background: #1e1e1e;
  color: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
}

h1 {
  font-size: 24px;
  margin-bottom: 15px;
}

p {
  font-size: 18px;
  margin: 10px 0;
  color: #ddd;
}

.button-group {
  margin-top: 20px;
  display: flex;
  gap: 10px;
  justify-content: center;
}

button {
  padding: 10px 15px;
  font-size: 16px;
  font-weight: bold;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: 0.3s;
}

.add {
  background: #27ae60;
  color: white;
}

.add:hover {
  background: #219150;
}

.edit {
  background: #f39c12;
  color: white;
}

.edit:hover {
  background: #d98c0a;
}

.delete {
  background: #e74c3c;
  color: white;
}

.delete:hover {
  background: #c0392b;
}

 .error {
  color: red;
  font-size: 16px;
  font-weight: bold;
}
</style>

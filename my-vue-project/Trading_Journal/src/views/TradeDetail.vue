<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import api from '../api/axiosInstance'

const route = useRoute()

interface Trade {
  id: number
  coinType: string
  worth: number
  dateCreated: string
  idTrader: number
}

const trade = ref<Trade | null>(null)
const errorMessage = ref<string>('')

onMounted(async () => {
  try {
    const response = await api.get(`/trades/${route.params.id}`)
    trade.value = response.data
  } catch (error) {
    console.error('Chyba při načítání detailu obchodu:', error)
    errorMessage.value = '❌ Nepodařilo se načíst obchod.'
  }
})
</script>

<template>
  <div class="trade-detail">
    <div v-if="trade">
      <h1>📜 Detail obchodu: {{ trade.coinType }}</h1>
      <p><strong>🆔 ID:</strong> {{ trade.id }}</p>
      <p><strong>📅 Datum:</strong> {{ new Date(trade.dateCreated).toLocaleDateString() }}</p>
      <p><strong>💰 Hodnota:</strong> {{ trade.worth.toLocaleString() }} Kč</p>
      <p><strong>👤 ID obchodníka:</strong> {{ trade.idTrader }}</p>
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

.error {
  color: red;
  font-size: 16px;
  font-weight: bold;
}
</style>

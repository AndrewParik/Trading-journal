<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import api from '../api/axiosInstance'
const route = useRoute()
const trade = ref<{
  id: number,
  coinType: string,
  worth: number,
  dateCreated: string,
  idTrader: number
} | null>(null)

const errorMessage = ref('')

onMounted(async () => {
  try {
    const response = await api.get(`/trades/${route.params.id}`) 
    trade.value = response.data
  } catch (error) {
    console.error('Chyba při načítání detailu obchodu:', error)
    errorMessage.value = 'Nepodařilo se načíst obchod.'
  }
})
</script>

<template>
  <div class="trade-detail">
    <div v-if="trade">
      <h1>Detail obchodu: {{ trade.coinType }}</h1>
      <p><strong>ID:</strong> {{ trade.id }}</p>
      <p><strong>Datum:</strong> {{ new Date(trade.dateCreated).toLocaleDateString() }}</p>
      <p><strong>💰 Hodnota:</strong> {{ trade.worth.toLocaleString() }} Kč</p>
      <p><strong>👤 ID obchodníka:</strong> {{ trade.idTrader }}</p>
    </div>
    
    <div v-else>
      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
      <p v-else>Načítání...</p>
    </div>
  </div>
</template>

<style scoped>
.trade-detail {
  max-width: 600px;
  margin: 50px auto;
  text-align: center;
  font-family: Arial, sans-serif;
}

p {
  font-size: 18px;
  margin: 10px 0;
}

.error {
  color: red;
}
</style>

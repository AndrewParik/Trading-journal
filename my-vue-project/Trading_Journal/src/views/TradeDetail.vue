<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '../api/axiosInstance'

const router = useRouter()
const user = ref(JSON.parse(localStorage.getItem('user') || 'null'))
const errorMessage = ref(user.value ? '' : '❌ Nepodařilo se načíst uživatelská data.')
const tradeIdToDelete = ref<number | null>(null)


const newTrade = ref({
  coinType: '',
  worth: 0,
  dateCreated: new Date().toISOString(),
  idTrader: user.value?.id || 0
});

const addTradeAndUpdateStorage = async () => {
  
  if (!newTrade.value.coinType || newTrade.value.worth <= 0) {
    errorMessage.value = '❌ Vyplňte všechny údaje správně.';
    return;
  }

  try {
    console.log("📤 Odesílám požadavek na API:", newTrade.value);
    
    const response = await api.post('/trade/add', {
      coinType: newTrade.value.coinType,
      worth: newTrade.value.worth,
      dateCreated: newTrade.value.dateCreated,
      idTrader: newTrade.value.idTrader
    });

    console.log("✅ Obchod přidán:", response.data);

    console.log(`📤 Odesílám požadavek na API: /trader/0`);
    const updatedResponse = await api.get(`/trader/0`);
    
    console.log("✅ API odpovědělo:", updatedResponse.data);
    
    localStorage.removeItem('user');
    localStorage.setItem('user', JSON.stringify(updatedResponse.data));
    
    user.value = updatedResponse.data;

    newTrade.value = { coinType: '', worth: 0, dateCreated: '', idTrader: user.value.id };

    errorMessage.value = '';
    alert('✅ Obchod úspěšně přidán a data byla aktualizována!');
  } catch (error) {
    console.error('❌ Chyba:', error);
    errorMessage.value = '❌ Nepodařilo se přidat obchod nebo aktualizovat data.';
  }
};

const deleteTradeById = async (id: number) => {
  if (!confirm(`⚠️ Opravdu chcete odstranit obchod s ID ${id}?`)) return;

  try {
    console.log(`📤 Odesílám požadavek na API: /trade/del/${id}`);
    await api.delete(`/trade/del/${id}`);
    console.log("✅ Obchod odstraněn.");

    console.log(`📤 Odesílám požadavek na API: /trader/0`);
    const updatedResponse = await api.get(`/trader/0`);

    console.log("✅ API odpovědělo:", updatedResponse.data);

    localStorage.removeItem('user');
    localStorage.setItem('user', JSON.stringify(updatedResponse.data));

    user.value = updatedResponse.data;
    alert('✅ Obchod úspěšně odstraněn a data byla aktualizována!');
  } catch (error) {
    console.error('❌ Chyba při mazání obchodu nebo aktualizaci:', error);
    errorMessage.value = '❌ Nepodařilo se odstranit obchod nebo aktualizovat data.';
  }
};


</script>

<template>
  <div class="background-container">
    <div class="overlay"></div>
    <div class="trade-detail">
      <nav class="navbar">
        <router-link to="/">🏠 Lobby</router-link>
        <router-link to="/profile/0">👤 Profil</router-link>
        <router-link to="/trades/0">📈 Obchody</router-link>
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
        <div class="form-group">
          <label for="dateCreated">Datum</label>
          <input type="date" id="dateCreated" v-model="newTrade.dateCreated" />
        </div>
        <button @click="addTradeAndUpdateStorage" class="primary-btn">💾 Přidat obchod</button>
      </div>

      <div class="trades" v-if="user?.trades?.length && user.trades.length > 0">
        <h3>📈 Seznam obchodů</h3>
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
            <tr v-for="trade in user.trades" :key="trade.id">
              <td>{{ trade.id }}</td>
              <td>{{ trade.coinType }}</td>
              <td>{{ trade.worth.toLocaleString() }} Kč</td>
              <td>{{ new Date(trade.dateCreated).toLocaleDateString() }}</td>
              <td>
                <button @click="deleteTradeById(trade.id)" class="small-delete-btn">🗑️</button>
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

html, body {
  height: 100%;
  overflow-y: auto;
}

.background-container {
  position: relative; 
  width: 100%;
  min-height: 100vh; 
  background: url('https://source.unsplash.com/1600x900/?business,finance,technology') no-repeat center center/cover;
  display: flex;
  justify-content: center;
  align-items: flex-start;
  overflow-y: auto; 
  padding: 20px 0;
}
.refresh-btn {
  background: #3498db;
  color: white;
  padding: 10px 15px;
  border-radius: 5px;
  font-size: 16px;
  cursor: pointer;
  transition: 0.3s;
  margin-bottom: 20px;
}

.refresh-btn:hover {
  background: #2980b9;
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
  padding: 12px;
  text-align: center;
}

th {
  background: #01b7ff;
  color: white;
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
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.1);
  animation: fadeIn-7fc90d69 0.6s ease-in-out;
}



.navbar {
  display: flex;
  justify-content: space-around;
  background: #3498db;
  padding: 10px;
  border-radius: 5px;
  margin-bottom: 20px;
}
.small-delete-btn {
  background: #e74c3c;
  color: white;
  padding: 5px 10px;
  border-radius: 5px;
  cursor: pointer;
  border: none;
  transition: 0.3s;
}

.small-delete-btn:hover {
  background: #c0392b;
}

.navbar a {
  text-decoration: none;
  color: white;
  font-weight: bold;
  padding: 10px 15px;
  border-radius: 5px;
}

.add-trade-form, .delete-trade-form {
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

.delete-btn {
  background: #e74c3c;
  color: white;
  padding: 10px 15px;
  border-radius: 5px;
  cursor: pointer;
}

.delete-btn:hover {
  background: #c0392b;
}

.error {
  color: red;
  margin-top: 10px;
}
</style>

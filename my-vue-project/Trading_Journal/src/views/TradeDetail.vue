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

const updateLocalStorage = async () => {
  try {
    console.log(`📤 Odesílám požadavek na API: /trader/0`);
    const response = await api.get(`/trader/0`);
    
    console.log("✅ API odpovědělo:", response.data);
    
    localStorage.removeItem('user');
    localStorage.setItem('user', JSON.stringify(response.data));
    
    user.value = response.data;


    alert('✅ Data úspěšně aktualizována!');
  } catch (error) {
    console.error('❌ Chyba při aktualizaci dat:', error);
    errorMessage.value = '❌ Nepodařilo se aktualizovat data.';
  }
};

const addTrade = async () => {
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

    user.value.trades.push(response.data);
    localStorage.setItem('user', JSON.stringify(user.value));

    newTrade.value = { coinType: '', worth: 0, dateCreated: new Date().toISOString(), idTrader: user.value.id };

    errorMessage.value = '';
    alert('✅ Obchod úspěšně přidán!');
  } catch (error) {
    console.error('❌ Chyba při přidávání obchodu:', error);
    errorMessage.value = '❌ Nepodařilo se přidat obchod.';
  }
};




const deleteTrade = async () => {
  if (!tradeIdToDelete.value) {
    errorMessage.value = '❌ Zadejte ID obchodu k odstranění.';
    return;
  }

  if (!confirm(`⚠️ Opravdu chcete odstranit obchod s ID ${tradeIdToDelete.value}?`)) return;

  try {
    const response = await api.delete(`/trade/del/${tradeIdToDelete.value}`);
    console.log("✅ Obchod odstraněn:", response.data);

    localStorage.removeItem('user');
    localStorage.setItem('user', JSON.stringify(response.data));

    user.value.trades = response.data;
    tradeIdToDelete.value = null;


    alert('✅ Obchod úspěšně odstraněn!');
  } catch (error) {
    console.error('❌ Chyba při mazání obchodu:', error);
    errorMessage.value = '❌ Nepodařilo se odstranit obchod.';
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
        <button @click="addTrade" class="primary-btn">💾 Přidat obchod</button>
        <button @click="updateLocalStorage" class="refresh-btn">🔄 Aktualizovat data</button>
      </div>

      <div class="delete-trade-form">
        <h2>🗑️ Odstranit obchod</h2>
        <div class="form-group">
          <label for="tradeIdToDelete">ID obchodu</label>
          <input type="number" id="tradeIdToDelete" v-model="tradeIdToDelete" placeholder="Zadejte ID obchodu" />
        </div>
        <button @click="deleteTrade" class="delete-btn">🗑️ Smazat obchod</button>
        <button @click="updateLocalStorage" class="refresh-btn">🔄 Aktualizovat data</button>
      </div>

      <div class="trades" v-if="user?.trades?.length > 0">
        <h3>📈 Seznam obchodů</h3>
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

.trade-detail {
  position: relative;
  background: rgba(255, 255, 255, 0.95);
  padding: 30px;
  border-radius: 12px;
  text-align: center;
  width: 80%;
  max-width: 800px;
  animation: fadeIn-7fc90d69 0.6sease-in-out;
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

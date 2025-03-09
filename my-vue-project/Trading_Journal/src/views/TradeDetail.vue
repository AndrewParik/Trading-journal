<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api from '../api/axiosInstance'

const router = useRouter()
const user = ref(JSON.parse(localStorage.getItem('user') || 'null'))
const errorMessage = ref(user.value ? '' : '❌ Nepodařilo se načíst uživatelská data.')

const tradeToEdit = ref<{ id: number; coinType: string; worth: number; dateCreated: string } | null>(null)

const editTrade = (trade: any) => {
  tradeToEdit.value = { ...trade }; 
};

const saveTradeChanges = async () => {
  if (!tradeToEdit.value) return;

  console.log("📤 Posílám požadavek na API:", {
    CoinType: tradeToEdit.value.coinType,
    Worth: tradeToEdit.value.worth,
    DateCreated: tradeToEdit.value.dateCreated ? tradeToEdit.value.dateCreated.split("T")[0] : null
  });

  try {
    await api.put(`/trade/edit/${tradeToEdit.value.id}`, {
      CoinType: tradeToEdit.value.coinType,
      Worth: tradeToEdit.value.worth,
      DateCreated: tradeToEdit.value.dateCreated ? tradeToEdit.value.dateCreated.split("T")[0] : null
    });

    console.log("✅ API odpovědělo: úspěšně aktualizováno!");
    tradeToEdit.value = null;
    await refreshUserData();
  } catch (error) {
    console.error("❌ Chyba při editaci trejdu:", error);
    
    errorMessage.value = "❌ Nepodařilo se upravit obchod.";
  }
};






const deleteTradeById = async (id: number) => {
  if (!confirm(`⚠️ Opravdu chcete odstranit obchod s ID ${id}?`)) return

  try {
    console.log(`📤 Odesílám požadavek na API: /trade/del/${id}`)
    await api.delete(`/trade/del/${id}`)

    console.log("✅ Obchod odstraněn.")
    await refreshUserData()
  } catch (error) {
    console.error('❌ Chyba při mazání:', error)
    errorMessage.value = '❌ Nepodařilo se odstranit obchod.'
  }
}

const refreshUserData = async () => {
  try {
    console.log(`📤 Odesílám požadavek na API: /trader/${user.value.id}`)
    const response = await api.get(`/trader/${user.value.id}`)

    console.log("✅ API odpovědělo:", response.data)

    localStorage.setItem('user', JSON.stringify(response.data))
    user.value = response.data
  } catch (error) {
    console.error('❌ Chyba při načítání uživatelských dat:', error)
    errorMessage.value = '❌ Nepodařilo se načíst uživatelská data.'
  }
}

onMounted(refreshUserData)
</script>

<template>
  <div class="background-container">
    <div class="overlay"></div>
    <div class="trade-box">
      <nav class="navbar">
        <router-link to="/">🏠 Lobby</router-link>
        <router-link to="/profile/0">👤 Profil</router-link>
        <router-link to="/trades/0">📈 Obchody</router-link>
      </nav>

      <h1 class="trade-title">📜 Moje obchody</h1>

      <div class="trade-content" v-if="user?.trades?.length">
        <div class="trades-card">
          <h2>📈 Seznam obchodů</h2>
          <div class="table-container">
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
                  <td class="actions">
                    <button @click="editTrade(trade)" class="edit-btn">✏️</button>
                    <button @click="deleteTradeById(trade.id)" class="delete-btn">🗑️</button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        <div v-if="tradeToEdit" class="edit-card">
          <h2>📝 Editovat obchod</h2>
          <div class="form-group">
            <label for="editCoinType">Coin Type</label>
            <input type="text" id="editCoinType" v-model="tradeToEdit.coinType" />
          </div>
          <div class="form-group">
            <label for="editWorth">Hodnota</label>
            <input type="number" id="editWorth" v-model="tradeToEdit.worth" />
          </div>
          <div class="form-group">
            <label for="editDate">Datum</label>
            <input type="date" id="editDate" v-model="tradeToEdit.dateCreated" />
          </div>
          <button @click="saveTradeChanges" class="save-btn">💾 Uložit změny</button>
        </div>
      </div>

      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
    </div>
  </div>
</template>

<style scoped>
.background-container {
  position: absolute;
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

.trade-box {
  position: relative;
  background: rgba(255, 255, 255, 0.95);
  padding: 30px;
  border-radius: 12px;
  text-align: center;
  width: 90%;
  max-width: 850px;
  min-height: 500px;
  overflow: auto;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
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
  transition: 0.3s;
}

.navbar a:hover {
  background: rgba(255, 255, 255, 0.2);
}

.trade-title {
  font-size: 28px;
  margin-bottom: 15px;
  color: #333;
}

.trades-card, .edit-card {
  background: #f4f4f4;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
  width: 100%;
  max-width: 700px;
  text-align: left;
  margin-bottom: 20px;
}

.table-container {
  width: 100%;
  overflow-x: auto;
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
  padding: 12px;
  text-align: center;
  border-bottom: 1px solid #ddd;
}

th {
  background: #3498db;
  color: white;
  font-weight: bold;
}

td.actions {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 8px;
}

.edit-btn, .delete-btn {
  border: none;
  padding: 8px 12px;
  border-radius: 5px;
  cursor: pointer;
  margin: 2px;
  font-size: 16px;
}

.edit-btn {
  background: #f1c40f;
  color: black;
}

.edit-btn:hover {
  background: #d4ac0d;
}

.delete-btn {
  background: #e74c3c;
  color: white;
}

.delete-btn:hover {
  background: #c0392b;
}

.error {
  color: red;
  font-size: 14px;
  margin-top: 10px;
}
</style>

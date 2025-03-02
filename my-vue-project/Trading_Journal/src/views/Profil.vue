<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const user = ref(JSON.parse(localStorage.getItem('user') || 'null'))

const errorMessage = ref(user.value ? '' : '❌ Nepodařilo se načíst uživatelská data.')
</script>

<template>
  <div class="background-container">
    <div class="overlay"></div>
    <div class="profile-box">
      <nav class="navbar">
        <router-link to="/">🏠 Lobby</router-link>
        <router-link to="/trades/0">📈 Obchody</router-link>
        <router-link to="/edit/0">✏️ Upravit profil</router-link>
      </nav>

      <h1 class="profile-title">👤 Profil uživatele</h1>

      <div class="profile-content" v-if="user">
        <div class="info-card">
          <h2>📊 Informace o uživateli</h2>
          <p><strong>👤 Jméno:</strong> {{ user.firstName || 'Načítání...' }} {{ user.lastName || '' }}</p>
          <p><strong>💰 Portfolio:</strong> {{ user.portfolioValue ? user.portfolioValue.toLocaleString() : 'Načítání...' }} Kč</p>
        </div>

        <div class="trades-card" v-if="user.trades.length > 0">
          <h2>📈 Moje obchody</h2>
          <div class="table-container">
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
        </div>

        <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
      </div>
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

.profile-box {
  position: relative;
  background: rgba(255, 255, 255, 0.95);
  padding: 30px;
  border-radius: 12px;
  text-align: center;
  width: 80%;
  max-width: 800px;
  animation: fadeIn 0.6s ease-in-out;
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

.profile-title {
  font-size: 28px;
  margin-bottom: 10px;
  color: #333;
}

.profile-content {
  display: flex;
  flex-direction: column;
  align-items: end;
  gap: 20px;
  margin-top: -10px;
  width: 90%;
  max-width: 1000px;
}

.info-card, .trades-card {
  background: #f4f4f4;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
  width: 100%;
  max-width: 600px;
  text-align: left;
}

.table-container {
  width: 100%;
  display: flex;
  justify-content: center;
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
}

th {
  background: #3498db;
  color: white;
}

td {
  border-bottom: 1px solid #ddd;
}

.error {
  color: red;
  margin-top: 10px;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>

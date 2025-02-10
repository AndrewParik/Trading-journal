<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '../api/axiosInstance'

const firstName = ref('')
const lastName = ref('')
const password = ref('')
const errorMessage = ref('')
const router = useRouter()

const handleLogin = async () => {
  if (!firstName.value || !lastName.value || !password.value) {
    errorMessage.value = '❌ Prosím, vyplňte všechna pole.'
    return
  }

  const username = `${firstName.value.trim()} ${lastName.value.trim()}`

  console.log(`api/trader/login`);

  try {
    const response = await api.post(`api/trader/login`, {Username:username, Password:password.value})

    if (response.status === 200) {
      console.log('✅ Přihlášení úspěšné:', response.data)
      router.push('/lobby')
    }
  } catch (error: any) {
    if (error.response && error.response.status === 404) {
      errorMessage.value = error.response.data.message
    } else {
      errorMessage.value = '❌ Neočekávaná chyba při přihlášení.'
    }

    console.error('❌ Chyba při přihlášení:', error)
  }
}
</script>

<template>
  <div class="background-container">
    <div class="overlay"></div>
    <div class="login-box">
      <h1>👤 Přihlášení</h1>
      <p class="subtitle">Vítejte zpět! Přihlaste se ke svému účtu.</p>

      <div class="form-group">
        <label for="firstName">Jméno</label>
        <input type="text" id="firstName" v-model="firstName" placeholder="Zadejte jméno" />
      </div>

      <div class="form-group">
        <label for="lastName">Příjmení</label>
        <input type="text" id="lastName" v-model="lastName" placeholder="Zadejte příjmení" />
      </div>

      <div class="form-group">
        <label for="password">Heslo</label>
        <input type="password" id="password" v-model="password" placeholder="Zadejte heslo" />
      </div>

      <button @click="handleLogin">🔑 Přihlásit se</button>
      <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
    </div>
  </div>
</template>

<style scoped>
.body {
  background-color: black;
}
.background-container {
  position: fixed;
  background-color: black;
  top: 0;
  left: 0;
  width: 100%;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
}

.login-box {
  background: darkgray;
  padding: 30px;
  border-radius: 12px;
  text-align: center;
  width: 350px;
  box-shadow: 0px 4px 10px rgba(134, 133, 133, 0.2);
}

h1 {
  font-size: 28px;
  margin-bottom: 10px;
}

.form-group {
  margin-bottom: 15px;
}

input {
  width: 92.7%;
  padding: 12px;
  background-color: black;
  border: 1px solid black;
  color: white;
  border-radius: 8px;
  font-size: 16px;
}

button {
  width: 100%;
  padding: 12px;
  font-size: 16px;
  font-weight: bold;
  background: #3498db;
  color: white;
  border: none;
  border-radius: 8px;
}

.error-message {
  color: red;
  font-size: 14px;
}
</style>

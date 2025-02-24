<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '../api/axiosInstance'

const firstName = ref('')
const lastName = ref('')
const passWord = ref('')
const errorMessage = ref('')
const successMessage = ref('')
const router = useRouter()

const handleRegister = async () => {
  if (!firstName.value || !lastName.value || !passWord.value) {
    errorMessage.value = '❌ Prosím, vyplňte všechna pole.';
    return
  }

  try {
    const response = await api.post('/trader/new', {
      FirstName: firstName.value,
      LastName: lastName.value,
      PassWord: passWord.value
    })

    if (response.status === 201) {
      successMessage.value = '✅ Registrace úspěšná!'
      setTimeout(() => router.push('/'), 2000) 
    } else {
      errorMessage.value = '❌ Nepodařilo se zaregistrovat.'
    }
  } catch (error: any) {
    errorMessage.value = '❌ Chyba při registraci.'
    console.error(error)
  }
}
</script>

<template>
  <div class="background-container">
    <div class="overlay"></div>
    <div class="register-box">
      <h1>📝 Registrace</h1>
      <p class="subtitle">Vytvořte si nový účet</p>

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
        <input type="password" id="password" v-model="passWord" placeholder="Zadejte heslo" />
      </div>

      <button @click="handleRegister">🆕 Registrovat</button>
      <p v-if="successMessage" class="success-message">{{ successMessage }}</p>
      <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>

      <p class="switch">Máte již účet? <router-link to="/">🔑 Přihlásit se</router-link></p>
    </div>
  </div>
</template>

<style scoped>
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

.register-box {
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
  text-align: left;
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
  background: #27ae60;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
}

button:hover {
  background: #219150;
}

.switch {
  margin-top: 15px;
  color: white;
}

.switch a {
  color: #3498db;
  text-decoration: none;
  font-weight: bold;
}

.switch a:hover {
  text-decoration: underline;
}

.success-message {
  color: #2ecc71;
  font-size: 14px;
  margin-top: 10px;
}

.error-message {
  color: red;
  font-size: 14px;
  margin-top: 10px;
}
</style>

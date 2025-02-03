<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const username = ref('')
const password = ref('')
const errorMessage = ref('')
const router = useRouter()

const handleLogin = () => {
  if (username.value === 'admin' && password.value === 'password') {
    router.push('/lobby')
  } else {
    errorMessage.value = '❌ Nesprávné uživatelské jméno nebo heslo.'
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
        <label for="username">Uživatelské jméno</label>
        <input type="text" id="username" v-model="username" placeholder="Zadejte uživatelské jméno" />
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
.background-container {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100vh;
  background: url('/my-vue-project/Trading_Journal/public/mracek.jpg') no-repeat center center/cover;
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

.login-box {
  position: relative;
  background: rgba(255, 255, 255, 0.95);
  padding: 30px;
  border-radius: 12px;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
  text-align: center;
  width: 350px;
  animation: fadeIn 0.6s ease-in-out;
}

h1 {
  font-size: 28px;
  color: #333;
  margin-bottom: 10px;
}

.subtitle {
  color: #555;
  font-size: 14px;
  margin-bottom: 20px;
}

.form-group {
  margin-bottom: 15px;
  text-align: left;
}

label {
  display: block;
  font-size: 14px;
  color: #555;
  margin-bottom: 5px;
}

input {
  width: 100%;
  padding: 12px;
  border: 1px solid #ccc;
  border-radius: 8px;
  font-size: 16px;
  transition: 0.3s;
}

input:focus {
  border-color: #4facfe;
  box-shadow: 0px 0px 8px rgba(79, 172, 254, 0.5);
  outline: none;
}

button {
  width: 100%;
  padding: 12px;
  font-size: 16px;
  font-weight: bold;
  border: none;
  border-radius: 8px;
  background: linear-gradient(135deg, #4facfe, #00f2fe);
  color: white;
  cursor: pointer;
  transition: 0.3s;
}

button:hover {
  background: linear-gradient(135deg, #00f2fe, #4facfe);
  transform: scale(1.05);
}

.error-message {
  color: red;
  font-size: 14px;
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

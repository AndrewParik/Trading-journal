<script setup lang="ts">
import { ref } from 'vue'

const username = ref('');
const password = ref('');
const errorMessage = ref('');

const handleLogin = () => {
  // Mock přihlášení - zde byste mohli volat API
  if (username.value === 'admin' && password.value === 'password') {
    alert('Přihlášení úspěšné!');
  } else {
    errorMessage.value = 'Nesprávné uživatelské jméno nebo heslo.';
  }
};
</script>

<template>
  <div class="login-page">
    <h1>Přihlášení</h1>

    <div class="login-form">
      <div class="form-group">
        <label for="username">Uživatelské jméno</label>
        <input
          type="text"
          id="username"
          v-model="username"
          placeholder="Zadejte uživatelské jméno"
        />
      </div>

      <div class="form-group">
        <label for="password">Heslo</label>
        <input
          type="password"
          id="password"
          v-model="password"
          placeholder="Zadejte heslo"
        />
      </div>

      <button type="button" @click="handleLogin">Přihlásit se</button>

      <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
    </div>
  </div>
</template>

<style scoped>
.login-page {
  max-width: 400px;
  margin: 50px auto;
  text-align: center;
  font-family: Arial, sans-serif;
}

.login-form {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  text-align: left;
}

input {
  padding: 10px;
  font-size: 16px;
}

button {
  padding: 10px;
  font-size: 16px;
  cursor: pointer;
}

.error-message {
  color: red;
  font-size: 14px;
}
</style>

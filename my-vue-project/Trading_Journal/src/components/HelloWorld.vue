@ -1,41 +1,85 @@
<script setup lang="ts">
import { ref } from 'vue'

defineProps<{ msg: string }>()
const username = ref('');
const password = ref('');
const errorMessage = ref('');

const count = ref(0)
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
  <h1>{{ msg }}</h1>

  <div class="card">
    <button type="button" @click="count++">count is {{ count }}</button>
    <p>
      Edit
      <code>components/HelloWorld.vue</code> to test HMR
    </p>
  </div>
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

  
  
      </div>
  </div>
</template>

<style scoped>
.read-the-docs {
  color: #888;
.login-page {
  max-width: 400px;
  margin: 50px auto;
  text-align: center;
  font-family: Arial, sans-serif;
}}

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
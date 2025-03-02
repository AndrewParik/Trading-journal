<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import api from '../api/axiosInstance'

const router = useRouter()
const user = ref(JSON.parse(localStorage.getItem('user') || '{}'))
const successMessage = ref('')
const errorMessage = ref('')

const userId = ref(0)

const originalProfil = ref({
  firstName: user.value.firstName || '',
  lastName: user.value.lastName || '',
  passWord: user.value.passWord || ''
})

const profil = ref(Object.assign({}, originalProfil.value))

const isChanged = computed(() => {
  return (
    profil.value.firstName !== originalProfil.value.firstName ||
    profil.value.lastName !== originalProfil.value.lastName ||
    profil.value.passWord !== originalProfil.value.passWord
  )
})

const saveChanges = async () => {
  if (!isChanged.value) {
    errorMessage.value = '❌ Nebyla provedena žádná změna.';
    return;
  }
  localStorage.removeItem('user'); 

  try {
    const response = await api.put('trader/edit', {
      Id: userId.value,
      FirstName: profil.value.firstName,
      LastName: profil.value.lastName,
      PassWord: profil.value.passWord
    });

    if (response.status === 200 && response.data) {
      console.log("✅ Odpověď ze serveru:", response.data);

      localStorage.removeItem('user'); 
      localStorage.setItem('user', JSON.stringify(response.data));

      originalProfil.value = Object.assign({}, profil.value);
      successMessage.value = '✅ Údaje úspěšně aktualizovány!';
      errorMessage.value = '';
      localStorage.setItem('user', JSON.stringify(response.data));
    } else {
      errorMessage.value = '❌ Nepodařilo se aktualizovat údaje.';
    }
  } catch (error) {
    console.error('❌ Chyba při ukládání změn:', error);
    errorMessage.value = '❌ Chyba při ukládání změn.';
  }
};


</script>

<template>
  <div class="background-container">
    <div class="overlay"></div>
    <div class="content-box">
      <h1>📝 Úprava profilu</h1>
      <p class="subtitle">Změňte své přihlašovací údaje</p>

      <div class="form-group">
        <label for="firstName">Jméno</label>
        <input type="text" id="firstName" v-model="profil.firstName"/>
      </div>

      <div class="form-group">
        <label for="lastName">Příjmení</label>
        <input type="text" id="lastName" v-model="profil.lastName"/>
      </div>

      <div class="form-group">
        <label for="passWord">Heslo</label>
        <input type="password" id="passWord" v-model="profil.passWord" />
      </div>

      <button @click="saveChanges" class="primary-btn">💾 Uložit</button>
      <button @click="router.push('/')" class="secondary-btn">🔙 Zpět</button>

      <p v-if="successMessage" class="success-message">{{ successMessage }}</p>
      <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
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

.overlay {
  position: absolute;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
}

.content-box {
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
  border: none;
  border-radius: 8px;
  margin-top: 10px;
  cursor: pointer;
}

.primary-btn {
  background: #27ae60;
  color: white;
}

.primary-btn:hover {
  background: #219150;
}

.secondary-btn {
  background: #f39c12;
  color: white;
}

.secondary-btn:hover {
  background: #d98c0a;
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

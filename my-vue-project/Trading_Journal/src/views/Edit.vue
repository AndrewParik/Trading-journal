<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import api from '../api/axiosInstance'

const router = useRouter()
const route = useRoute()
const user1 = ref(JSON.parse(localStorage.getItem('user') || '{}'));

onMounted(() => {
  if (!user1.value.userId) {
    router.push('/');
  }
});


const profil = ref({
  firstName: 'PůvodníJméno',
  lastName: 'PůvodníPříjmení',
  pass_word: 'PůvodníHeslo'
});

const firstn = ref('');
const lastn = ref('');
const passw = ref('');

const action = () => {
  if (firstn.value !== user1.value.firstName) {
    user1.value.firstName = firstn.value;
  }
  if (lastn.value !== user1.value.lastName) {
    user1.value.lastName = lastn.value;
  }
  if (passw.value !== user1.value.pass_word) {
    user1.value.pass_word = passw.value;
  }
  return profil.value;
}

const responseedit = api.put('/trader/edit', {
      

    });
    
</script>

<template>
    <div class="background-container">
      <div class="overlay"></div>
      <div class="content-box">
        <h1>📌 Nadpis stránky</h1>
        <p class="subtitle">vyplňte nové přihlašovací údaje</p>
  
        <div class="form-group">
          <label for="input1">Jméno</label>
          <input ref="firstn" type="text" id="input1" v-model="profil.firstName" />
        </div>
  
        <div class="form-group">
          <label for="input2">Příjmeni</label>
          <input ref="lastn" type="text" id="input2" v-model="profil.lastName" />
        </div>
  
        <div class="form-group">
          <label for="input3">Heslo</label>
          <input ref="passw" type="text" id="input3" v-model="profil.pass_word"/>
        </div>
  
        <button @click="action()" class="primary-btn">💾 Uložit</button>
        <button class="secondary-btn">🔙 Zpět</button>
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
  </style>
  
import { ref } from 'vue'

const user = ref(JSON.parse(localStorage.getItem('user') || '{}'))

export const useUserStore = () => {
  const setUser = (userData: any) => {
    user.value = userData
    localStorage.setItem('user', JSON.stringify(userData))
  }

  const clearUser = () => {
    user.value = {}
    localStorage.removeItem('user')
  }

  return { user, setUser, clearUser }
}

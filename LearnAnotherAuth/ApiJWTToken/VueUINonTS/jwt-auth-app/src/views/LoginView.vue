<template>
  <div>
    <div v-if="auth.isAuthenticated">
      <p>Selamat datang! Token Anda: {{ auth.token }}</p>
      <button @click="logout">Logout</button>
    </div>
    <div v-else>
      <input v-model="username" type="text" placeholder="Username" />
      <input v-model="password" type="password" placeholder="Password" />
      <button @click="handleLogin">Login</button>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue'
import { useAuthStore } from '../stores/auth'
import { login, logout } from '../services/authService'

export default {
  setup() {
    const auth = useAuthStore()
    const username = ref('')
    const password = ref('')

    const handleLogin = async () => {
      try {
        await login({ name: username.value, password: password.value })
        // Redirect ke halaman lain setelah login berhasil
      } catch (error) {
        console.error('Login error:', error.message)
      }
    }

    const handleLogout = () => {
      logout()
      // Redirect ke halaman lain setelah logout berhasil
    }

    return {
      auth,
      username,
      password,
      handleLogin,
      handleLogout
    }
  }
}
</script>

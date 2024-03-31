<template>
  <div>
    <input v-model="username" type="text" placeholder="Username" />
    <input v-model="password" type="password" placeholder="Password" />
    <button @click="handleLogin">Login</button>
  </div>
</template>

<script>
import { ref } from 'vue'
import { login } from '../services/authService'

export default {
  setup() {
    const username = ref('')
    const password = ref('')

    const handleLogin = async () => {
      try {
        await login({ username: username.value, password: password.value })
        // Redirect ke halaman lain setelah login berhasil
      } catch (error) {
        console.error('Login error:', error.message)
      }
    }

    return {
      username,
      password,
      handleLogin
    }
  }
}
</script>

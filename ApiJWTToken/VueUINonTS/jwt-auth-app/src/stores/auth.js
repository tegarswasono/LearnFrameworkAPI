// store/auth.js

import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || null,
    isAuthenticated: localStorage.getItem('isAuthenticated') === 'true' || false
  }),
  actions: {
    login(token) {
      this.token = token
      localStorage.setItem('token', token)
      this.isAuthenticated = true
      localStorage.setItem('isAuthenticated', 'true') // Simpan isAuthenticated dalam local storage
    },
    logout() {
      this.token = null
      localStorage.removeItem('token')
      this.isAuthenticated = false
      localStorage.setItem('isAuthenticated', 'false') // Hapus isAuthenticated dari local storage saat logout
    }
  }
})

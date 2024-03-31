// store/auth.js

import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: null,
    isAuthenticated: false
  }),
  actions: {
    login(token: any) {
      this.token = token
      this.isAuthenticated = true
    },
    logout() {
      this.token = null
      this.isAuthenticated = false
    }
  }
})

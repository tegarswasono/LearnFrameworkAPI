// services/authService.js

import { useAuthStore } from '../stores/auth'

export async function login(credentials) {
  try {
    // Panggil API untuk autentikasi, misalnya dengan axios atau fetch
    // Di sini, Anda akan mendapatkan token JWT setelah login berhasil
    const response = await fetch('http://localhost:45841/api/users/authenticate', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(credentials)
    })

    if (!response.ok) {
      throw new Error('Login failed')
    }

    const data = await response.json()
    const token = data.token

    // Simpan token ke store menggunakan Pinia
    const authStore = useAuthStore()
    authStore.login(token)
  } catch (error) {
    console.error('Login error:', error.message)
    throw error
  }
}

export function logout() {
  // Hapus token dari store saat logout
  const authStore = useAuthStore()
  authStore.logout()
}

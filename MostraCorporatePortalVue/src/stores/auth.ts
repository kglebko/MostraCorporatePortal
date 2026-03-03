import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import authService from '@/services/authService'
import type { User } from 'oidc-client-ts'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const isLoading = ref(false)
  const error = ref<string | null>(null)
  const authReady = ref(false)

  const isAuthenticated = computed(() =>
    user.value !== null && !user.value.expired
  )

  const userClaims = computed(() => {
  if (!user.value || !user.value.profile) return null

  // Приводим full_name к строке
  const fullName = String(user.value.profile.full_name || '')
  const nameParts = fullName.trim().split(' ')

  const photoRaw = String(user.value.profile.photo || '')
  const photo = photoRaw.split(',')[0]


  return {
    sub: String(user.value.profile.sub || ''),
    name: String(user.value.profile.name || ''),
    email: String(user.value.profile.email || ''),
    firstName: nameParts[1] || '',
    lastName: nameParts[0] || '',
    fullName,
    photo
  }
})


  async function init() {
    if (authReady.value) return // 🔹 инициализация один раз

    isLoading.value = true
    error.value = null

    try {
      const loadedUser = await authService.getUser()
      user.value = loadedUser && !loadedUser.expired ? loadedUser : null

      // Подписки один раз
      authService.onUserLoaded((loadedUser) => {
        user.value = loadedUser
      })

      authService.onUserUnloaded(() => {
        user.value = null
      })

    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Ошибка инициализации авторизации'
      user.value = null
    } finally {
      isLoading.value = false
      authReady.value = true
    }
  }

  async function login() {
    isLoading.value = true
    error.value = null

    try {
      await authService.login()
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Ошибка входа'
      throw err
    } finally {
      isLoading.value = false
    }
  }

  async function logout() {
    isLoading.value = true
    error.value = null
    try {
      await authService.logout()
    } finally {
      user.value = null
      isLoading.value = false
    }
  }

  async function handleCallback() {
    isLoading.value = true
    error.value = null
    try {
      const callbackUser = await authService.handleCallback()
      user.value = callbackUser && !callbackUser.expired ? callbackUser : null
      return callbackUser
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Ошибка обработки callback'
      user.value = null
      return null
    } finally {
      isLoading.value = false
    }
  }

  return {
    user,
    isLoading,
    error,
    authReady,
    isAuthenticated,
    userClaims,
    init,
    login,
    logout,
    handleCallback
  }
})
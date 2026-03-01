import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import authService from '@/services/authService'
import type { User } from 'oidc-client-ts'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => user.value !== null && (user.value?.expired === false))

  const userClaims = computed(() => {
    if (!user.value) return null
    
    return {
      sub: user.value.profile.sub,
      name: user.value.profile.name,
      email: user.value.profile.email,
      firstName: user.value.profile.first_name,
      lastName: user.value.profile.last_name,
      fullName: user.value.profile.full_name,
      photo: user.value.profile.photo
    }
  })

  async function loadUser() {
    isLoading.value = true
    error.value = null
    try {
      user.value = await authService.getUser()
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Ошибка загрузки пользователя'
      user.value = null
    } finally {
      isLoading.value = false
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
      user.value = null
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Ошибка выхода'
      throw err
    } finally {
      isLoading.value = false
    }
  }

  async function handleCallback() {
    isLoading.value = true
    error.value = null
    try {
      user.value = await authService.handleCallback()
      return user.value
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Ошибка обработки callback'
      user.value = null
      return null
    } finally {
      isLoading.value = false
    }
  }
  function init() {
    loadUser()
  
    authService.onUserLoaded((loadedUser) => {
      user.value = loadedUser
    })

    authService.onUserUnloaded(() => {
      user.value = null
    })

    authService.onAccessTokenExpiring(() => {
      authService.getAccessToken()
    })

    authService.onAccessTokenExpired(() => {
      authService.getAccessToken()
    })
  }

  return {
    user,
    isLoading,
    error,
    isAuthenticated,
    userClaims,
    loadUser,
    login,
    logout,
    handleCallback,
    init
  }
})


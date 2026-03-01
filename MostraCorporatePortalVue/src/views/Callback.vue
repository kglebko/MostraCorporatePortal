<template>
  <div class="callback-container">
    <div v-if="loading" class="loading">
      <div class="spinner"></div>
      <p>Обработка авторизации...</p>
    </div>
    <div v-else-if="error" class="error">
      <p>Ошибка авторизации: {{ error }}</p>
      <button @click="retry" class="retry-btn">Повторить</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()
const loading = ref(true)
const error = ref<string | null>(null)

onMounted(async () => {
  try {
    await authStore.handleCallback()
    router.push('/')
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Неизвестная ошибка'
    loading.value = false
  }
})

function retry() {
  loading.value = true
  error.value = null
  authStore.handleCallback().then(() => {
    router.push('/')
  }).catch((err) => {
    error.value = err instanceof Error ? err.message : 'Неизвестная ошибка'
    loading.value = false
  })
}
</script>

<style scoped>
.callback-container {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  background: linear-gradient(135deg, var(--ligth-gray) 0%, var(--primary-orange) 100%);
}

.loading {
  text-align: center;
  color: white;
}

.spinner {
  border: 4px solid var(--ligth-gray);
  border-top: 4px solid white;
  border-radius: 50%;
  width: 50px;
  height: 50px;
  animation: spin 1s linear infinite;
  margin: 0 auto 20px;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.error {
  text-align: center;
  color: white;
  background: var(--white);
  padding: 20px;
  border-radius: 10px;
}

.retry-btn {
  margin-top: 15px;
  padding: 10px 20px;
  background: white;
  color: var(--primary-orange);
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-weight: 600;
}

.retry-btn:hover {
  background: #f0f0f0;
}
</style>


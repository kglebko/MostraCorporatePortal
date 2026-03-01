<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import authService from '@/services/authService'

const router = useRouter()

onMounted(async () => {
  await authService.handleLogoutCallback()
  router.replace('/')
})
</script>

<template>
  <div>Выход...</div>
</template>
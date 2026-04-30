<script setup lang="ts">
import { onMounted, onBeforeUnmount, watch } from 'vue'
import { useRoute } from 'vue-router'
import { useChatsStore } from '@/stores/chats'
import ChatSidebar from './ChatSidebar.vue'
import ChatWindow from './ChatWindow.vue'
import '@/assets/styles/chats.scss'

const route = useRoute()
const chatsStore = useChatsStore()

onMounted(async () => {
  chatsStore.isChatPageActive = true

  /*await chatsStore.bootstrap()*/

  const employeeId = route.query.employeeId
  if (typeof employeeId === 'string' && employeeId) {
    await chatsStore.openByEmployee(employeeId)
  }
})

onBeforeUnmount(() => {
  chatsStore.isChatPageActive = false
  chatsStore.selectedChat = null
})

watch(
  () => route.query.employeeId,
  async (employeeId) => {
    if (typeof employeeId === 'string' && employeeId) {
      await chatsStore.openByEmployee(employeeId)
    }
  }
)
</script>

<template>
  <section class="chats-page">
    <ChatSidebar />
    <ChatWindow />
  </section>
</template>

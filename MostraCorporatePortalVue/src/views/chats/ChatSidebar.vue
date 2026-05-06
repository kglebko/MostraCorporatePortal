<script setup lang="ts">
import { computed, watch, onMounted } from 'vue'
import { useChatsStore } from '@/stores/chats'

const chatsStore = useChatsStore()

const chats = computed(() => chatsStore.filteredChats)

function formatChatTime(dateString?: string | null) {
  if (!dateString) return ''

  const date = new Date(dateString)
  const now = new Date()

  const isToday =
    date.getDate() === now.getDate() &&
    date.getMonth() === now.getMonth() &&
    date.getFullYear() === now.getFullYear()

  const yesterday = new Date()
  yesterday.setDate(now.getDate() - 1)

  const isYesterday =
    date.getDate() === yesterday.getDate() &&
    date.getMonth() === yesterday.getMonth() &&
    date.getFullYear() === yesterday.getFullYear()

  if (isToday) {
    return date.toLocaleTimeString([], {
      hour: '2-digit',
      minute: '2-digit'
    })
  }

  if (isYesterday) {
    return 'Вчера'
  }

  return date.toLocaleDateString('ru-RU', {
    day: '2-digit',
    month: '2-digit'
  })
}

onMounted(async () => {
  await chatsStore.bootstrap()
})

watch(
  () => chatsStore.searchQuery,
  async () => {
    await chatsStore.loadChats()
    await chatsStore.loadEmployees(chatsStore.searchQuery)
  }
)
</script>

<template>
  <aside class="chat-sidebar">
    <div class="chat-sidebar__search search-bar">
      <img class="icon_search" src="@/assets/icons/search_icon.svg" />
      <input
        v-model="chatsStore.searchQuery"
        type="text"
        placeholder="Найти сотрудника или чат"
      />
    </div>

    <div class="chat-sidebar__list">
      <div
        v-if="chatsStore.searchQuery.trim() && chatsStore.employeeResults.length > 0"
        class="chat-search-group"
      >
        <p class="chat-search-group__title">Сотрудники</p>
        <button
          v-for="employee in chatsStore.employeeResults"
          :key="employee.userId"
          class="chat-list-item"
          @click="chatsStore.openByEmployee(employee.userId)"
        >
          <div class="chat-list-item__avatar">
            <img
              v-if="employee.photo"
              :src="`https://localhost:5078/images/collaborators_photo/${employee.photo}`"
              alt="avatar"
            />
            <span v-else>{{ employee.fullName.slice(0, 1).toUpperCase() }}</span>
          </div>
          <div class="chat-list-item__content">
            <p class="chat-list-item__name">{{ employee.fullName }}</p>
            <p class="chat-list-item__message">Начать личный диалог</p>
          </div>
        </button>
      </div>

      <div class="chat-search-group">
        <p v-if="chatsStore.searchQuery.trim()" class="chat-search-group__title">Чаты</p>
      <button
        v-for="chat in chats"
        :key="chat.id"
        class="chat-list-item"
        :class="{ active: chatsStore.selectedChat?.id === chat.id }"
        @click="chatsStore.selectChat(chat.id)"
      >
        <div class="chat-list-item__avatar">
          <img
            v-if="chat.avatarUrl"
            :src="`https://localhost:5078/images/collaborators_photo/${chat.avatarUrl}`"
            alt="avatar"
          />
          <span v-else>{{ chat.name.slice(0, 1).toUpperCase() }}</span>
        </div>

        <div class="chat-list-item__content">
          <div class="chat-list-item__top">
            <p class="chat-list-item__name">{{ chat.name }}</p>
            <span class="chat-list-item__time">
              {{ formatChatTime(chat.lastMessageAt) }}
            </span>
          </div>
          <div class="chat-list-item__bottom">
            <p class="chat-list-item__message">{{ chat.lastMessage || 'Нет сообщений' }}</p>
            <span v-if="chat.unreadCount > 0" class="chat-list-item__unread">{{ chat.unreadCount }}</span>
          </div>
        </div>
      </button>
      </div>
    </div>
  </aside>
</template>

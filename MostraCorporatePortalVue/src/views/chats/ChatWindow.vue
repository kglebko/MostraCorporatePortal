<script setup lang="ts">
import { computed, ref, watch, nextTick } from 'vue'
import { useRouter } from 'vue-router'
import { useChatsStore } from '@/stores/chats'
import { useAuthStore } from '@/stores/auth'

const chatsStore = useChatsStore()
const authStore = useAuthStore()
const router = useRouter()
const messageText = ref('')
const isInfoOpen = ref(false)
const messagesContainer = ref<HTMLElement | null>(null)

const groupedMessages = computed(() => {
  const groups = new Map<string, typeof chatsStore.messages>()
  chatsStore.messages.forEach((message) => {
    const key = new Date(message.createdAt).toLocaleDateString()
    const existing = groups.get(key) ?? []
    existing.push(message)
    groups.set(key, existing)
  })
  return Array.from(groups.entries())
})

const subtitle = computed(() => {
  if (!chatsStore.selectedChat) {
    return ''
  }
  if (chatsStore.selectedChat.type === 'Group' || chatsStore.selectedChat.type === 'Department') {
    return `${chatsStore.selectedChat.participants.length} участников`
  }
  return ''
})

const chatAvatarPhoto = computed(() => {
  if (!chatsStore.selectedChat) {
    return null
  }

  const authId = authStore.userClaims?.sub
  if (chatsStore.selectedChat.type === 'Direct') {
    return chatsStore.selectedChat.participants.find((p) => p.userId !== authId)?.photo ?? null
  }
  return null
})

const formattedParticipants = computed(() => {
  if (!chatsStore.selectedChat) {
    return []
  }
  return chatsStore.selectedChat.participants.map((x) => x.fullName)
})

const selectedChatListItem = computed(() => {
  if (!chatsStore.selectedChat) {
    return null
  }
  return chatsStore.chats.find((item) => item.id === chatsStore.selectedChat?.id) ?? null
})

const canOpenProfile = computed(() => chatsStore.selectedChat?.type === 'Direct')

const menuStats = computed(() => {
  if (!chatsStore.selectedChat) {
    return []
  }

  return [
    { label: 'Тип чата', value: chatsStore.selectedChat.type === 'Direct' ? 'Личный' : 'Групповой' },
    { label: 'Участников', value: String(chatsStore.selectedChat.participants.length) },
    { label: 'Сообщений', value: String(chatsStore.messages.length) }
    /*{ label: 'Непрочитанных', value: String(selectedChatListItem.value?.unreadCount ?? 0) }*/
  ]
})

const placeholderText = computed(() => {
  if (chatsStore.draftDirectEmployee) {
    return 'Нет сообщений'
  }
  return 'Выберите чат и начните общение'
})

async function onSend() {
  if (!messageText.value.trim()) {
    return
  }
  await chatsStore.sendMessage(messageText.value)
  messageText.value = ''
}

async function scrollToBottom() {
  await nextTick()

  if (!messagesContainer.value) {
    return
  }

  messagesContainer.value.scrollTop =
    messagesContainer.value.scrollHeight
}

watch(
  () => chatsStore.messages.length,
  async () => {
    await scrollToBottom()
  }
)

watch(
  () => chatsStore.selectedChat?.id,
  async () => {
    await scrollToBottom()
  }
)

function goToDirectParticipantProfile() {
  if (chatsStore.selectedChat?.type !== 'Direct') {
    return
  }

  const authId = authStore.userClaims?.sub
  const participant = chatsStore.selectedChat.participants.find((p) => p.userId !== authId)
  if (participant) {
    router.push({ name: 'employeeDetails', params: { id: participant.userId } })
  }
}
</script>

<template>
  <section class="chat-window">
    <div v-if="!chatsStore.selectedChat" class="chat-window__placeholder">
      <div class="chat-window__placeholder-icon">
        <img src="@/assets/icons/start_chat_icon.svg">
      </div>
      <p>{{ placeholderText }}</p>
    </div>

    <template v-else>
      <header class="chat-window__header">
        <button
          class="chat-window__header-title"
          :class="{ clickable: canOpenProfile }"
          :disabled="!canOpenProfile"
          @click="goToDirectParticipantProfile"
        >
          <div class="chat-window__avatar">
            <img
              v-if="chatAvatarPhoto"
              :src="`https://localhost:5078/images/collaborators_photo/${chatAvatarPhoto}`"
              alt="avatar"
            />
            <span v-else>{{ chatsStore.selectedChat.name.slice(0, 1).toUpperCase() }}</span>
          </div>
          <div>
            <h3>{{ chatsStore.selectedChat.name }}</h3>
            <p v-if="subtitle" class="chat-subtitle">{{ subtitle }}</p>
          </div>
        </button>
        <div class="chat-window__more-wrap">
          <button class="chat-window__more" @click="isInfoOpen = !isInfoOpen">⋯</button>
          <div v-if="isInfoOpen" class="chat-window__menu">
            <p class="chat-window__menu-title">О чате</p>
            <div class="chat-window__menu-stats">
              <div v-for="item in menuStats" :key="item.label" class="chat-window__menu-stat">
                <span>{{ item.label }}</span>
                <strong>{{ item.value }}</strong>
              </div>
            </div>
            <p class="chat-window__menu-title participants">Участники</p>
            <p v-for="name in formattedParticipants" :key="name" class="chat-window__menu-item">{{ name }}</p>
          </div>
        </div>
      </header>

      <div ref="messagesContainer" class="chat-window__messages">
        <p v-if="groupedMessages.length === 0" class="chat-window__empty">Нет сообщений</p>
        <template v-for="[date, items] in groupedMessages" :key="date">
          <div class="chat-window__date-divider">
            {{ items[0] ? new Date(items[0].createdAt).toLocaleDateString('ru-RU', { day: 'numeric', month: 'long', year: 'numeric' }) : '' }}
          </div>
          <div
            v-for="message in items"
            :key="message.id"
            class="chat-message"
            :class="{ own: message.isOwn }"
          >
            <div class="chat-message__bubble">
              <p v-if="chatsStore.selectedChat?.type !== 'Direct' && !message.isOwn"
                class="chat-message__author">
                {{ message.senderName }}
              </p>

              <p class="chat-message__text">
                {{ message.messageText }}
              </p>

              <div class="chat-message__meta">
                <span>
                  {{ new Date(message.createdAt).toLocaleTimeString([], {
                    hour: '2-digit',
                    minute: '2-digit'
                  }) }}
                </span>

                <span
                  v-if="message.isOwn"
                  class="chat-message__status">
                  {{ message.isReadByEveryone ? '✓✓' : '✓' }}
                </span>
              </div>

            </div>
          </div>
        </template>
      </div>

      <footer class="chat-window__input">
        <input
          v-model="messageText"
          type="text"
          placeholder="Введите сообщение"
          @keydown.enter.prevent="onSend"
        />
        <button @click="onSend">Отправить</button>
      </footer>
    </template>
  </section>
</template>

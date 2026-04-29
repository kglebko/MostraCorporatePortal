import { defineStore } from 'pinia'
import { computed, ref } from 'vue'
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { useAuthStore } from './auth'
import chatService from '@/services/chatService'
import collaboratorsService from '@/services/collaboratorsService'
import authService from '@/services/authService'
import type { ChatDetails, ChatListItem, ChatMessage, ChatParticipant } from '@/types/Chat'

export const useChatsStore = defineStore('chats', () => {
  const chats = ref<ChatListItem[]>([])
  const selectedChat = ref<ChatDetails | null>(null)
  const messages = ref<ChatMessage[]>([])
  const searchQuery = ref('')
  const loadingChats = ref(false)
  const loadingMessages = ref(false)
  const employeeResults = ref<ChatParticipant[]>([])
  const hubConnection = ref<HubConnection | null>(null)
  const draftDirectEmployee = ref<ChatParticipant | null>(null)
  const selectedByProfileEmployeeId = ref<string | null>(null)

  const filteredChats = computed(() => {
    if (!searchQuery.value.trim()) {
      return chats.value
    }

    const q = searchQuery.value.trim().toLowerCase()
    return chats.value.filter((x) => {
      return x.name.toLowerCase().includes(q) || (x.lastMessage ?? '').toLowerCase().includes(q)
    })
  })

  const totalUnreadCount = computed(() => {
    return chats.value.reduce((acc, item) => acc + item.unreadCount, 0)
  })

  async function loadChats() {
    loadingChats.value = true
    try {
      chats.value = await chatService.getChats(searchQuery.value)
    } finally {
      loadingChats.value = false
    }
  }

  async function loadEmployees(query: string) {
    if (!query.trim()) {
      employeeResults.value = []
      return
    }
    employeeResults.value = await chatService.searchEmployees(query)
  }

  async function selectChat(chatId: number) {
    draftDirectEmployee.value = null
    selectedChat.value = await chatService.getChat(chatId)
    await loadMessages(chatId, true)
  }

  async function loadMessages(chatId: number, shouldMarkRead = false) {
    loadingMessages.value = true
    try {
      const paged = await chatService.getMessages(chatId, 1, 50)
      messages.value = paged.items
      const lastMessageId = paged.items[paged.items.length - 1]?.id
      if (shouldMarkRead && lastMessageId) {
        await chatService.markAsRead(chatId, lastMessageId)
      }
    } finally {
      loadingMessages.value = false
    }
  }

  async function openDraftDirect(employeeId: string, fullName: string, photo?: string | null) {
    draftDirectEmployee.value = {
      userId: employeeId,
      fullName,
      photo: photo ?? null,
      isOnline: false
    }
    selectedChat.value = {
      id: 0,
      type: 'Direct',
      name: fullName,
      participants: [draftDirectEmployee.value],
      departmentId: null
    }
    messages.value = []
  }

  async function openByEmployee(employeeId: string) {
    selectedByProfileEmployeeId.value = employeeId
    selectedChat.value = null
    messages.value = []

    try {
      const existing = await chatService.getExistingDirectByEmployee(employeeId)
      if (existing) {
        draftDirectEmployee.value = null
        await loadChats()
        await selectChat(existing.id)
        return
      }

      const user = await collaboratorsService.getById(employeeId)
      await openDraftDirect(employeeId, user.fullName, user.photo)
    } catch (error) {
      console.error('Не удалось открыть чат по сотруднику', error)
    }
  }

  async function sendMessage(text: string) {
    const textValue = text.trim()
    if (!textValue) {
      return
    }

    try {
      let chatId = selectedChat.value?.id
      if (!chatId || chatId === 0) {
        const employeeId = draftDirectEmployee.value?.userId
        if (!employeeId) {
          return
        }
        const created = await chatService.getOrCreateDirectByEmployee(employeeId)
        draftDirectEmployee.value = null
        selectedChat.value = created
        chatId = created.id
      }

      const msg = await chatService.sendMessage(chatId, textValue)
      messages.value.push(msg)
      await loadChats()
    } catch (error) {
      console.error('Не удалось отправить сообщение', error)
    }
  }

  async function openOrCreateDirect(employeeId: string) {
    await openByEmployee(employeeId)
  }

  async function initRealtime() {
    if (hubConnection.value) {
      return
    }

    const connection = new HubConnectionBuilder()
      .withUrl('https://localhost:5078/hubs/chat', {
        withCredentials: true,
        accessTokenFactory: async () => (await authService.getAccessToken()) ?? ''
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build()

    connection.on('ConversationUpdated', async (conversationId: number) => {
      await loadChats()
      if (selectedChat.value?.id === conversationId) {
        await loadMessages(conversationId, true)
      }
    })

    await connection.start()
    hubConnection.value = connection
  }

  async function disposeRealtime() {
    if (hubConnection.value) {
      await hubConnection.value.stop()
      hubConnection.value = null
    }
  }

  async function bootstrap() {
    const authStore = useAuthStore()
    if (!authStore.isAuthenticated) {
      return
    }
    await loadChats()
    await initRealtime()
  }

  return {
    chats,
    selectedChat,
    messages,
    searchQuery,
    loadingChats,
    loadingMessages,
    employeeResults,
    draftDirectEmployee,
    selectedByProfileEmployeeId,
    filteredChats,
    totalUnreadCount,
    bootstrap,
    loadChats,
    loadEmployees,
    selectChat,
    sendMessage,
    openOrCreateDirect,
    openByEmployee,
    disposeRealtime
  }
})

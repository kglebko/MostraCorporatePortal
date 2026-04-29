import apiService from './apiService'
import type { ChatDetails, ChatListItem, ChatParticipant, ChatMessage, PagedMessages, MessageType } from '@/types/Chat'

class ChatService {
  async getChats(query?: string): Promise<ChatListItem[]> {
    const suffix = query ? `?query=${encodeURIComponent(query)}` : ''
    return apiService.get<ChatListItem[]>(`/chats${suffix}`)
  }

  async getChat(chatId: number): Promise<ChatDetails> {
    return apiService.get<ChatDetails>(`/chats/${chatId}`)
  }

  async getOrCreateDirectChat(employeeId: string): Promise<ChatDetails> {
    return apiService.post<ChatDetails>('/chats/direct', { employeeId })
  }

  async getOrCreateDirectByEmployee(employeeId: string): Promise<ChatDetails> {
    return apiService.get<ChatDetails>(`/chats/direct/by-employee/${employeeId}`)
  }

  async getExistingDirectByEmployee(employeeId: string): Promise<ChatDetails | null> {
    try {
      return await apiService.get<ChatDetails>(`/chats/direct/existing/${employeeId}`)
    } catch {
      return null
    }
  }

  async searchEmployees(query?: string): Promise<ChatParticipant[]> {
    const suffix = query ? `?query=${encodeURIComponent(query)}` : ''
    return apiService.get<ChatParticipant[]>(`/chats/employees/search${suffix}`)
  }

  async getMessages(chatId: number, page = 1, pageSize = 30): Promise<PagedMessages> {
    return apiService.get<PagedMessages>(`/chats/${chatId}/messages?page=${page}&pageSize=${pageSize}`)
  }

  async sendMessage(chatId: number, messageText: string, messageType: MessageType = 'Text'): Promise<ChatMessage> {
    return apiService.post<ChatMessage>(`/chats/${chatId}/messages`, { messageText, messageType })
  }

  async editMessage(chatId: number, messageId: number, messageText: string): Promise<ChatMessage> {
    return apiService.put<ChatMessage>(`/chats/${chatId}/messages/${messageId}`, { messageText })
  }

  async deleteMessage(chatId: number, messageId: number): Promise<void> {
    await apiService.delete<void>(`/chats/${chatId}/messages/${messageId}`)
  }

  async markAsRead(chatId: number, lastMessageId?: number): Promise<void> {
    const suffix = lastMessageId ? `?lastMessageId=${lastMessageId}` : ''
    await apiService.post<void>(`/chats/${chatId}/read${suffix}`)
  }
}

export const chatService = new ChatService()
export default chatService

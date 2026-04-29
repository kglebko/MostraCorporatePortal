export type ConversationType = 'Direct' | 'Group' | 'Department'
export type MessageType = 'Text' | 'System'

export interface ChatListItem {
  id: number
  type: ConversationType
  name: string
  avatarUrl?: string | null
  lastMessage?: string | null
  lastMessageAt?: string | null
  unreadCount: number
  isOnline: boolean
  lastMessageSender?: string | null
}

export interface ChatParticipant {
  userId: string
  fullName: string
  photo?: string | null
  isOnline: boolean
}

export interface ChatDetails {
  id: number
  type: ConversationType
  name: string
  departmentId?: number | null
  participants: ChatParticipant[]
}

export interface ChatMessage {
  id: number
  conversationId: number
  senderId: string
  senderName: string
  senderPhoto?: string | null
  messageText: string
  messageType: MessageType
  createdAt: string
  editedAt?: string | null
  isDeleted: boolean
  isOwn: boolean
  isReadByEveryone: boolean
}

export interface PagedMessages {
  page: number
  pageSize: number
  totalCount: number
  items: ChatMessage[]
}

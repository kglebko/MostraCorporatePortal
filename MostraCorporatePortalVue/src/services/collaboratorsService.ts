import apiService from './apiService'

export interface CollaboratorDto {
  id: number
  fullName: string
  birthDate: string
  position: string
  department: string
  departmentId: number
  workFormat?: string
  organization: string
  organizationId: number
  role: string
  login: string
  email: string
  mobilePhone?: string
  internalPhone?: string
  createdAt: string
  photo: string
}

export interface CreateCollaboratorDto {
  lastName: string
  firstName: string
  middleName?: string
  birthDate: string
  positionId: number
  departmentId: number
  workFormatId?: number
  organizationId: number
  roleId: number
  login: string
  email: string
  mobilePhone?: string
  internalPhone?: string
  photo?: string
}

class CollaboratorsService {
  // Получить всех сотрудников
  async getAll(): Promise<CollaboratorDto[]> {
    return apiService.get<CollaboratorDto[]>('/collaborators')
  }

  // Получить сотрудника по ID
  async getById(id: string): Promise<CollaboratorDto> {
    return apiService.get<CollaboratorDto>(`/collaborators/${id}`)
  }

  // Создать нового сотрудника
  async create(dto: CreateCollaboratorDto): Promise<CollaboratorDto> {
    return apiService.post<CollaboratorDto>('/collaborators', dto)
  }
}

export const collaboratorsService = new CollaboratorsService()
export default collaboratorsService


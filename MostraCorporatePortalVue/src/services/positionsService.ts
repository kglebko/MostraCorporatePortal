import apiService from './apiService'

export interface PositionDto {
  id: number
  name: string
}

class PositionsService {

  async getAll(): Promise<PositionDto[]> {
    return apiService.get<PositionDto[]>('/positions')
  }

}

export const positionsService = new PositionsService()
export default positionsService
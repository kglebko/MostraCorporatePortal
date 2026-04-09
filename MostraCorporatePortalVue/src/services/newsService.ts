import apiService from './apiService'

export interface NewsDto {
  id: number
  title: string
  description: string
  image?: string
  date: string
}

class NewsService {

  async getAll(): Promise<NewsDto[]> {
    return apiService.get<NewsDto[]>('/news')
  }

  async getLatest(count: number): Promise<NewsDto[]> {
    return apiService.get<NewsDto[]>(`/news/latest/${count}`)
  }

  async getById(id: number): Promise<NewsDto> {
    return apiService.get<NewsDto>(`/news/${id}`)
  }

}

export const newsService = new NewsService()
export default newsService
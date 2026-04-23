import apiService from './apiService'
import type { Event } from '@/types/Event'

class EventService {

 async getAll():Promise<Event[]>{
   return apiService.get<Event[]>('/events')
 }

 async getUpcoming(count: number): Promise<Event[]> {
    return apiService.get<Event[]>(`/events/upcoming/${count}`)
  }

 async getById(id:number):Promise<Event>{
   return apiService.get<Event>(`/events/${id}`)
 }

 async create(data:Partial<Event>){
   return apiService.post('/events', data)
 }

 async update(id:number,data:Partial<Event>){
   return apiService.put(`/events/${id}`, data)
 }

 async delete(id:number){
   return apiService.delete(`/events/${id}`)
 }

}

export default new EventService()
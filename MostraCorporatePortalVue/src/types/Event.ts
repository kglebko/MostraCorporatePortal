export interface Event {
  id:number
  title:string
  description?:string

  dateStart:string
  dateEnd?:string

  timeStart?:string
  timeEnd?:string

  location?:string
  organizer?:string

  visible:boolean

  isOnline:boolean
  meetingUrl?:string

  departmentId?:number
}
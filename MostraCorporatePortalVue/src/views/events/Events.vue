<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import DashboardCard from '@/components/DashboardCard.vue'
import eventService from '@/services/eventService'
import type { Event } from '@/types/Event'
import '@/assets/styles/events.scss'

const eventsData = ref<Event[]>([])

type ViewMode = 'month' | 'week'

const currentView = ref<ViewMode>('month')
const currentDate = ref(new Date())

const selectedEventId = ref<number | null>(null)

const searchQuery = ref('')
const searchDate = ref('')

onMounted(async () => {
  try {
    const data = await eventService.getAll()
    eventsData.value = data
  } catch (e) {
    console.error('Ошибка загрузки событий', e)
  }
})

const formatYMD = (date: Date): string => {
  const d = new Date(date)

  return `${d.getFullYear()}-${
    String(d.getMonth()+1).padStart(2,'0')
  }-${
    String(d.getDate()).padStart(2,'0')
  }`
}

const parseYMD = (str:string): Date => {
  return new Date(str)
}

const isSameDay = (
  a:Date,
  b:Date
):boolean => formatYMD(a) === formatYMD(b)

const formatDateRange = ( start:string, end?:string):string => {

  const realEnd = end || start
  const startDate = parseYMD(start)
  const endDate = parseYMD(realEnd)

  if(start === realEnd){
    return startDate.toLocaleDateString('ru-RU')
  }
  return `${startDate.toLocaleDateString('ru-RU')}
  — ${endDate.toLocaleDateString('ru-RU')}`
}

const toDay = (d: Date) => {
  const x = new Date(d)
  x.setHours(0,0,0,0)
  return x
}

const getEventsForDate = (date: Date): Event[] => {
  const day = toDay(date).getTime()

  return eventsData.value.filter(ev => {
    const start = toDay(new Date(ev.dateStart)).getTime()
    const end = toDay(new Date(ev.dateEnd || ev.dateStart)).getTime()

    return day >= start && day <= end
  })
}

const getWeeksInMonth = ( year:number, month:number):number => {

 const firstDay = new Date(year,month,1)

 const lastDay = new Date(year,month+1,0)

 const firstWeekday = (firstDay.getDay()+6)%7

 const daysInMonth = lastDay.getDate()

 return Math.ceil((firstWeekday+daysInMonth)/7)

}

interface CalendarDay{
 date:Date
 isOtherMonth:boolean
 events:Event[]
 isToday:boolean
}

const monthDays = computed<CalendarDay[]>(()=>{

 const year = currentDate.value.getFullYear()

 const month = currentDate.value.getMonth()

 const firstDay = new Date(year,month,1)

 const startWeekday = (firstDay.getDay()+6)%7

 const daysInMonth = new Date(year,month+1,0).getDate()

 const weeksCount = getWeeksInMonth(year,month)

 const totalCells = weeksCount*7

 const today = new Date()
 const days:CalendarDay[]=[]

 for(let i=0;i<startWeekday;i++){

   const prevDate =
     new Date( year, month, -startWeekday+i+1)
   days.push({
      date:prevDate,
      isOtherMonth:true,
      events:getEventsForDate(prevDate),
      isToday:isSameDay(prevDate,today)
   })
 }

 for(let d=1; d<=daysInMonth; d++){

   const curDate = new Date(year,month,d)

   days.push({
      date:curDate,
      isOtherMonth:false,
      events:getEventsForDate(curDate),
      isToday:isSameDay(curDate,today)
   })
 }

 let remain = totalCells-days.length

 for(let i=1;i<=remain;i++){

   const nextDate = new Date(year,month+1,i)
   days.push({
      date:nextDate,
      isOtherMonth:true,
      events:getEventsForDate(nextDate),
      isToday:isSameDay(nextDate,today)
   })
 }
 return days

})

const weekDays = computed<CalendarDay[]>(()=>{

 const base = new Date(currentDate.value)

 const startOfWeek = new Date(base)

 const dow =(startOfWeek.getDay()+6)%7

 startOfWeek.setDate(startOfWeek.getDate()-dow)

 const today = new Date()

 const days:CalendarDay[]=[]

 for(let i=0;i<7;i++){

   const d = new Date(startOfWeek)

   d.setDate(startOfWeek.getDate()+i)
   days.push({
      date:d,
      isOtherMonth:false,
      events:getEventsForDate(d),
      isToday:isSameDay(d,today)
   })
 }
 return days
})

const eventLocationLabel = computed(() => {
  if (!selectedEvent.value) return ''

  if (selectedEvent.value.isOnline) {
    return 'Онлайн'
  }

  return selectedEvent.value.location || ''
})

const currentDays =computed(()=>(
 currentView.value==='month'
 ? monthDays.value
 : weekDays.value
))

const isAnyFilterActive =computed(()=>(
 searchQuery.value.trim()!=='' ||
 searchDate.value!==''
))

const leftPanelTitle =computed(()=>( isAnyFilterActive.value
 ? 'Все события'
 : 'Ближайшие события'
))

const isPastEvent = (ev: Event): boolean => {

 const today = new Date()
 today.setHours(0,0,0,0)

 const end = parseYMD(ev.dateEnd || ev.dateStart)
 end.setHours(0,0,0,0)
 return end < today
}

const isTodayEvent = (ev: Event): boolean => {

 const today = new Date()
 today.setHours(0,0,0,0)

 const start = parseYMD(ev.dateStart)
 start.setHours(0,0,0,0)

 const end = parseYMD(ev.dateEnd || ev.dateStart)
 end.setHours(0,0,0,0)
 return (
   today >= start &&
   today <= end
 )
}

const isToday = (date: Date): boolean => {
  const today = new Date()
  today.setHours(0,0,0,0)

  const d = new Date(date)
  d.setHours(0,0,0,0)

  return today.getTime() === d.getTime()
}

const baseEvents = computed<Event[]>(()=>{

 if(isAnyFilterActive.value){
   return eventsData.value
 }

 const today = new Date()

 today.setHours(0,0,0,0)

 const future = new Date(today)

 future.setDate(today.getDate()+7)

 return eventsData.value.filter(ev=>{
   const startDate =parseYMD(ev.dateStart)
   const endDate = parseYMD( ev.dateEnd || ev.dateStart)
   return (
      endDate>=today && startDate<=future
   )
 })
})



const filteredEvents =
computed<Event[]>(()=>{

 let events = baseEvents.value
 const term = searchQuery.value.toLowerCase().trim()

 if(term){
   events = events.filter(
    ev=>
      ev.title
      .toLowerCase()
      .includes(term)
   )
 }

 if (searchDate.value) {

  const selectedDate = toDay(new Date(searchDate.value)).getTime()

  events = events.filter(ev => {

    const start = toDay(new Date(ev.dateStart)).getTime()

    const end = toDay(
      new Date(ev.dateEnd || ev.dateStart)
    ).getTime()

    return (
      selectedDate >= start &&
      selectedDate <= end
    )
  })
}

 return events.sort(
  (a,b)=>
   a.dateStart.localeCompare(
     b.dateStart
   )
 )
})

const selectedEvent =
computed<Event|undefined>(()=>
 eventsData.value.find(
   ev =>
    ev.id===selectedEventId.value
 )
)

const periodLabel =
computed(()=>{
 if(
   currentView.value==='month'
 ){
   return currentDate.value
   .toLocaleString( 'ru-RU',
      {
       month:'long',
       year:'numeric'
      }
   )
 }

 const start =
    new Date(currentDate.value)

 const dow =
   (start.getDay()+6)%7

 start.setDate(
   start.getDate()-dow
 )

 const end =
   new Date(start)

 end.setDate(
   start.getDate()+6
 )

 return `
 ${start.toLocaleDateString('ru-RU')}
 —
 ${end.toLocaleDateString('ru-RU')}
 `

})

const navigate = (delta:number)=>{
 if(
   currentView.value==='month'
 ){
   currentDate.value =
    new Date(
      currentDate.value.getFullYear(),
      currentDate.value.getMonth()+delta,
      1
    )
 }else{

   const d =
      new Date(
        currentDate.value
      )
   d.setDate(
    d.getDate()+delta*7
   )
   currentDate.value=d
 }
}

const changeView = (view:ViewMode)=>{
 currentView.value=view
}

const selectEvent = (id:number)=>{
 selectedEventId.value=id
}

const getRangeClass = (ev: Event, date: Date) => {
  const start = new Date(ev.dateStart)
  const end = new Date(ev.dateEnd || ev.dateStart)

  const d = new Date(date)

  start.setHours(0,0,0,0)
  end.setHours(0,0,0,0)
  d.setHours(0,0,0,0)

  if (start.getTime() === end.getTime()) return ''

  if (d.getTime() === start.getTime()) {
    return 'range-start'
  }

  if (d.getTime() === end.getTime()) {
    return 'range-end'
  }

  if (d.getTime() > start.getTime() && d.getTime() < end.getTime()) {
    return 'range-middle'
  }

  return ''
}

const formatShortTime = (time?: string): string => {
  if (!time) return ''
  return time.slice(0, 5)
}

const formatTimeRange = (ev: Event): string => {

 const start = formatShortTime(ev.timeStart)
 const end = formatShortTime(ev.timeEnd)

 if (!start) return ''
 if (end) {
   return `${start} — ${end}`
 }
 return start
}

const escapeHtml = (str:string):string => {

 if(!str) return ''

 return str.replace(
  /[&<>]/g,
  m=>{
   if(m==='&') return '&amp;'
   if(m==='<') return '&lt;'
   if(m==='>') return '&gt;'
   return m
  }
 )
}
</script>

<template>
  <div class="page-header">
    <h1>Календарь событий</h1>
    <div class="search-controls">
      <div class="search-bar">
        <input type="text" v-model="searchQuery" placeholder="Поиск по названию" />
        <img class="icon_search" src="@/assets/icons/search_icon.svg" />
      </div>
      <div class="date-filter">
        <input type="date" v-model="searchDate" />
        <button v-if="searchDate" class="clear-date" @click="searchDate = ''">✖</button>
      </div>
    </div>
  </div>

  <div class="events-container">
    <DashboardCard class="sticky-panel" :title="leftPanelTitle" icon="/icons/events_icon.svg">
      <div class="events-list">
        <div v-for="ev in filteredEvents" :key="ev.id"
             class="event-item"
             :class="{
               selected: selectedEventId === ev.id,
               'event-past': isPastEvent(ev)
             }"
             @click="selectEvent(ev.id)">
          <div class="event-title">
            <img src="@/assets/icons/list_marker_icon.svg">
            <span v-html="escapeHtml(ev.title)"></span>
          </div>
          <div class="event-date">
            {{ formatDateRange(ev.dateStart, ev.dateEnd) }}
            <span v-if="isTodayEvent(ev)" class="today-badge">Сегодня</span>
          </div>
        </div>
        <div v-if="filteredEvents.length === 0" class="empty-detail">
          {{ searchDate ? 'Нет событий на выбранную дату' : 'События не найдены' }}
        </div>
      </div>
    </DashboardCard>

    <DashboardCard class="calendar-panel">
      <div class="calendar-toolbar">
        <div class="view-buttons">
          <button class="view-btn" :class="{ active: currentView === 'month' }" @click="changeView('month')">Месяц</button>
          <button class="view-btn" :class="{ active: currentView === 'week' }" @click="changeView('week')">Неделя</button>
        </div>
        <div class="month-nav">
          <button class="icon-btn" @click="navigate(-1)">◀</button>
          <div class="month-title">{{ periodLabel }}</div>
          <button class="icon-btn" @click="navigate(1)">▶</button>
        </div>
      </div>
      <div class="calendar-viewport">
        <div class="calendar-grid">
          <div class="weekday" v-for="dayName in ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс']" :key="dayName">
            {{ dayName }}
          </div>
          <div v-for="day in currentDays" :key="formatYMD(day.date)"
               class="calendar-day"
               :class="{
                 'other-month': day.isOtherMonth,
                 'today': day.isToday
               }">
            <div class="day-number">{{ day.date.getDate() }}</div>
            <div v-for="ev in day.events.slice(0, 2)" :key="ev.id"
                 class="event-bar"
                 :class="[
                   getRangeClass(ev, day.date),
                   { 'event-past-calendar': isPastEvent(ev) },
                   { 'today-event': isToday(day.date) }
                 ]"
                 @click.stop="selectEvent(ev.id)"
                 v-html="escapeHtml(ev.title)"></div>
            <div v-if="day.events.length > 2" class="more-events" @click.stop="selectEvent(day.events[2]!.id)">
              +{{ day.events.length - 2 }} ещё
            </div>
          </div>
        </div>
      </div>
    </DashboardCard>

    <DashboardCard class="sticky-panel" title="Информация">
      <div v-if="selectedEvent" class="event-details">
        <div class="detail-field">
          <div class="detail-value" style=" color: var(--orange); font-weight: 600; font-size: 1.1rem; margin-bottom: 4px;" v-html="escapeHtml(selectedEvent.title)"></div>
        </div>
        <div class="detail-field" v-if="selectedEvent.description">
          <div class="detail-label">Описание</div>
          <div class="detail-value" v-html="escapeHtml(selectedEvent.description)"></div>
        </div>
        <div class="detail-field">
          <div class="detail-label">Дата проведения</div>
          <div class="detail-value">{{ formatDateRange(selectedEvent.dateStart, selectedEvent.dateEnd) }}</div>
        </div>
        <div class="detail-field" v-if="(selectedEvent.timeStart || selectedEvent.timeEnd)">
          <div class="detail-label">Время</div>
          <div class="detail-value">{{ formatTimeRange(selectedEvent) }}</div>
        </div>
        <div class="detail-field" v-if="selectedEvent.organizer">
          <div class="detail-label">Организатор</div>
          <div class="detail-value" v-html="escapeHtml(selectedEvent.organizer)"></div>
        </div>
        <div class="detail-field" v-if="selectedEvent">
          <div class="detail-label">Место проведения</div>
          <div class="detail-value">{{ eventLocationLabel }}</div>
        </div>
        <div class="detail-field" v-if="selectedEvent.isOnline && selectedEvent.meetingUrl">
          <div class="detail-label">Ссылка</div>
          <div class="detail-value"><a :href="selectedEvent.meetingUrl" target="_blank" style=" color: var(--orange);">Подключиться</a></div>
        </div>
      </div>
      <div v-else class="empty-detail">Выберите событие, чтобы увидеть подробности</div>
    </DashboardCard>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref, computed } from 'vue'
import DashboardCard from '@/components/DashboardCard.vue'
import '@/assets/styles/home.scss'
import newsService from '@/services/newsService'
import type { News } from '@/types/News'
import { getNewsImage, formatDate } from '@/utils/helpers'
import eventService from '@/services/eventService'
import type { Event } from '@/types/Event'

const eventsData = ref<Event[]>([])
const latestNews = ref<News[]>([])
const upcomingEvents = ref<Event[]>([])

const now = new Date()

const currentMonth = now.getMonth()
const currentYear = now.getFullYear()

const monthName = computed(() =>
  new Date().toLocaleString('ru-RU', { month: 'long' })
)

const daysInMonth = new Date(currentYear, currentMonth + 1, 0).getDate()

const firstDayOfMonth = new Date(currentYear, currentMonth, 1).getDay()
const startOffset = firstDayOfMonth === 0 ? 6 : firstDayOfMonth - 1

onMounted(async () => {
  latestNews.value = await newsService.getLatest(4)
  eventsData.value = await eventService.getAll()

  const today = new Date()
  today.setHours(0, 0, 0, 0)

  upcomingEvents.value = eventsData.value
    .filter(ev => {
      const end = new Date(ev.dateEnd || ev.dateStart)
      end.setHours(0, 0, 0, 0)
      return end >= today
    })
    .sort((a, b) =>
      new Date(a.dateStart).getTime() -
      new Date(b.dateStart).getTime()
    )
    .slice(0, 4)
})

const isToday = (day: number | null): boolean => {
  if (!day) return false

  const d = new Date(currentYear, currentMonth, day)
  const t = new Date()
  t.setHours(0, 0, 0, 0)
  d.setHours(0, 0, 0, 0)

  return d.getTime() === t.getTime()
}

const hasEvent = (day: number | null): boolean => {
  if (!day) return false

  const date = new Date(currentYear, currentMonth, day)
  date.setHours(0, 0, 0, 0)

  return eventsData.value.some(ev => {
    const start = new Date(ev.dateStart)
    const end = new Date(ev.dateEnd || ev.dateStart)

    start.setHours(0, 0, 0, 0)
    end.setHours(0, 0, 0, 0)

    return date >= start && date <= end
  })
}

const calendarDays = computed(() => {
  const days: (number | null)[] = []

  for (let i = 0; i < startOffset; i++) {
    days.push(null)
  }

  for (let day = 1; day <= daysInMonth; day++) {
    days.push(day)
  }

  return days
})

const formatEventDateRange = (start: string, end?: string): string => {
  const startDate = new Date(start)
  const endDate = new Date(end || start)
  
  const startDay = startDate.getDate()
  const endDay = endDate.getDate()
  const startMonth = startDate.getMonth()
  const endMonth = endDate.getMonth()
  const startYear = startDate.getFullYear()
  const endYear = endDate.getFullYear()
  
  const months = ['января', 'февраля', 'марта', 'апреля', 'мая', 'июня', 
                  'июля', 'августа', 'сентября', 'октября', 'ноября', 'декабря']
  
  if (!end || start === end) {
    return `${startDay} ${months[startMonth]} ${startYear}`
  }
  if (startMonth === endMonth && startYear === endYear) {
    return `${startDay}-${endDay} ${months[startMonth]} ${startYear}`
  }

  if (startYear === endYear) {
    return `${startDay} ${months[startMonth]} — ${endDay} ${months[endMonth]} ${startYear}`
  }
  return `${startDay} ${months[startMonth]} ${startYear} — ${endDay} ${months[endMonth]} ${endYear}`
}

const formatDateRange = (start: string, end?: string): string => {
  const s = new Date(start)
  const e = new Date(end || start)

  if (!end || start === end) {
    return s.toLocaleDateString('ru-RU')
  }

  return `${s.toLocaleDateString('ru-RU')} — ${e.toLocaleDateString('ru-RU')}`
}

const courses = [
  { title: 'Профессиональная этика', progress: 25 },
  { title: 'Введение в корпоративные процессы', progress: 75 },
  { title: 'Управление временем и приоритетами', progress: 50 }
]
</script>

<template>
  <div class="dashboard">
    
    <div class="dashboard-container">

      <div class="dashboard-columns" style='width: 75%;'>
        <div class="dashboard-row">
          <DashboardCard
              title="Корпоративная жизнь"
              icon="/icons/corporate_life_icon.svg"
              :link="{ name: 'newsList' }"
              linkText="Все новости"
              style='width: 70%;'
              >

            <div class="news-block" v-if="latestNews.length">

              <div class="main-post" v-if="latestNews[0]">
                  <img
                    class="news-photo"
                    :src="getNewsImage(latestNews[0].image)"/>

                  <div class="news-info">
                    <RouterLink :to="{ name:'newsItem', params:{ id: latestNews[0].id }}">
                      <p class="news-name">{{ latestNews[0].title }}</p>
                    </RouterLink>
                    <p class="news-date">
                      {{ formatDate(latestNews[0].date) }}
                    </p>
                  </div>
              </div>

              <div class="news-list">
                <div v-for="item in latestNews.slice(1)"
                  :key="item.id"
                  class="list-item"
                >
                  <RouterLink :to="{ name:'newsItem', params:{ id:item.id }}">
                      <p class="news-name">{{ item.title }}</p>
                  </RouterLink>
                  <p class="news-date">
                    {{ formatDate(item.date) }}
                  </p>
                </div>
              </div>

            </div>

          </DashboardCard>

          <DashboardCard
              title="Опросы"
              icon="/icons/polls_icon.svg"
              style='width: 30%;'
              >

              <div class="polls">
                <div class="poll">
                  <img class="marker-icon" src="../assets/icons/list_marker_icon.svg">
                  <div class="poll-info">
                    <a>Обратная связь по корпоративному порталу</a>
                    <p class="poll-status-0">Не завершен</p>
                  </div>
                </div>
  
                <div class="poll">
                  <img class="marker-icon" src="../assets/icons/list_marker_icon.svg">
                  <div class="poll-info">
                    <a>Удовлетворенность условиями работы</a>
                    <p class="poll-status-1">Завершен</p>
                  </div>
                </div>

                <div class="poll">
                  <img class="marker-icon" src="../assets/icons/list_marker_icon.svg">
                  <div class="poll-info">
                    <a>Опрос вовлеченности сотрудников</a>
                    <p class="poll-status-1">Завершен</p>
                  </div>
                </div>

              </div>

          </DashboardCard>
        </div>

        <div class="dashboard-row">
          <DashboardCard
              title="Дни рождения"
              icon="/icons/birthdays_icon.svg"
              style='width: 30%;'
              >

              <div class="birthdays">
                <p>Сегодня</p>
                <div class="birthdays-today">

                  <div class="birthday-item">
                    <div class="birthday-block">
                      <img class="birthday-photo" src="../assets/images/profile_photo.png">
                      <div class="birthday-age">27</div>
                    </div>
                    <a class="birthday-person">Соколова Наталья Андреевна</a>
                  </div>

                  <div class="birthday-item">
                    <div class="birthday-block">
                      <img class="birthday-photo" src="../assets/images/profile_photo.png">
                      <div class="birthday-age">32</div>
                    </div>
                    <a class="birthday-person">Михайлов Кирилл Денисович</a>
                  </div>

                  <div class="birthday-item">
                    <div class="birthday-block">
                      <img class="birthday-photo" src="../assets/images/profile_photo.png">
                      <div class="birthday-age">31</div>
                    </div>
                    <a class="birthday-person">Хмельницкая Софья Юрьевна</a>
                  </div>

                  <div class="birthday-item">
                    <div class="birthday-block">
                      <img class="birthday-photo" src="../assets/images/profile_photo.png">
                      <div class="birthday-age">22</div>
                    </div>
                    <a class="birthday-person">Петкун Диана Олеговна</a>
                  </div>

                </div>
              </div>

          </DashboardCard>

          <DashboardCard
              title="События"
              icon="/icons/events_icon.svg"
              link="/events"
              linkText="Все события"
              style='width: 70%;'
              >

              <div class="home-events">
                
                <div class="home-calendar">
                  <p class="home-calendar-month">
                    {{ monthName.charAt(0).toUpperCase() + monthName.slice(1) }}
                  </p>

                  <div class="home-calendar-weekdays">
                    <span>Пн</span>
                    <span>Вт</span>
                    <span>Ср</span>
                    <span>Чт</span>
                    <span>Пт</span>
                    <span>Сб</span>
                    <span>Вс</span>
                  </div>

                  <div class="home-calendar-grid">
                    <div class="home-calendar-day"
                      v-for="(day, index) in calendarDays"
                      :key="index"
                      :class="{
                        today: isToday(day),
                        'has-event': hasEvent(day)
                      }"
                    >
                      <span v-if="day">{{ day }}</span>

                      <div v-if="hasEvent(day)" class="home-event-dot"></div>
                    </div>
                  </div>
                </div>

                <div class="home-events-list">
                  <div v-for="ev in upcomingEvents" :key="ev.id" class="list-item">

                    <p class="home-event-date">
                      {{ formatEventDateRange(ev.dateStart, ev.dateEnd) }}
                    </p>

                    <p class="home-event-description">
                      {{ ev.title }}
                    </p>

                  </div>
                </div>

              </div>

          </DashboardCard>

        </div>
      </div>

      <div class="dashboard-columns" style='width: 25%;'>
        <DashboardCard
            title="Задачи"
            icon="/icons/tasks_icon.svg"
            link="/training"
            linkText="Обучение"
            style='height: 86%;'
            >

            <div class="tasks">
              <div class="tasks-block">
                <p>Тесты</p>

                <div class="tasks">

                  <div class="task">
                    <img src="/icons/completed_test_icon.svg">
                    <div class="task-title">
                      <a>Информационная безопасность</a>
                      <p class="task-date">до 28.02</p>
                    </div>
                  </div>

                  <div class="task">
                    <img src="/icons/completed_test_icon.svg">
                    <div class="task-title">
                      <a>Охрана труда</a>
                      <p class="task-date">до 01.03</p>
                    </div>
                  </div>

                  <div class="task">
                    <img src="/icons/completed_test_icon.svg">
                    <div class="task-title">
                      <a>Оценка навыков командного взаимодействия</a>
                      <p class="task-date">до 15.03</p>
                    </div>
                  </div>

                </div>
              </div>

              <div class="tasks-block">
                
                <div class="tasks-block">
                  <p>Курсы</p>

                  <div class="tasks">

                    <div class="task" v-for="course in courses" :key="course.title">
                      <img src="/icons/course_icon.svg">

                      <div class="task-title">
                        <a>{{ course.title }}</a>

                        <div class="progress">
                          <div
                            class="progress-fill"
                            :style="{ width: course.progress + '%' }"
                          ></div>

                          <span
                            class="progress-text"
                            :style="{ '--progress-width': course.progress + '%' }"
                          >
                            {{ course.progress }}%
                          </span>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            
            </div>
        </DashboardCard>

        <div class="game" style="height: 20%;">
            <div class="game-block">
              <div class="game-logo">
                <img src="/icons/logo_emblem_white.svg">
                <div class="game-title">
                  <p class="game-title-text">Скажи</p>
                  <p class="game-title-text">Мостра!</p>
                </div>
              </div>
              <a>Играть</a>
            </div>
        </div>

      </div>  
    </div>

  </div>
</template>
<script setup lang="ts">
import { onMounted, ref, computed } from 'vue'
import DashboardCard from '@/components/DashboardCard.vue'
import '@/assets/styles/home.scss'
import newsService from '@/services/newsService'
import type { News } from '@/types/News'
import { getNewsImage, formatDate } from '@/utils/helpers'
import eventService from '@/services/eventService'
import type { Event } from '@/types/Event'
import collaboratorsService, { type CollaboratorDto } from '@/services/collaboratorsService'
import { useChatsStore } from '@/stores/chats'
import BaseButton from '@/components/ui/BaseButton.vue'
import InverseButton from '@/components/ui/InverseButton.vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()

const eventsData = ref<Event[]>([])
const latestNews = ref<News[]>([])
const upcomingEvents = ref<Event[]>([])
const collaborators = ref<CollaboratorDto[]>([])
const chatsStore = useChatsStore()
const router = useRouter()
const birthdayModalOpen = ref(false)
const selectedBirthdayUser = ref<CollaboratorDto | null>(null)
const birthdayMessage = ref('С днем рождения!')
const stickerOptions = ['🎉', '🎂', '🎈', '🥳', '🌟', '🎁']
const selectedSticker = ref('🎉')

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
  collaborators.value = await collaboratorsService.getAll()

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

const birthdayPeople = computed(() => {
  const today = new Date()
  const month = today.getMonth()
  const day = today.getDate()
  const currentUserId = authStore.userClaims?.sub

  return collaborators.value
    .filter((item) => {
      if (!item.birthDate) return false

      //исключить самого себя
      if (String(item.id) === currentUserId) return false

      const birth = new Date(item.birthDate)
      return birth.getMonth() === month && birth.getDate() === day
    })
    .map((item) => {
      const birth = new Date(item.birthDate)
      const age = today.getFullYear() - birth.getFullYear()
      return { user: item, age }
    })
})

function openBirthdayModal(person: CollaboratorDto) {
  selectedBirthdayUser.value = person
  birthdayMessage.value = 'С днем рождения!'
  selectedSticker.value = '🎉'
  birthdayModalOpen.value = true
}

function closeBirthdayModal() {
  birthdayModalOpen.value = false
  selectedBirthdayUser.value = null
}

async function sendBirthdayMessage() {
  if (!selectedBirthdayUser.value) {
    return
  }
  const employeeId = String(selectedBirthdayUser.value.id)
  const text = `${selectedSticker.value} ${birthdayMessage.value.trim() || 'С днем рождения!'}`
  await chatsStore.openByEmployee(employeeId)
  await chatsStore.sendMessage(text)
  closeBirthdayModal()
  await router.push({
    name: 'chats',
    query: {
      employeeId,
      openAt: String(Date.now())
    }
  })
}

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
                  <div v-if="birthdayPeople.length === 0" class="birthday-empty">
                    Сегодня именинников нет
                  </div>
                  <div v-for="person in birthdayPeople" :key="person.user.id" class="birthday-item">
                    <div class="birthday-block">
                      <img
                        class="birthday-photo"
                        :src="person.user.photo
                          ? `https://localhost:5078/images/collaborators_photo/${person.user.photo}`
                          : 'https://localhost:5078/images/collaborators_photo/profile_photo.png'"
                      >
                      <div class="birthday-age">{{ person.age }}</div>
                    </div>
                    <button class="birthday-person" @click="openBirthdayModal(person.user)">
                      {{ person.user.fullName }}
                    </button>
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

  <Transition name="modal">
    <div v-if="birthdayModalOpen" class="birthday-modal-overlay" @click.self="closeBirthdayModal">
      <div class="birthday-modal">
        <h3>Поздравить коллегу</h3>
        <p v-if="selectedBirthdayUser">{{ selectedBirthdayUser.fullName }}</p>
        <div class="birthday-stickers">
          <button
            v-for="sticker in stickerOptions"
            :key="sticker"
            class="birthday-sticker"
            :class="{ active: selectedSticker === sticker }"
            @click="selectedSticker = sticker"
          >
            {{ sticker }}
          </button>
        </div>
        <textarea v-model="birthdayMessage" rows="4" />
        <div class="birthday-modal-actions">
          <InverseButton @click="closeBirthdayModal">Отмена</InverseButton>
          <BaseButton @click="sendBirthdayMessage">Отправить</BaseButton>
        </div>
      </div>
    </div>
  </Transition>

</template>


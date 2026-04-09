<script setup lang="ts">
import { onMounted, ref, computed } from 'vue'
import DashboardCard from '@/components/DashboardCard.vue'
import '@/assets/styles/home.scss'
import newsService from '@/services/newsService'
import type { News } from '@/types/News'
import { getNewsImage , formatDate} from '@/utils/helpers'

const latestNews = ref<News[]>([])

onMounted(async () => {
  latestNews.value = await newsService.getLatest(4)
})

const today = new Date()
const currentMonth = today.getMonth()
const currentYear = today.getFullYear()

const monthName = computed(() =>
  today.toLocaleString('ru-RU', { month: 'long' })
)

const daysInMonth = new Date(currentYear, currentMonth + 1, 0).getDate()

const firstDayOfMonth = new Date(currentYear, currentMonth, 1).getDay()

const startOffset = firstDayOfMonth === 0 ? 6 : firstDayOfMonth - 1

const events = [
  { date: 9, title: 'Вебинар' },
  { date: 17, title: 'Квиз' },
  { date: 18, title: 'Квиз' },
  { date: 19, title: 'Квиз' },
  { date: 27, title: 'Вебинар' }
]

const hasEvent = (day: number | null) => {
  if (!day) return false
  return events.some(e => e.date === day)
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
              style='width: 75%;'
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
              style='width: 25%;'
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
              style='width: 20%;'
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
              style='width: 80%;'
              >

              <div class="events">
                
                <div class="calendar">
                  <p class="calendar-month">
                    {{ monthName.charAt(0).toUpperCase() + monthName.slice(1) }}
                  </p>

                  <div class="calendar-weekdays">
                    <span>Пн</span>
                    <span>Вт</span>
                    <span>Ср</span>
                    <span>Чт</span>
                    <span>Пт</span>
                    <span>Сб</span>
                    <span>Вс</span>
                  </div>

                  <div class="calendar-grid">
                    <div
                      v-for="(day, index) in calendarDays"
                      :key="index"
                      class="calendar-day"
                      :class="{
                        today: day === today.getDate(),
                        'has-event': hasEvent(day)
                      }"
                    >
                      <span v-if="day">{{ day }}</span>
                      <div v-if="hasEvent(day)" class="event-dot"></div>
                    </div>
                  </div>
                </div>

                <div class="events-list">
                  <div class="list-item">
                    <p class="event-date">9 февраля</p>
                    <a class="event-description">Вебинар «Планы и приоритеты на 2026 год»</a>
                  </div>
                  <div class="list-item">
                    <p class="event-date">17-19 февраля</p>
                    <a class="event-description">Корпоративный онлайн-квиз для сотрудников всех подразделений</a>
                  </div>
                  <div class="list-item">
                    <p class="event-date">27 февраля</p>
                    <a class="event-description">Вебинар «Планы и приоритеты на 2026 год»</a>
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
            style='height: 80%;'
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

        <div class="game">
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
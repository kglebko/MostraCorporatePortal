<script setup lang="ts">
import { ref, onMounted } from 'vue'
import DashboardCard from '@/components/DashboardCard.vue'
import apiService from '@/services/apiService'
import type { Collaborator } from '@/types/Collaborator'

const user = ref<Collaborator | null>(null)
const loading = ref(true)

onMounted(async () => {
  try {
    user.value = await apiService.get<Collaborator>('/profile')
  } catch (error) {
    console.error(error)
  } finally {
    loading.value = false
  }
})
</script>

<template>
  <div v-if="loading">Загрузка...</div>

  <div v-else-if="user">
    <div class="page-title">
      <h1>{{ user.fullName }}</h1>
      <h2>{{ user.role }}</h2>
    </div>

    <div class="dashboard">
      <div class="dashboard-row">

        <DashboardCard title="Основные сведения" style="width: 74%;">
          <div class="content-block">

            <div class="avatar">
                <img :src="user.photo ? `https://localhost:5078/images/collaborators_photo/${user.photo}` : 'https://localhost:5078/images/collaborators_photo/profile_photo.png'" alt="Фото сотрудника">
            </div>
            <div class="user-information">

              <div class="about-user-block">
                <div class="info-row">
                  <p class="user-about">Должность:</p>
                  <p class="user-info">{{ user.position }}</p>
                </div>

                <div class="info-row">
                  <p class="user-about">Подразделение:</p>
                  <RouterLink
                    :to="{ name: 'departmentDetails', params: { id: user.departmentId } }"
                    class="user-info"
                  >
                    {{ user.department }}
                  </RouterLink>
                </div>

                <div class="info-row">
                  <p class="user-about">Организация:</p>
                  <RouterLink
                    :to="{ name: 'organizationDetails', params: { id: user.organizationId } }"
                    class="user-info"
                  >
                    {{ user.organization }}
                  </RouterLink>
                </div>

              </div>

              <div class="about-user-block">
                <div class="info-row">
                  <p class="user-about">Формат работы:</p>
                  <p class="user-info">{{ user.workFormat }}</p>
                </div>

                <div class="info-row">
                  <p class="user-about">Внутренний телефон:</p>
                  <p class="user-info">{{ user.internalPhone }}</p>
                </div>
              </div>

              <div class="about-user-block">
                <div class="info-row">
                  <p class="user-about">Логин:</p>
                  <p class="user-info">{{ user.username }}</p>
                </div>

                <div class="info-row">
                  <p class="user-about">E-mail:</p>
                  <p class="user-info">{{ user.email }}</p>
                </div>

                <div class="info-row">
                  <p class="user-about">Мобильный телефон:</p>
                  <p class="user-info">{{ user.mobilePhone }}</p>
                </div>
              </div>

              <div class="about-user-block">
                <div class="info-row">
                  <p class="user-about">Дата рождения:</p>
                  <p class="user-info">
                    {{ new Date(user.birthDate).toLocaleDateString() }}
                  </p>
                </div>
              </div>

            </div>
          </div>
        </DashboardCard>

        <DashboardCard
          title="Пройденные курсы и тесты"
          style="width: 26%;"
        >
          <div class="stats">
            <div id="completed-tests-count" class="stats-item">
              <p class="completed-count">2</p>
              <p class="completed-label">тесты</p>
            </div>
            <div id="completed-courses-count" class="stats-item">
              <p class="completed-count">3</p>
              <p class="completed-label">курсы</p>
            </div>
          </div>

          <div id="completed_tests" class="info-list">
            <div class="list_item">
              <img class="icon" src="/icons/completed_test_icon.svg" />
              <p>Охрана труда</p>
            </div>
            <div class="list_item">
              <img class="icon" src="/icons/completed_test_icon.svg" />
              <p>Информационная безопасность</p>
            </div>
          </div>

          <div id="completed_courses" class="info-list">
            <div class="list_item">
              <img class="icon" src="/icons/course_icon.svg" />
              <p>Адаптационный курс</p>
            </div>
            <div class="list_item">
              <img class="icon" src="/icons/course_icon.svg" />
              <p>Профессиональная этика</p>
            </div>
            <div class="list_item">
              <img class="icon" src="/icons/course_icon.svg" />
              <p>Введение в корпоративные процессы</p>
            </div>
          </div>
        </DashboardCard>

      </div>
    </div>
  </div>

</template>


<style scoped lang="scss">

.content-block {
  display: flex;
  gap: 60px;
  align-items: center;
}

.avatar {
  flex: 0 0 300px;
  aspect-ratio: 1;
  border-radius: 50%;
  overflow: hidden;
}

.avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
}

.user-information {
  flex: 1;  
  display: flex;
  flex-direction: column;
  gap: 40px;
}

.about-user-block {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

a{
  color: $orange;
}

.info-row {
  display: flex;
  gap: 2rem;
}

.user-about {
  width: 200px;
  color: $gray;
  font-size: 16px;
  font-weight: 400;
}

.user-info {
  flex: 1;
  padding: 0;
}

.info-list{
  display: flex;
  flex-direction: column;
  row-gap: 1rem;
}

.list_item{
  display: flex;
  gap: 12px;
  align-items: center;
}

.stats{
  display: flex;
  justify-content: center;
  column-gap: 2rem;
  margin: 0.2rem 0 0.4rem;
}

.stats-item{
  display: flex;
  flex-direction: column;
  align-items: center;
}

.completed-count{
  color: $orange;
  font-size: 24px;
  font-weight: 600;
}

.completed-label{
  font-weight: 400;
  font-size: 16px;
  color: $gray;
}
</style>


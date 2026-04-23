<script setup lang="ts">
import { computed, ref, onMounted, onBeforeUnmount } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import '@/assets/styles/header.scss'


const authStore = useAuthStore()
const router = useRouter()

const currentUser = computed(() => authStore.userClaims)
const isMenuOpen = ref(false)
const menuRef = ref<HTMLElement | null>(null)

const toggleMenu = () => {
  isMenuOpen.value = !isMenuOpen.value
}

const logout = async () => {
  isMenuOpen.value = false
  await authStore.logout()
  router.replace('/')
}

const handleClickOutside = (event: MouseEvent) => {
  if (menuRef.value && !menuRef.value.contains(event.target as Node)) {
    isMenuOpen.value = false
  }
}

onMounted(() => {
  document.addEventListener('click', handleClickOutside)
})

onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside)
})
</script>

<template>
  <header>
    <div class="header-container">
      <div>
        <RouterLink :to="{ name: 'home' }">
          <img alt="logo" class="logo" src="@/assets/icons/logo.svg" />
        </RouterLink>
      </div>
  
      <nav>
        <RouterLink :to="{ name: 'home' }">Главная</RouterLink>
        <RouterLink :to="{ name: 'corporateLife' }">Корпоративная жизнь</RouterLink>
        <RouterLink :to="{ name: 'events' }">События</RouterLink>
        <RouterLink :to="{ name: 'training' }">Обучение</RouterLink>
        <RouterLink :to="{ name: 'forNew' }">Новичку</RouterLink>
        <RouterLink :to="{ name: 'employees' }">Сотрудники</RouterLink>
      </nav>
  
      <div class="user_block" ref="menuRef" v-if="currentUser">
        <RouterLink 
          :to="{ name: 'employeeDetails', params: { id: currentUser.sub } }"
          class="user_info"
        >
          <p class="namelabel">{{ currentUser.lastName }} {{ currentUser.firstName }}</p>
          <img class="profile_photo" :src="currentUser?.photo 
              ? `https://localhost:5078/images/collaborators_photo/${currentUser.photo}` 
              : 'https://localhost:5078/images/collaborators_photo/profile_photo.png'"
            alt="Фото сотрудника"
          />
        </RouterLink>
  
        <button class="menu_btn" @click.stop="toggleMenu">
          <img src="/icons/menu_more_icon.svg" alt="menu" />
        </button>
  
        <div v-if="isMenuOpen" class="dropdown">
          <div class="dropdown-item">
            <img class="dropdown-icon" src="/icons/exit.svg">
            <button class="dropdown-text" @click="logout">Выйти</button>
          </div>
        </div>
      </div>
    </div>
  </header>
</template>
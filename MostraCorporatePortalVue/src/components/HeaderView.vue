<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { ref, onMounted, onBeforeUnmount } from 'vue'
import apiService from '@/services/apiService'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
const isMenuOpen = ref(false)
const menuRef = ref<HTMLElement | null>(null)

interface ServerUser {
  id: string
  fullName: string
  photo?: string
}

const currentUser = ref<{ id: string; firstName: string; lastName: string; photo: string } | null>(null)

const toggleMenu = () => {
  isMenuOpen.value = !isMenuOpen.value
}

const logout = async () => {
  isMenuOpen.value = false
  await authStore.logout()
}

const handleClickOutside = (event: MouseEvent) => {
  if (menuRef.value && !menuRef.value.contains(event.target as Node)) {
    isMenuOpen.value = false
  }
}

onMounted(async () => {
  document.addEventListener('click', handleClickOutside)

  try {
    const user = await apiService.get<ServerUser>('/profile')
    // Разбиваем fullName на фамилию и имя
    const nameParts = user.fullName.trim().split(' ')
    const lastName = nameParts[0] || ''
    const firstName = nameParts[1] || ''

    currentUser.value = {
      id: user.id,
      firstName,
      lastName,
      photo: user.photo
        ? `https://localhost:5078/images/collaborators_photo/${user.photo}`
        : new URL('../assets/images/profile_photo.png', import.meta.url).href
    }
  } catch (err) {
    console.error('Не удалось загрузить профиль:', err)
  }
})

onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside)
})
</script>

<template>
  <header>
    <div>
      <RouterLink :to="{ name: 'home' }">
        <img alt="logo" class="logo" src="@/assets/logo.svg" />
      </RouterLink>
    </div>

    <div>
      <nav>
        <RouterLink :to="{ name: 'home' }">Главная</RouterLink>
        <RouterLink :to="{ name: 'corporateLife' }">Корпоративная жизнь</RouterLink>
        <RouterLink :to="{ name: 'training' }">Обучение</RouterLink>
        <RouterLink :to="{ name: 'events' }">События</RouterLink>
        <RouterLink :to="{ name: 'forNew' }">Новичку</RouterLink>
        <RouterLink :to="{ name: 'employees' }">Сотрудники</RouterLink>
      </nav>
    </div>

    <div class="user_block" ref="menuRef" v-if="currentUser">
      <RouterLink 
        :to="{ name: 'employeeDetails', params: { id: currentUser.id } }"
        class="user_info"
      >
        <p class="namelabel">{{ currentUser.firstName }} {{ currentUser.lastName }}</p>
        <img class="profile_photo" :src="currentUser.photo" alt="Фото сотрудника"/>
      </RouterLink>

      <button class="menu_btn" @click.stop="toggleMenu">
        <img src="/icons/menu_more_icon.svg" alt="menu" />
      </button>

      <div v-if="isMenuOpen" class="dropdown">
        <button class="dropdown_item" @click="logout">
          Выйти
        </button>
      </div>
    </div>
  </header>
</template>

<style scoped>
header { 
  width: 100%;
  padding: 0 10rem;
  display: flex;
  height: 40px;
  align-items: center;
  justify-content: space-between;
}

.logo {
  display: block;
  height: 40px;
  transition: 0.4s ease-in-out;
}

.logo:hover {
  transform: translateY(-2px);
}

.profile_photo {
  height: 40px;
  width: 40px;
  object-fit: cover;
  border-radius: 50%;
}

nav {
  font-size: 15px;
  font-weight: 500;
  text-align: center;
  display: flex;
  gap: 40px;
}

.user_block {
  position: relative;
  display: flex;
  align-items: center;
  gap: 10px;
}

.user_info {
  display: flex;
  align-items: center;
  gap: 10px;
  text-decoration: none;
}

.namelabel {
  color: var(--blue-gray);
  font-size: 13px;
  font-weight: 400;
  max-width: 88px;
  overflow-wrap: break-word;
  min-width: 0;
}

.menu_btn {
  background: transparent;
  border: none;
  cursor: pointer;
  padding: 5px;
  display: flex;
  align-items: center;
}

.menu_btn img {
  width: 20px;
  height: 20px;
}

.menu_btn:hover {
  opacity: 0.7;
}

.dropdown {
  position: absolute;
  top: 50px;
  right: 0;
  background: var(--white);
  border-radius: 15px;
  box-shadow: 0 0 4px rgba(0, 0, 0, 0.15);
  min-width: 120px;
  z-index: 100;
}

.dropdown_item {
  width: 100%;
  padding: 12px 16px;
  background: none;
  border: none;
  text-align: left;
  cursor: pointer;
  font-family: 'TT Travels', sans-serif;
  font-weight: 500;
  font-size: 16px;
  color: var(--primary-orange);
}

.dropdown_item:hover {
  background: #fff4ec;
  border-radius: 15px;
}

nav a.router-link-exact-active {
  color: var(--primary-orange);
}
</style>
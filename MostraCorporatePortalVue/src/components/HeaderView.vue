<script setup lang="ts">
import { computed, ref, onMounted, onBeforeUnmount } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

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
  router.replace('/') // после выхода на главную
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
    <div>
      <RouterLink :to="{ name: 'home' }">
        <img alt="logo" class="logo" src="@/assets/logo.svg" />
      </RouterLink>
    </div>

    <nav>
      <RouterLink :to="{ name: 'home' }">Главная</RouterLink>
      <RouterLink :to="{ name: 'corporateLife' }">Корпоративная жизнь</RouterLink>
      <RouterLink :to="{ name: 'training' }">Обучение</RouterLink>
      <RouterLink :to="{ name: 'events' }">События</RouterLink>
      <RouterLink :to="{ name: 'forNew' }">Новичку</RouterLink>
      <RouterLink :to="{ name: 'employees' }">Сотрудники</RouterLink>
    </nav>

    <div class="user_block" ref="menuRef" v-if="currentUser">
      <RouterLink 
        :to="{ name: 'employeeDetails', params: { id: currentUser.sub } }"
        class="user_info"
      >
        <p class="namelabel">{{ currentUser.firstName }} {{ currentUser.lastName }}</p>
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

.dropdown-item {
  width: 100%;
  padding: 12px 16px;
  border-radius: 15px;
  display: flex;
  align-items: center;
  gap: 4px;
}

.dropdown-icon{
  height: 20px;
}

.dropdown-item:hover {
  background: #fff4ec;
}

.dropdown-text{
  background: none;
  border: none;
  text-align: left;
  cursor: pointer;
  font-family: 'TT Travels', sans-serif;
  font-weight: 500;
  font-size: 16px;
  color: var(--dark-blue);
}

nav a.router-link-exact-active {
  color: var(--primary-orange);
}
</style>
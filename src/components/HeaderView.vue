<script setup lang="ts">
import { RouterLink } from 'vue-router'

const currentUser = {
  id: 15,
  fullName: 'Константин Глебко',
  photo: new URL('../assets/images/profile_foto.png', import.meta.url).href
}
</script>


<template>
  <header>

    <div>
      <RouterLink :to="{ name: 'home'}">
        <img alt="logo" class="logo" src="@/assets/logo.svg" />
      </RouterLink>
    </div>

    <div>
      <nav>
        <RouterLink :to="{ name: 'home'}">Главная</RouterLink>
        <RouterLink :to="{ name: 'corporateLife'}">Корпоративная жизнь</RouterLink>
        <RouterLink :to="{ name: 'training'}">Обучение</RouterLink>
        <RouterLink :to="{ name: 'events'}">События</RouterLink>
        <RouterLink :to="{ name: 'forNew'}">Новичку</RouterLink>
        <RouterLink :to="{ name: 'employees'}">Сотрудники</RouterLink>
      </nav>
    </div>

    <RouterLink :to="{ name: 'employeeDetails', params: { id: currentUser.id } }" class="user_info">
      <p class="namelabel">{{ currentUser.fullName }}</p>
      <img class="profile_photo" :src="currentUser.photo" />
    </RouterLink>


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

.logo:hover{
  transform: translateY(-4px);
  transition: 0.4s ease-in-out;
}

.profile_photo{
  height: 40px;
  width: 40px;
  object-fit: cover;
  border-radius: 50px;
}

nav {
  font-size: 14px;
  font-weight: 500;
  text-align: center;
  display: flex;
  gap: 40px;
}

.user_info{
  display: flex;
  align-items: center;
  gap: 10px;
  text-decoration: none;
}

.namelabel {
  color: var(--blue-gray);
  font-size: 12px;
  font-weight: 400;

  max-width: 80px;
  overflow-wrap: break-word;

  flex: 0 1 auto;
  min-width: 0;
}


nav a {
  text-align: center;
}

nav a.router-link-exact-active {
  color: var(--primary-orange);
}

nav a.router-link-exact-active:hover {
  color: var(--primary-orange);
}


nav a:first-of-type {
  border: 0;
}

</style>

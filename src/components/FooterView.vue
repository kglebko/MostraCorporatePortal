<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>


<template>
  <footer class="footer">

      <RouterLink :to="{ name: 'home'}">
        <img alt="logo_white" class="logo" src="@/assets/logo_white.svg" />
      </RouterLink>

      <div class="socials">
        <a href="#"><img src="@/assets/icons/facebook_icon.svg" /></a>
        <a href="#"><img src="@/assets/icons/x_icon.svg" /></a>
        <a href="#"><img src="@/assets/icons/youtube_icon.svg" /></a>
        <a href="#"><img src="@/assets/icons/vk_icon.svg" /></a>
        <a href="#"><img src="@/assets/icons/linkedin_icon.svg" /></a>
        <a href="#"><img src="@/assets/icons/instagram_icon.svg" /></a>
        <a href="#"><img src="@/assets/icons/odnoklassniki_icon.svg" /></a>
      </div>

      <p class="copyright">© 2026 Учебный центр "МОНСТРА-ГРУПП"</p>

  </footer>
</template>


<style scoped>

footer {
  width: 100%;
  background-color: var(--blue-gray);
  padding: 12px 10rem;
  display: flex;
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

.socials {
  display: flex;
  gap: 16px;
  align-items: center;
}

.socials img {
  width: 32px;
  height: 32px;
  opacity: 0.8;
  transition: 0.4s ease-in-out;
  padding-top: 4px;
}

.socials img:hover {
  opacity: 1;
  transform: translateY(-2px);
}

.copyright{
  font-size: 10px;
  color: var(--white);
}

</style>

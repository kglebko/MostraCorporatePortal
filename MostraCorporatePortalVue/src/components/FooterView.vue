<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>


<template>
  <footer class="footer">

      <RouterLink :to="{ name: 'home'}">
        <img alt="logo_white" class="logo" src="/icons/logo_white.svg" />
      </RouterLink>

      <div class="socials">
        <a href="#"><img src="@/assets/icons/social/facebook_icon.svg" /></a>
        <a href="#"><img src="@/assets/icons/social/x_icon.svg" /></a>
        <a href="#"><img src="@/assets/icons/social/youtube_icon.svg" /></a>
        <a href="#"><img src="@/assets/icons/social/vk_icon.svg" /></a>
        <a href="#"><img src="@/assets/icons/social/linkedin_icon.svg" /></a>
        <a href="#"><img src="@/assets/icons/social/instagram_icon.svg" /></a>
        <a href="#"><img src="@/assets/icons/social/odnoklassniki_icon.svg" /></a>
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
  transform: translateY(-2px);
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
  transition: 0.4s ease-in-out;
}

.copyright{
  font-size: 10px;
  color: var(--white);
  font-weight: 400;
}

</style>

<script setup lang="ts">
import { RouterLink } from 'vue-router'
import '@/assets/styles/footer.scss'
</script>


<template>
  <footer class="footer">
    <div class="footer-container">
      <RouterLink :to="{ name: 'home'}">
        <img alt="logo_white" class="logo" src="@/assets/icons/logo_white.svg" />
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

      <p class="copyright">© 2026 Учебный центр "МОСТРА-ГРУПП"</p>
    </div>
  </footer>
</template>

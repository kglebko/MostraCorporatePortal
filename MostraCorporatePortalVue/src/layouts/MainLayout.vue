<script setup lang="ts">
import HeaderView from '@/components/HeaderView.vue'
import FooterView from '@/components/FooterView.vue'
</script>

<template>
  <HeaderView />
  <main id="main" class="page-container">
    <RouterView />
  </main>
  <FooterView />
</template>

<style scoped>
.page-container {
  padding: 40px 0 60px;
  min-height: calc(100vh - 120px);
  background-color: var(--white);
}
</style>

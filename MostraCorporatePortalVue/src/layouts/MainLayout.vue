<script setup lang="ts">
import HeaderView from '@/components/HeaderView.vue'
import FooterView from '@/components/FooterView.vue'
</script>

<template>
  <HeaderView />
  <main id="main">
    <RouterView />
  </main>
  <FooterView />
</template>

<style scoped>

</style>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import InverseButton from '@/components/ui/InverseButton.vue'
import BaseButton from '@/components/ui/BaseButton.vue'

const router = useRouter()

function goBack() {
  router.back()
}

function goHome() {
  router.push({ name: 'home' })
}
</script>

<template>
  <section class="not-found">
    <div class="not-found__code">
      <span class="orange">4</span>
      <span class="orange">0</span>
      <span class="gray">4</span>
    </div>
    <h1>Упс! Страницы, которую вы ищите не существует :(</h1>
    <div class="not-found__actions">
      <InverseButton @click="goBack">Вернуться назад</InverseButton>
      <BaseButton @click="goHome">На главную</BaseButton>
    </div>
  </section>
</template>

<style scoped lang="scss">
.not-found {
  min-height: calc(100vh - 200px);
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 28px;
  text-align: center;
}

.not-found__code {
  display: flex;
  align-items: flex-end;
  font-size: 180px;
  font-weight: 700;
  line-height: 1;
}

.orange {
  color: $orange;
}

.gray {
  color: $gray;
  transform: rotate(90deg) translateX(8px);
  transform-origin: center;
}

.not-found h1 {
  max-width: 620px;
}

.not-found__actions {
  display: flex;
  gap: 16px;
}
</style>

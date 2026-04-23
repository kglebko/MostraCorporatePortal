<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { computed } from 'vue'

const props = defineProps<{
  title?: string
  icon?: string
  link?: string | object
  linkText?: string
}>()

const hasHeader = computed(() => {
  return props.title || props.icon || props.link
})
</script>

<template>
  <div class="dashboard-item">

    <div v-if="hasHeader" class="dashboard-header">
      <div class="dashboard-label">
        <img v-if="icon" :src="icon" class="dashboard-icon" alt="icon" />
        <p v-if="title" class="dashboard-title">{{ title }}</p>
      </div>

      <RouterLink
        v-if="link"
        :to="link"
        class="dashboard-link"
      >
        {{ linkText ?? 'Подробнее' }}
      </RouterLink>
    </div>

    <slot />

  </div>
</template>


<style scoped lang="scss">

.dashboard-item{
  display: flex;
  min-width: 200px;
  flex-direction: column;
  background: white;
  border-radius: $border-radius;
  border: 1px solid $light-gray;
  box-shadow: 0 0 6px rgba(0, 0, 0, 0.15);
  padding: 32px 40px 40px;
  gap: 1rem;
  overflow-y: auto;
}

.dashboard-header{
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.6rem;
}

.dashboard-label{
  display: flex;
  align-items: center; 
  gap: 12px;
}

.dashboard-icon{
  height: 24px;
  width: 24px;
  object-fit: contain;
}

.dashboard-title{
  font-size: 18px;
  font-weight: 600;
  margin: 0;
}

.dashboard-link {
  display: inline-flex;
  align-items: center;
  gap: 2px;

  font-size: $bs;
  color: $orange;
  text-decoration: none;
  transition: 0.4s ease-out;

  :hover{
    transform: translateY(-2px);
  }

  &::after {
    content: '';
    width: 12px;
    height: 12px;
    background: url('@/assets/icons/arrow_r.svg') no-repeat center;
    background-size: contain;
  }

}

.dashboard-link:hover{
  transform: translateY(-2px);
}

</style>

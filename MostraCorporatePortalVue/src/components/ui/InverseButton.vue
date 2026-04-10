<script setup lang="ts">
import { ref } from 'vue'

const props = defineProps<{
  type?: 'button' | 'submit'
  size?: 'sm' | 'md' | 'lg'
  iconPosition?: 'left' | 'right'
  icon?: string
  iconActive?: string
  active?: boolean
}>()

const emit = defineEmits(['click'])

const isHovered = ref(false)

const sizes = {
  sm: { fontSize: '14px', iconSize: '16px', padding: '10px 20px' },
  md: { fontSize: '16px', iconSize: '18px', padding: '12px 24px' },
  lg: { fontSize: '20px', iconSize: '20px', padding: '14px 28px' }
}

const currentSize = sizes[props.size ?? 'md']
const iconPos = props.iconPosition ?? 'right'
</script>

<template>
  <button
    class="btn"
    :class="{ active: props.active }"
    :type="type || 'button'"
    @click="emit('click')"
    @mouseenter="isHovered = true"
    @mouseleave="isHovered = false"
    :style="{
      fontSize: currentSize.fontSize,
      padding: currentSize.padding
    }"
  >

    <img
      v-if="icon && iconPos === 'left'"
      class="btn-icon"
      :src="props.active && iconActive ? iconActive : icon"
      :style="{ width: currentSize.iconSize, height: currentSize.iconSize }"
    />

    <span class="btn-content">
      <slot />
    </span>

    <img
      v-if="icon && iconPos === 'right'"
      class="btn-icon"
      :src="props.active && iconActive ? iconActive : icon"
      :style="{ width: currentSize.iconSize, height: currentSize.iconSize }"
    />
  </button>
</template>

<style scoped lang="scss">
.btn {
  background: $white;
  color: $orange;
  border: solid 1px $orange;
  border-radius: 50px;
  cursor: pointer;
  font-family: 'TT Travels';
  transition: 0.4s ease-out;

  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  
}

.btn.active {
    background: $orange;
    color: white;
}

.btn:hover{
    transform: translateY(-2px);
}

.btn-content {
  display: flex;
  align-items: center;
}

.btn-icon {
  display: flex;
  align-items: center;
  justify-content: center;

  & > * {
    width: 100%;
    height: 100%;
  }
}

.btn-icon {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;

  .icon {
    position: absolute;
    width: 100%;
    height: 100%;
    transition: opacity 0.4s ease-out;
  }

  .default {
    opacity: 1;
  }

  .hover {
    opacity: 0;
  }
}

.btn:hover .default {
  opacity: 0;
}

.btn:hover .hover {
  opacity: 1;
}

</style>
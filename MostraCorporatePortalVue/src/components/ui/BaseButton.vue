<script setup lang="ts">
const props = defineProps<{
  type?: 'button' | 'submit'
  size?: 'sm' | 'md' | 'lg'
  iconPosition?: 'left' | 'right'
}>()

const emit = defineEmits(['click'])

const sizes = {
  sm: { fontSize: '14px', iconSize: '16px', padding: '12px 24px' },
  md: { fontSize: '16px', iconSize: '18px', padding: '12px 24px' },
  lg: { fontSize: '20px', iconSize: '20px', padding: '12px 24px' }
}

const currentSize = sizes[props.size || 'md']
const iconPos = props.iconPosition || 'right'
</script>

<template>
  <button
    class="btn"
    :type="type || 'button'"
    @click="emit('click')"
    :style="{
      fontSize: currentSize.fontSize,
      padding: currentSize.padding
    }"
  >
    <span
      v-if="$slots.icon && iconPos === 'left'"
      class="btn-icon"
      :style="{ width: currentSize.iconSize, height: currentSize.iconSize }"
    >
      <slot name="icon" />
    </span>

    <span class="btn-content">
      <slot />
    </span>

    <span
      v-if="$slots.icon && iconPos === 'right'"
      class="btn-icon"
      :style="{ width: currentSize.iconSize, height: currentSize.iconSize }"
    >
      <slot name="icon" />
    </span>
  </button>
</template>

<style scoped lang="scss">
.btn {
  background: $orange;
  height: fit-content;
  color: white;
  border: none;
  border-radius: 50px;
  cursor: pointer;
  font-family: 'TT Travels';
  transition: 0.4s ease-out;

  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 10px;

  &:hover {
    transform: translateY(-2px);
  }
}

.btn-content {
  display: flex;
  align-items: center;
}

.btn-icon {
  display: flex;
  align-items: center;

  & > * {
    width: 100%;
    height: 100%;
  }
}
</style>
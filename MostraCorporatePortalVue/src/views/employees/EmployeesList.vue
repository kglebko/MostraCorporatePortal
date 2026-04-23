<script setup lang="ts">
import { ref, computed, onMounted, onBeforeUnmount } from 'vue'
import axios from 'axios'
import type { Employee } from '@/types/Employee'
import BaseButton from '@/components/ui/BaseButton.vue'
import InverseButton from '@/components/ui/InverseButton.vue'
import positionsService, { type PositionDto } from '@/services/positionsService'

const positions = ref<PositionDto[]>([])

const isFilterOpen = ref(false)

// селекты
const isPositionOpen = ref(false)
const isDepartmentOpen = ref(false)

const selectedPositions = ref<string[]>([])
const selectedDepartments = ref<string[]>([])

const searchQuery = ref('')
const employees = ref<Employee[]>([])
const isLoading = ref(true)
const error = ref<string | null>(null)

// загрузка
const loadEmployees = async () => {
  isLoading.value = true
  error.value = null
  try {
    const response = await axios.get<Employee[]>('https://localhost:5078/api/collaborators')
    employees.value = response.data
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Ошибка загрузки сотрудников'
  } finally {
    isLoading.value = false
  }
}

const loadPositions = async () => {
  try {
    positions.value = await positionsService.getAll()
  } catch (e) {
    console.error('Ошибка загрузки позиций', e)
  }
}

onMounted(() => {
  loadEmployees()
  loadPositions()
  document.addEventListener('click', handleClickOutside)
})

onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside)
})

const uniqueDepartments = computed(() => {
  return [...new Set(employees.value.map(e => e.department))]
  .sort((a, b) => a.localeCompare(b, 'ru'))
})

// toggle
const togglePosition = (pos: string) => {
  if (selectedPositions.value.includes(pos)) {
    selectedPositions.value = selectedPositions.value.filter(p => p !== pos)
  } else {
    selectedPositions.value.push(pos)
  }
}

const toggleDepartment = (dep: string) => {
  if (selectedDepartments.value.includes(dep)) {
    selectedDepartments.value = selectedDepartments.value.filter(d => d !== dep)
  } else {
    selectedDepartments.value.push(dep)
  }
}

// удаление
const removePosition = (pos: string) => {
  selectedPositions.value = selectedPositions.value.filter(p => p !== pos)
}

const removeDepartment = (dep: string) => {
  selectedDepartments.value = selectedDepartments.value.filter(d => d !== dep)
}

// сброс
const resetFilters = () => {
  selectedPositions.value = []
  selectedDepartments.value = []
}

// клик вне
const handleClickOutside = (e: MouseEvent) => {
  const target = e.target as HTMLElement
  if (!target.closest('.select')) {
    isPositionOpen.value = false
    isDepartmentOpen.value = false
  }
}

// фильтрация
const filteredEmployees = computed(() => {
  return employees.value.filter(emp => {
    const matchesSearch =
      !searchQuery.value ||
      emp.fullName.toLowerCase().includes(searchQuery.value.toLowerCase())

    const matchesPosition =
      selectedPositions.value.length === 0 ||
      selectedPositions.value.includes(emp.position)

    const matchesDepartment =
      selectedDepartments.value.length === 0 ||
      selectedDepartments.value.includes(emp.department)

    return matchesSearch && matchesPosition && matchesDepartment
  })
})
</script>

<template>
  <div class="page-title">
    <h1>Справочник сотрудников</h1>
  </div>

  <div class="filters-block">
    <div class="search-block">
      <div class="search-bar">
        <input type="text" v-model="searchQuery" placeholder="Поиск" />
        <img class="icon_search" src="@/assets/icons/search_icon.svg" />
      </div>
      <InverseButton
        :active="isFilterOpen"
        icon="/src/assets/icons/filters_icon.svg"
        iconActive="/src/assets/icons/filters_icon_white.svg"
        @click="isFilterOpen= !isFilterOpen"
      >
        Фильтры
      </InverseButton>
    </div>
  
      <Transition name="filters">
      <div v-if="isFilterOpen" class="filters-dropdown">
        <div class="filters-row">

          <div class="select">
            <div class="select-input" @click="isPositionOpen = !isPositionOpen">
              <span>Должность</span>
              <img
                class="select-icon"
                src="@/assets/icons/arrow_down.svg"
                :class="{ open: isPositionOpen }"
              />
            </div>

            <div v-if="isPositionOpen" class="select-dropdown">
              <div
                v-for="pos in positions"
                :key="pos.id"
                class="option"
                @click="togglePosition(pos.name)"
              >
                <input type="checkbox" :checked="selectedPositions.includes(pos.name)" />
                {{ pos.name }}
              </div>
            </div>
          </div>

          <div class="select">
            <div class="select-input" @click="isDepartmentOpen = !isDepartmentOpen">
              <span>Подразделение</span>
              <img
                class="select-icon"
                src="@/assets/icons/arrow_down.svg"
                :class="{ open: isDepartmentOpen }"
              />
            </div>

            <div v-if="isDepartmentOpen" class="select-dropdown">
              <div
                v-for="dep in uniqueDepartments"
                :key="dep"
                class="option"
                @click="toggleDepartment(dep)"
              >
                <input type="checkbox" :checked="selectedDepartments.includes(dep)" />
                {{ dep }}
              </div>
            </div>
          </div>

          <BaseButton @click="resetFilters" icon-position="left">
            Сбросить
            <template #icon>
              <img src="@/assets/icons/reset_icon.svg" />
            </template>
          </BaseButton>

        </div>
      </div>
    </Transition>

      <div class="filters-params">
        <div
          class="filter-param"
          v-for="pos in selectedPositions"
          :key="'pos-' + pos"
        >
          <p class="base-text">{{ pos }}</p>
          <img
            class="close-icon"
            src="@/assets/icons/close_icon.svg"
            @click="removePosition(pos)"
          >
        </div>

        <div
          class="filter-param"
          v-for="dep in selectedDepartments"
          :key="'dep-' + dep"
        >
          <p class="base-text">{{ dep }}</p>
          <img
            class="close-icon"
            src="@/assets/icons/close_icon.svg"
            @click="removeDepartment(dep)"
          >
        </div>

      </div>

  </div>

  <div class="table-wrapper">
    <table>
      <thead>
        <tr>
          <th>ФИО</th>
          <th>Должность</th>
          <th>Подразделение</th>
          <th>Email</th>
          <th>Внутренний телефон</th>
          <th>Мобильный телефон</th>
        </tr>
      </thead>

      <tbody>
        <tr v-if="isLoading">
          <td colspan="6">Загрузка...</td>
        </tr>
        <tr v-if="error">
          <td colspan="6">Ошибка: {{ error }}</td>
        </tr>
        <tr v-for="emp in filteredEmployees" :key="emp.id">
          <td>{{ emp.fullName }}</td>
          <td>{{ emp.position }}</td>
          <td>{{ emp.department }}</td>
          <td><a :href="`mailto:${emp.email}`">{{ emp.email }}</a></td>
          <td>{{ emp.internalPhone }}</td>
          <td>{{ emp.mobilePhone }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped lang="scss">

.filters-block {
  display: flex;
  flex-direction: column;
  row-gap: 20px;
  margin-bottom: 2rem;
}

.search-block {
  display: flex;
  column-gap: 24px;
}

.filters-row {
  display: flex;
  gap: 24px;
  align-items: center;
}

.select {
  position: relative;
  width: 300px;
}

.select-input {
  border: 1px solid $light-gray;
  border-radius: 50px;
  padding: 12px 24px;
  color: $orange;
  font-weight: 400;
  cursor: pointer;
  min-height: 40px;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.select-icon {
  width: 16px;
  transition: 0.4s;
}

.select-icon.open {
  transform: rotate(180deg);
}

.select-dropdown {
  position: absolute;
  top: 120%;
  width: 100%;
  background: white;
  border: 1px solid $light-gray;
  font-weight: 400;
  border-radius: 15px;
  max-height: 200px;
  overflow-y: auto;
  z-index: 10;
}

.option {
  padding: 8px 12px;
  cursor: pointer;
  display: flex;
  gap: 16px;
}

.option:hover {
  background: rgba(#dfdfdf, 0.3);
}

input[type="checkbox"] {
  accent-color: $blue;
}

.filters-params {
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
}

.filter-param {
  display: flex;
  padding: 8px 20px;
  gap: 8px;
  border: 1px solid $light-gray;
  border-radius: 50px;
}

:global(.filters-enter-active),
:global(.filters-leave-active) {
  transition: all 0.2s ease-out;
}

:global(.filters-enter-from),
:global(.filters-leave-to) {
  opacity: 0;
  transform: translateY(-12px);
}

:global(.filters-enter-to),
:global(.filters-leave-from) {
  opacity: 1;
  transform: translateY(0);
}

.table-wrapper {
  background: white;
  border-radius: $border-radius;
  border: 1px solid $light-gray;
  box-shadow: 0 0 6px rgba(0, 0, 0, 0.15);

  max-height: 480px;
  overflow-y: auto;
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
  table-layout: fixed;
}

thead th {
  position: sticky;
  top: 0;
  background: #f9fafb;
  z-index: 2;
  font-weight: 600;
  font-size: 15px;
  padding: 16px 20px;
}

tbody {
  font-family: $font;
  font-weight: 400;
  font-size: 14px;
}

th, td {
  padding: 12px 20px;
  text-align: left;
  border-bottom: 1px solid $light-gray;
  word-wrap: break-word;
}

tbody tr:hover {
  background-color: rgba(#dfdfdf, 0.3);
}

th:nth-child(1) { width: 200px; }
th:nth-child(2) { width: 180px; }
th:nth-child(3) { width: 180px; }
th:nth-child(4) { width: 240px; }
th:nth-child(5) { width: 140px; }
th:nth-child(6) { width: 150px; }

</style>


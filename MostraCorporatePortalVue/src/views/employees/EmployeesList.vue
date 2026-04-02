<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'
import type { Employee } from '@/types/Employee'

const searchQuery = ref('')
const employees = ref<Employee[]>([])
const isLoading = ref(true)
const error = ref<string | null>(null)

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

onMounted(loadEmployees)

const filteredEmployees = computed(() => {
  if (!searchQuery.value) return employees.value
  return employees.value.filter(emp =>
    emp.fullName.toLowerCase().includes(searchQuery.value.toLowerCase())
  )
})
</script>

<template>
  <div class="page-title">
    <h1>Справочник сотрудников</h1>
  </div>

  <div class="search-bar">
    <input type="text" v-model="searchQuery" placeholder="Поиск" />
    <img class="icon_search" src="../../assets/icons/search_icon.svg" />
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

.icon_search {
  position: absolute;
  left: 14px;
  top: 50%;
  transform: translateY(-50%);
  width: 18px;
  height: 18px;
  opacity: 0.6;
  pointer-events: none;
}

.table-wrapper {
  background: white;
  border-radius: 15px;
  border: 1px solid $light-gray;
  box-shadow: 0 0 6px rgba(0, 0, 0, 0.15);

  max-height: 480px;
  overflow-y: auto;
  overflow-x: auto;
}

table {
  width: 100%;
  max-width: $mw1520;
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
  font-family: 'TT Travels', sans-serif;
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
  background-color: #fff4ec;
}

th:nth-child(1) { width: 200px; }
th:nth-child(2) { width: 180px; }
th:nth-child(3) { width: 180px; }
th:nth-child(4) { width: 240px; }
th:nth-child(5) { width: 140px; }
th:nth-child(6) { width: 150px; }

</style>


<script setup lang="ts">
import { ref } from 'vue'

const searchQuery = ref('')

const employees = ref([
  {
    fullName: 'Глебко Константин Александрович',
    position: 'Инженер-программист',
    department: 'Департамент информационных технологий',
    email: 'k_glebko@mostra.by',
    internalPhone: '',
    mobilePhone: ''
  },
  {
    fullName: 'Думанский Максим Юрьевич',
    position: 'Разработчик',
    department: 'Департамент информационных технологий',
    email: 'm_dumanski@mostra.by',
    internalPhone: '',
    mobilePhone: ''
  },
  {
    fullName: 'Запекин Никита Андреевич',
    position: 'Разработчик',
    department: 'Отдел развития программных продуктов',
    email: 'n_zapekin@mostra.by',
    internalPhone: '1155',
    mobilePhone: '375296611766'
  },
  {
    fullName: 'Швед Дарья Олеговна',
    position: 'Бренд-менеджер',
    department: 'Отдел маркетинга и продвижения',
    email: 'd_shved@mostra.by',
    internalPhone: '',
    mobilePhone: ''
  },
  {
    fullName: 'Петкун Диана Геннадьевна',
    position: 'Делопроизводитель',
    department: 'Отдел внешних маркетинговых коммуникаций',
    email: 'd_petkun@mostra.by',
    internalPhone: '2772',
    mobilePhone: ''
  },
  {
    fullName: 'Кургей Анастасия Анатольевна',
    position: 'Редактор-копирайтер',
    department: 'Отдел развития',
    email: 'n_kyrgey@mostra.by',
    internalPhone: '3131',
    mobilePhone: ''
  },
  {
    fullName: 'Лаханская Надежда Александровна',
    position: 'Дизайнер',
    department: 'Отдел внешних маркетинговых коммуникаций',
    email: 'n_lahanskaya@mostra.by',
    internalPhone: '',
    mobilePhone: ''
  },
  {
    fullName: 'Боровая Полина Сергеевна',
    position: 'Маркетолог',
    department: 'Отдел маркетинга',
    email: 'p_borovaya@mostra.by',
    internalPhone: '222',
    mobilePhone: '+375447716161'
  },
  {
    fullName: 'Коваль Максим Алексеевич',
    position: 'Разработчик',
    department: 'Департамент информационных технологий',
    email: 'm_koval@mostra.byy',
    internalPhone: '',
    mobilePhone: ''
  }
  
])

const filteredEmployees = () => {
  if (!searchQuery.value) return employees.value
  return employees.value.filter(emp =>
    emp.fullName.toLowerCase().includes(searchQuery.value.toLowerCase())
  )
}
</script>

<template>

  <div class="page-title">
     <h1>Справочник сотрудников</h1>
  </div>

  <div class="search-bar">
    <input
      type="text"
      v-model="searchQuery"
      placeholder="Поиск"
    />
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
        <tr v-for="emp in filteredEmployees()" :key="emp.email">
          <td>{{ emp.fullName }}</td>
          <td>{{ emp.position }}</td>
          <td>{{ emp.department }}</td>
          <td>
            <a :href="`mailto:${emp.email}`">{{ emp.email }}</a>
          </td>
          <td>{{ emp.internalPhone }}</td>
          <td>{{ emp.mobilePhone }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>

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
  border: 1px solid var(--light-gray);
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
  font-family: 'TT Travels', sans-serif;
  font-weight: 400;
  font-size: 14px;
}

th, td {
  padding: 12px 20px;
  text-align: left;
  border-bottom: 1px solid var(--light-gray);
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


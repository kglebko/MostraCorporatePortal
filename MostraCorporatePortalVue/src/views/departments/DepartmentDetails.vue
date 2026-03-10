<script setup lang="ts">
import { ref, watch } from 'vue'
import { useRoute } from 'vue-router'
import DashboardCard from '@/components/DashboardCard.vue'
import apiService from '@/services/apiService'

interface Manager {
  id: string
  fullName: string
}

interface Department {
  id: number
  name: string
  organizationId: number
  organizationName: string
  parentDepartmentId?: number
  parentDepartmentName?: string
  managers: Manager[]
}

const route = useRoute()
const department = ref<Department | null>(null)

async function loadDepartment(id: string | number) {
  department.value = await apiService.get(`/departments/${id}`)
}

// при первоначальной загрузке
const initialId = Array.isArray(route.params.id) ? route.params.id[0] : route.params.id
if (initialId) loadDepartment(initialId)

// следим за изменением параметра id
watch(
  () => route.params.id,
  (newId) => {
    const id = Array.isArray(newId) ? newId[0] : newId
    if (id) loadDepartment(id)
  }
)
</script>

<template>

    <div v-if="department">
        <div class="page-title">
            <h1>{{ department.name }}</h1>
            <h2>Подразделение</h2>
        </div>
    
        <div class="dashboard">
            <DashboardCard style='width: 100%;'>
                <div class="user-information">
                    <div class="about-user-block">
                        <div class="info-row">
                            <p class="user-about">Название:</p>
                            <p class="user-info">{{ department.name }}</p>
                        </div>
                        <div class="info-row">
                            <p class="user-about">Руководитель:</p>
                            <div class="info-list">
                                <p
                                v-for="manager in department.managers"
                                :key="manager.id"
                                class="user-info"
                                >
                                {{ manager.fullName }}
                                </p>
                            </div>
                        </div>
                        <div class="info-row" v-if="department.parentDepartmentName">
                            <p class="user-about">Родительское подразделение:</p>
                            <RouterLink
                                :to="{ name:'departmentDetails', params:{ id: department.parentDepartmentId }}"
                                class="user-info"
                                >
                                {{ department.parentDepartmentName }}
                                </RouterLink>
                        </div>
                        <div class="info-row">
                            <p class="user-about">Организация:</p>
                            <RouterLink
                                :to="{ name:'organizationDetails', params:{ id: department.organizationId }}"
                                class="user-info"
                                >
                                {{ department.organizationName }}
                                </RouterLink>
                        </div>
                    </div>
                </div>
            </DashboardCard>
        </div>
    </div>
    
</template>



<style scoped>

.user-information {
    flex: 1;  
    display: flex;
    flex-direction: column;
    gap: 40px;
}

.about-user-block {
    display: flex;
    flex-direction: column;
    gap: 16px;
}

a{
    color: var(--primary-orange)
}

.info-row {
    display: flex;
    gap: 2rem;
}

.user-about {
    width: 320px;
    color: var(--blue-gray);
    font-size: 16px;
    font-weight: 400;
}

.user-info {
    flex: 1;
    padding: 0
}

.info-list{
    display: flex;
    flex-direction: column;
    gap: 4px;
}

</style>


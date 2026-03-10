<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import DashboardCard from '@/components/DashboardCard.vue'
import apiService from '@/services/apiService'

interface Manager {
  id: string
  fullName: string
}

interface Organization {
  id:number
  name:string
  managers: Manager[]
}

const route = useRoute()

const organization = ref<Organization | null>(null)

onMounted(async () => {
  organization.value = await apiService.get(`/organizations/${route.params.id}`)
})

</script>

<template>

    <div v-if="organization">

        <div class="page-title">
            <h1>{{ organization.name }}</h1>
            <h2>Организация</h2>
        </div>
        
        <div class="dashboard">
            <DashboardCard style='width: 100%;'>
                <div class="user-information">
                    <div class="about-user-block">
                        <div class="info-row">
                            <p class="user-about">Название:</p>
                            <p class="user-info">{{ organization.name }}</p>
                        </div>
                        <div class="info-row">
                            <p class="user-about">Руководитель:</p>
                            <div class="info-list">
                                <p
                                v-for="manager in organization.managers"
                                :key="manager.id"
                                class="user-info"
                                >
                                {{ manager.fullName }}
                                </p>
                            </div>
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


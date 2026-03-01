<script setup lang="ts">

</script>

<script setup lang="ts">
import DashboardCard from '@/components/DashboardCard.vue'
</script>

<template>
    
    <div class="page-title">
        <h1>MOSTRA</h1>
        <h2>Организация</h2>
    </div>

    <div class="dashboard">
            <DashboardCard style='width: 100%;'>
                <div class="user-information">
                    <div class="about-user-block">
                        <div class="info-row">
                            <p class="user-about">Название:</p>
                            <p class="user-info">MOSTRA</p>
                        </div>
                        <div class="info-row">
                            <p class="user-about">Руководитель:</p>
                        </div>
                    </div>
                </div>
            </DashboardCard>
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


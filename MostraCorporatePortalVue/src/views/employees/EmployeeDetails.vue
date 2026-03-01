<script setup lang="ts">
import DashboardCard from '@/components/DashboardCard.vue'
</script>

<template>
    
    <div class="page-title">
        <h1>Глебко Константин Александрович</h1>
        <h2>Сотрудник</h2>
    </div>

    <div class="dashboard">
        <div class="dashboard-row">
            <DashboardCard title="Основные сведения" style='width: 74%;'>
            <div class="content-block">
                <div class="avatar">
                    <img src="../../assets/images/profile_foto.png" alt="">
                </div>
                <div class="user-information">
                    <div class="about-user-block">
                        <div class="info-row">
                            <p class="user-about">Должность:</p>
                            <p class="user-info">Инженер-программист</p>
                        </div>
                        <div class="info-row">
                            <p class="user-about">Подразделение:</p>
                            <RouterLink :to="{ name: 'departmentDetails'}" class="user-info">Департамент информационных технологий</RouterLink>
                        </div>
                        <div class="info-row">
                            <p class="user-about">Организация:</p>
                            <RouterLink :to="{ name: 'organizationDetails'}" class="user-info">MOSTRA</RouterLink>
                        </div>
                    </div>
                    <div class="about-user-block">
                        <div class="info-row">
                            <p class="user-about">Формат работы:</p>
                            <p class="user-info">Гибридный</p>
                        </div>
                        <div class="info-row">
                            <p class="user-about">Внутренний телефон:</p>
                        </div>
                    </div>
                    <div class="about-user-block">
                        <div class="info-row">
                            <p class="user-about">Логин:</p>
                            <p class="user-info">k_glebko</p>
                        </div>
                        <div class="info-row">
                            <p class="user-about">E-mail:</p>
                            <p class="user-info">k_glebko@mostra.by</p>
                        </div>
                        <div class="info-row">
                            <p class="user-about">Мобильный телефон:</p>
                        </div>
                    </div>
                    <div class="about-user-block">
                        <div class="info-row">
                            <p class="user-about">Дата рождения:</p>
                            <p class="user-info">02.12.1977</p>
                        </div>
                        <div class="info-row">
                            <p class="user-about">Пол:</p>
                            <p class="user-info">Мужской</p>
                        </div>
                    </div>
                </div>


            </div>
            </DashboardCard>

            <DashboardCard
                title="Пройденные курсы и тесты"
                style='width: 26%;'
                >

                <div class="stats">
                    <div id="completed-tests-count" class="stats-item">
                        <p class="completed-count">2</p>
                        <p class="completed-label">тесты</p>
                    </div>
                    <div id="completed-courses-count" class="stats-item">
                        <p class="completed-count">3</p>
                        <p class="completed-label">курсы</p>
                    </div>
                </div>

                <div id="completed_tests" class="info-list">
                    <div class="list_item">
                        <img class="icon" src="/icons/completed_test_icon.svg"> 
                        <p>Охрана труда</p>
                    </div>
                    <div class="list_item">
                        <img class="icon" src="/icons/completed_test_icon.svg"> 
                        <p>Информационная безопасность</p>
                    </div>
                </div>

                <div id="completed_courses" class="info-list">
                    <div class="list_item">
                        <img class="icon" src="/icons/course_icon.svg"> 
                        <p>Адаптационный курс</p>
                    </div>
                    <div class="list_item">
                        <img class="icon" src="/icons/course_icon.svg"> 
                        <p>Профессиональная этика</p>
                    </div>
                     <div class="list_item">
                        <img class="icon" src="/icons/course_icon.svg"> 
                        <p>Введение в корпоративные процессы</p>
                    </div>
                </div>

            </DashboardCard>
        </div>
    </div>

</template>



<style scoped>

.content-block {
    display: flex;
    gap: 60px;
    align-items: center;
}

.avatar {
    flex: 0 0 300px;
    aspect-ratio: 1;
    border-radius: 50%;
    overflow: hidden;
}

.avatar img {
    width: 100%;
    height: 100%;
    object-fit: cover;
    display: block;
}

.user-information {
    flex: 1;  
    display: flex;
    flex-direction: column;
    gap: 40px;
}

.about-user-block {
    display: flex;
    flex-direction: column;
    gap: 8px;
}

a{
    color: var(--primary-orange)
}

.info-row {
    display: flex;
    gap: 2rem;
}

.user-about {
    width: 200px;
    color: var(--blue-gray);
    font-size: 16px;
    font-weight: 400;
}

.user-info {
    flex: 1;
    padding: 0;
}

.info-list{
    display: flex;
    flex-direction: column;
    row-gap: 1rem;
}

.list_item{
    display: flex;
    gap: 12px;
    align-items: center;
}

.stats{
    display: flex;
    justify-content: center;
    column-gap: 2rem;
    margin: 0.2rem 0 0.4rem;
}

.stats-item{
    display: flex;
    flex-direction: column;
    align-items: center;
}

.completed-count{
    color: var(--primary-orange);
    font-size: 24px;
    font-weight: 600;
}

.completed-label{
    font-weight: 400;
    font-size: 16px;
    color: var(--blue-gray);
}

</style>


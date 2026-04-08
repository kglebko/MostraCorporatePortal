<script setup lang="ts">

</script>

<template>
    <div class="page-title">
        <h1>Внедрены изменения в процессы внутреннего взаимодействия команд для  ускорения рабочих процессов</h1>
        <h2>6 февраля 2026 г.</h2>
    </div>

    <div class="news-info-block">
        <div class="news-image">
            <img src="../../assets/images/news_photos/news3.png">
        </div>
        <div class="news-content">
            <div class="news-description">
                <p class="base-text">Внедрение изменений направлено на улучшение качества внутренней отчетности и прозрачности процессов. Новые подходы предполагают использование более современных инструментов и технологий для обмена информацией, что позволит сотрудникам быстрее и точнее отслеживать выполнение задач, а также своевременно выявлять возможные проблемы. Кроме того, будет укреплено сотрудничество между департаментами через регулярные обмены информацией и четкую структуру коммуникаций.</p>
                <p class="base-text">Одной из ключевых инициатив является внедрение гибких методов работы, таких как Agile и Scrum, что позволит более оперативно адаптироваться к изменяющимся условиям рынка и внутренним требованиям. Благодаря этим методам, компании удастся не только повысить скорость выполнения проектов, но и улучшить качество конечных результатов за счет постоянной оценки и корректировки рабочих процессов.</p>
                <p class="base-text">Среди других важных шагов — создание единой базы знаний, в которой будут собраны лучшие практики и стандарты, что обеспечит унификацию рабочих процессов и повысит уровень навыков сотрудников. Это позволит не только ускорить выполнение повседневных задач, но и повысить общую квалификацию команды.</p>
                <p class="base-text">В дальнейшем планируется расширение этих изменений на другие аспекты бизнеса, что поможет обеспечить устойчивый рост компании и сохранение ее лидерских позиций на рынке. Важно отметить, что данные инициативы будут подкреплены регулярным обучением и тренингами для сотрудников, что позволит им более эффективно использовать новые инструменты и подходы.</p>
            </div>
            <RouterLink
                :to="{ name: 'newsList'}"
                class="more-block"
                >
                <img class="arrow-more" src="../../assets/icons/arrow_l.svg">
                <p class="more">Все новости</p>
            </RouterLink>
        </div>
    </div>

    <div class="news-more">
        <h3>Также будет интересно</h3>
        <div class="news-more-block">

            <div class="main-post">
                <img class="news-photo" src="../../assets/images/news_photos/news2.png">
                <div class="news-info">
                <a class="news-name">Внедрены изменения в процессы внутреннего взаимодействия команд</a>
                <p class="news-date">6 февраля 2026 г.</p>
                </div>
            </div>
            <div class="main-post">
                <img class="news-photo" src="../../assets/images/news_photos/news2.png">
                <div class="news-info">
                <a class="news-name">Внедрены изменения в процессы внутреннего взаимодействия команд</a>
                <p class="news-date">6 февраля 2026 г.</p>
                </div>
            </div>
            <div class="main-post">
                <img class="news-photo" src="../../assets/images/news_photos/news2.png">
                <div class="news-info">
                <a class="news-name">Внедрены изменения в процессы внутреннего взаимодействия команд</a>
                <p class="news-date">6 февраля 2026 г.</p>
                </div>
            </div>
            <div class="main-post">
                <img class="news-photo" src="../../assets/images/news_photos/news2.png">
                <div class="news-info">
                <a class="news-name">Внедрены изменения в процессы внутреннего взаимодействия команд</a>
                <p class="news-date">6 февраля 2026 г.</p>
                </div>
            </div>
            <div class="main-post">
                <img class="news-photo" src="../../assets/images/news_photos/news2.png">
                <div class="news-info">
                <a class="news-name">Внедрены изменения в процессы внутреннего взаимодействия команд</a>
                <p class="news-date">6 февраля 2026 г.</p>
                </div>
            </div>

        </div>

    </div>
    
</template>

<style scoped lang="scss">

.news-info-block {
    display: flex;
    width: 100%;
    gap: 6%;
}

.news-image {
    display: flex;
    width: 42%;
}

.news-image img {
    width: 100%;
    border-radius: $border-radius;
    object-fit: cover;
}

.news-content {
    display: flex;
    flex-direction: column;
    row-gap: 40px;
    justify-content: space-between;
    width: 52%;
}

.news-description{
    display: flex;
    flex-direction: column;
    row-gap: 20px;
}

.news-more{
    margin-top: 4rem;
    display: flex;
    flex-direction: column;
    row-gap: 24px;
}

/*
.news-more-block{
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 24px;
}

.main-post{
    display: flex;
    flex-direction: column;
    border: 1px solid $light-gray;
    border-radius: $border-radius;
    overflow: hidden;
}
*/

.news-more-block{
  display: flex;
  gap: 24px;
  overflow-x: auto;
  padding-bottom: 8px;
}

.main-post{
  flex: 0 0 360px;
  display: flex;
  flex-direction: column;
  border: 1px solid $light-gray;
  border-radius: $border-radius;
  overflow: hidden;
}

.news-more-block::-webkit-scrollbar {
  height: 8px;
}

.news-more-block::-webkit-scrollbar-thumb {
  background: $light-gray;
  border-radius: 10px;
}

.news-photo {
    width: 100%;
    height: 200px;  
    object-fit: cover;
    display: block;
    border-radius: $border-radius $border-radius 0 0;
}

.news-name{
    display: -webkit-box;
    -webkit-line-clamp: 2;
    line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
    text-overflow: ellipsis;
}

.news-info{
    padding: 12px;
    display: flex;
    flex-direction: column;
    row-gap: 4px;
}

.news-date{
  @include label;
}

</style>

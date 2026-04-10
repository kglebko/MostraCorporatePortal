<script setup lang="ts">
import { onMounted, ref } from 'vue'
import newsService from '@/services/newsService'
import type { News } from '@/types/News'
import { getNewsImage , formatDate} from '@/utils/helpers'

const newsList = ref<News[]>([])

onMounted(async () => {
  newsList.value = await newsService.getAll()
})
</script>

<template>

<div class="page-title">
  <h1>Новости</h1>
</div>

<div class="news">

  <div
    v-for="news in newsList"
    :key="news.id"
    class="news-table"
  >

    <img class="news-img" :src="getNewsImage(news.image)">

    <div class="news-info">

      <div class="news-about">

        <RouterLink
          :to="{ name: 'newsItem', params: { id: news.id } }"
          class="news-title"
        >
          <a>{{ news.title }}</a>
        </RouterLink>

        <p class="news-text">
          {{ news.description }}
        </p>

      </div>

      <div class="news-details">

        <div class="date-block">
          <img src="../../assets/icons/date_publish.svg">
          <p class="news-date-publish">
            {{ formatDate(news.date) }}
          </p>
        </div>

        <RouterLink
          :to="{ name: 'newsItem', params: { id: news.id } }"
          class="more-block"
        >
          <p class="more">Подробнее</p>
          <img class="arrow-more" src="../../assets/icons/arrow_r.svg">
        </RouterLink>

      </div>

    </div>

  </div>

</div>

</template>

<style scoped lang="scss">

.news{
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 40px;
}

.news-table{
  display: flex;
  flex-direction: column;
  border: 1px solid $light-gray;
  border-radius: $border-radius;
}

.news-img{
  width: 100%;
  height: 300px;
  object-fit: cover;
  background-color: $light-gray;
  border-radius: $border-radius $border-radius 0 0;
}

.news-info{
  padding: 20px;
  display: flex;
  flex-direction: column;
  row-gap: 24px;
}

.news-about{
  display: flex;
  flex-direction: column;
  row-gap: 16px;
}

.news-title{
  font-size: $bs;
  color: $blue;
  font-weight: 500;
  padding: 0;

  display: -webkit-box;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  text-overflow: ellipsis;
}

.news-text{
  @include label;

  display: -webkit-box;
  -webkit-line-clamp: 3;
  line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  text-overflow: ellipsis;
}

.news-details{
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.date-block{
  display: flex;
  column-gap: 8px;
  align-items: center;
}

.news-date-publish{
  font-size: $sm;
  font-weight: $light;
  color: $gray;
}


</style>

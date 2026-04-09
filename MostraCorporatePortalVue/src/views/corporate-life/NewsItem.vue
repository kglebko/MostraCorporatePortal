<script setup lang="ts">
import { onMounted, ref, computed, watch } from 'vue'
import { useRoute } from 'vue-router'
import newsService from '@/services/newsService'
import type { News } from '@/types/News'
import { getNewsImage , formatDate} from '@/utils/helpers'

const route = useRoute()

const news = ref<News | null>(null)
const relatedNews = ref<News[]>([])

const paragraphs = computed(() => {
  if (!news.value?.description) return []

  return news.value.description
    .split('\n')
    .map(p => p.trim())
    .filter(p => p.length)
})

async function loadNews(id: number) {
  news.value = await newsService.getById(id)

  const latest = await newsService.getLatest(5)
  relatedNews.value = latest.filter(n => n.id !== id)
}

onMounted(() => {
  loadNews(Number(route.params.id))
})

watch(
  () => route.params.id,
  (newId) => {
    loadNews(Number(newId))
  }
)

</script>

<template>

<div v-if="news">

<div class="page-title">
  <h1>{{ news.title }}</h1>
  <h2>{{ formatDate(news.date) }}
</h2>
</div>

<div class="news-info-block">

  <div class="news-image">
    <img class="news-img" :src="getNewsImage(news.image)">
  </div>

  <div class="news-content">

    <div class="news-description">
        <p
        v-for="(p, index) in paragraphs"
        :key="index"
        class="base-text"
        >
            {{ p }}
        </p>
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

    <div
        v-for="item in relatedNews"
        :key="item.id"
        class="main-post"
        >

        <img
            class="news-photo"
            :src="getNewsImage(item.image)"
        >

        <div class="news-info">
            <RouterLink
            :to="{ name: 'newsItem', params: { id: item.id } }"
            class="news-name"
            >
            {{ item.title }}
            </RouterLink>

            <p class="news-date">
            {{ formatDate(item.date) }}
            </p>
        </div>

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
  padding-bottom: 12px;
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
    height: 10px;
}

.news-more-block::-webkit-scrollbar-thumb {
    background: $light-gray;
    border-radius: 10px;
}

.news-more-block::-webkit-scrollbar-thumb:hover {
    background: rgba(#f67001, 0.3);
}

.news-more-block::-webkit-scrollbar-thumb:active {
    background: rgba(#f67001, 0.5);
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

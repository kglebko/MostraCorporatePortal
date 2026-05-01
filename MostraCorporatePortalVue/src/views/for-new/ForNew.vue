<script setup lang="ts">
import { ref } from 'vue'
import arrowIcon from '@/assets/icons/arrow_r.svg'
import downloadIcon from '/icons/download_icon.svg'
import loyaltyProgramFile from '@/assets/files/loyalty_program.pdf'

type ContentItem = {
  text: string
  icon?: string
}

type Section = {
  key: string
  title: string
  icon: string
  content: ContentItem[]
  link?: { text: string; to: string }
  file?: string
  fileLinkText?: string
}

type Stat = {
  value: string
  label: string
  icon: string
}

function handleDownload(filePath: string) {
  const link = document.createElement('a')
  link.href = filePath
  link.download = filePath.split('/').pop() || 'download'
  link.target = '_blank'
  document.body.appendChild(link)
  link.click()
  document.body.removeChild(link)
}

const stats: Stat[] = [
  { value: 'Топ-10', label: 'Поставщик в Беларуси', icon: '/icons/top_10_icon.svg' },
  { value: '95%', label: 'Мировых брендов в портфеле', icon: '/icons/world_brands_icon.svg' },
  { value: '20%', label: 'Доля рынка FMCG Беларуси', icon: '/icons/sales_market_icon.svg' },
  { value: '4 500+', label: 'SKU в ассортименте', icon: '/icons/sku_icon.svg' },
  { value: '70%', label: 'Партнёров с нами более 7 лет', icon: '/icons/partners_icon.svg' },
  { value: '10 500', label: 'Точек доставки ежемесячно', icon: "/icons/delivery_icon.svg" },
  { value: '5 500', label: 'Заказов каждый день', icon: '/icons/orders_icon.svg' },
  { value: '50+', label: 'Городов присутствия в Беларуси', icon: '/icons/cities_icon.svg' }
]

const sections: Section[] = [
  {
    key: 'about',
    title: 'О компании',
    icon: '/icons/company_logo_icon.svg',
    content: [
      { text: 'ОДО "Мостра-групп" — лидер в области дистрибуции товаров народного потребления в Беларуси. Мы объединяем мировые бренды и локальный рынок, обеспечивая высокое качество сервиса и надёжность поставок.' },
      { text: 'Наша миссия — делать лучшие товары доступными для каждого жителя Беларуси.' },
      { text: 'Более 15 лет успешной работы, постоянное развитие и инновации — наши главные приоритеты.' }
    ]
  },
  {
    key: 'departments',
    title: 'Подразделения и офисы',
    icon: '/icons/building_icon.svg',
    content: [
      { text: 'Главный офис: г. Минск, пр. Партизанский, 79 (БЦ "PRIZMA")', icon: '/icons/location_icon.svg' },
      { text: 'График работы: с 9:00 до 18:00', icon: '/icons/clock_icon.svg' },
      { text: '11 этаж: бухгалтерия, HR, отдел учёта товара, отдел кадров, диджитал департамент', icon: '/icons/list_marker_icon.svg' },
      { text: '12 этаж: коммерческий департамент, логистика, контроллинг, IT и автоматизация', icon: '/icons/list_marker_icon.svg' },
      { text: '13 этаж: юридический отдел, департамент безопасности, маркетинг', icon: '/icons/list_marker_icon.svg' },
      { text: 'Офис на Селицкого: г. Минск, ул. Селицкого, 21б', icon: '/icons/location_icon.svg' },
      { text: 'Региональные филиалы: Барановичи, Брест, Витебск, Гомель, Гродно, Могилёв', icon: '/icons/location_icon.svg' }
    ],
    link: { text: 'Перейти в справочник сотрудников', to: '/employees' }
  },
  {
    key: 'salary',
    title: 'Выплата заработной платы',
    icon: '/icons/money_icon.svg',
    content: [
      { text: 'Зарплатные карты: ЗАО "Альфа-Банк", ОАО "Банк БелВЭБ"'},
      { text: 'Офисные сотрудники: аванс — 25 числа, основная часть + премия — 10 числа', icon: '/icons/list_marker_icon.svg' },
      { text: 'Коммерческие команды: аванс — 25 числа, остаток — 10 числа, премия — 28–31 числа', icon: '/icons/list_marker_icon.svg' },
      { text: 'ООО "Тибетрэй": аванс — 28 числа, остаток + премия — 15 числа', icon: '/icons/list_marker_icon.svg' },
      { text: 'ООО "Стима": аванс — 28 числа, остаток + премия — 18 числа', icon: '/icons/list_marker_icon.svg' },
      { text: 'Если дата выплаты выпадает на выходной — выплата производится в предыдущий рабочий день' }
    ]
  },
  {
    key: 'benefits',
    title: 'Наши преимущества',
    icon: '/icons/advantages_icon.svg',
    content: [
      { text: 'Стабильная заработная плата и своевременные выплаты', icon: '/icons/list_marker_icon.svg' },
      { text: 'Официальное трудоустройство и полный социальный пакет', icon: '/icons/list_marker_icon.svg' },
      { text: 'ДМС и корпоративные программы здоровья', icon: '/icons/list_marker_icon.svg' },
      { text: 'Корпоративное обучение и развитие', icon: '/icons/list_marker_icon.svg' },
      { text: 'Комфортные офисы и современное оборудование', icon: '/icons/list_marker_icon.svg' },
      { text: 'Возможности для карьерного роста', icon: '/icons/list_marker_icon.svg' },
      { text: 'Дружный коллектив и корпоративные мероприятия', icon: '/icons/list_marker_icon.svg' }
    ]
  },
  {
    key: 'medical',
    title: 'Медицинская помощь и ДМС',
    icon: '/icons/health_icon.svg',
    content: [
      { text: 'При плохом самочувствии сообщите руководителю и в HR', icon: '/icons/list_marker_icon.svg' },
      { text: 'Экстренные случаи — звоните 103 или на рецепцию офиса', icon: '/icons/list_marker_icon.svg' },
      { text: 'Список медицинских учреждений по ДМС можно уточнить в HR-отделе', icon: '/icons/list_marker_icon.svg' },
      { text: 'Для оформления больничного передайте номер ЭЛН в отдел кадров в день открытия', icon: '/icons/list_marker_icon.svg' },
      { text: 'Раз в год — бесплатный профилактический осмотр', icon: '/icons/list_marker_icon.svg' }
    ]
  },
  {
    key: 'resources',
    title: 'Техника и материальные ценности',
    icon: '/icons/tools_icon.svg',
    content: [
      { text: 'Ноутбук, гарнитура, пропуск, периферия — оформляется через заявочный сервис', icon: '/icons/list_marker_icon.svg' },
      { text: 'Доступы к системам согласовываются с руководителем и IT-департаментом', icon: '/icons/list_marker_icon.svg' },
      { text: 'Папка обмена информацией — по заявке в течение 1 рабочего дня', icon: '/icons/list_marker_icon.svg' },
      { text: 'Курьерская служба, картриджи, канцтовары, визитки — через АХО', icon: '/icons/list_marker_icon.svg' }
    ]
  },
  {
    key: 'letters',
    title: 'Корреспонденция и документы',
    icon: '/icons/docs_icon.svg',
    content: [
      { text: 'Входящая и исходящая корреспонденция регистрируется через секретариат', icon: '/icons/list_marker_icon.svg' },
      { text: 'Для срочных отправлений — пометка "Срочно" и контактный номер получателя', icon: '/icons/list_marker_icon.svg' },
      { text: 'Внутренние документы — через корпоративный портал', icon: '/icons/list_marker_icon.svg' },
      { text: 'Подписанные оригиналы передаются через курьерскую службу офиса', icon: '/icons/list_marker_icon.svg' }
    ]
  },
  {
    key: 'loyalty',
    title: 'Программа лояльности для сотрудников',
    icon: '/icons/loyalty_icon.svg',
    content: [
      { text: 'Скидки и бонусы от партнёров компании', icon: '/icons/list_marker_icon.svg' },
      { text: 'Специальные предложения на фитнес и wellness', icon: '/icons/list_marker_icon.svg' },
      { text: 'Скидки в кафе и ресторанах-партнёрах', icon: '/icons/list_marker_icon.svg' },
      { text: 'Акции на товары из нашего портфеля', icon: '/icons/list_marker_icon.svg' }
    ],
    fileLinkText: 'Скачать программу лояльности',
    file: loyaltyProgramFile
  }
]

const opened = ref<string[]>(['about'])

function toggleSection(key: string) {
  if (opened.value.includes(key)) {
    opened.value = opened.value.filter(x => x !== key)
  } else {
    opened.value.push(key)
  }
}
</script>

<template>
  <div class="page-header">
    <h1>Новичку</h1>
    <p class="page-subtitle">Всё, что нужно знать для успешного старта</p>
  </div>
  <div class="for-new">
    <div class="welcome-card">
      <div class="welcome-icon"><img src="@/assets/icons/logo.svg" alt="logo"></div>
      <h2>Добро пожаловать в команду!</h2>
      <p class="welcome-text">
        Дорогой коллега, мы рады приветствовать Вас в нашей компании! 
        Здесь Вы найдёте ответы на все вопросы, которые могут возникнуть 
        в первые дни работы, а также познакомитесь с основными правилами 
        и традициями нашей команды.
      </p>
      <p class="welcome-quote">
        «Вместе мы создаём лучшее будущее для наших клиентов и партнёров»
      </p>
    </div>

    <div class="stats-section">
      <h2 class="section-title">Мостра-Групп в цифрах</h2>
      <div class="stats-grid">
        <div v-for="(stat, idx) in stats" :key="idx" class="stat-card">
          <img v-if="stat.icon && stat.icon.startsWith('/icons/')" 
            :src="stat.icon" 
            class="stat-icon"
            :alt="stat.label" />
          <div class="stats-text">
            <div class="stat-value">{{ stat.value }}</div>
            <div class="stat-label">{{ stat.label }}</div>
          </div>
        </div>
      </div>
    </div>

    <div class="info-card">
      <h2 class="section-title">Полезная информация</h2>

      <div v-for="(section, index) in sections" :key="section.key" class="info-item" :class="{ 'with-border': index !== sections.length - 1 }">
        <button class="info-header" @click="toggleSection(section.key)">
          <div class="header-left">
            <img 
              v-if="section.icon && section.icon.includes('/icons/')" 
              :src="section.icon" 
              class="section-icon" 
              alt="icon" />
            <span class="section-title-text">{{ section.title }}</span>
          </div>
          <img 
            :src="arrowIcon" 
            class="toggle-arrow"
            :class="{ rotated: opened.includes(section.key) }"
            alt="toggle" />
        </button>

        <div v-if="opened.includes(section.key)" class="info-body">
          <div class="content-list">
            <div v-for="(item, idx) in section.content" :key="idx" class="content-line">
              <span v-if="item.icon && !item.icon.startsWith('/')" class="content-icon">{{ item.icon }}</span>
              <img 
                v-else-if="item.icon && item.icon.startsWith('/')" 
                :src="item.icon" 
                class="content-image-icon" 
                alt="icon" />
              <span class="content-text">{{ item.text }}</span>
            </div>
          </div>

          <RouterLink
            v-if="section.link"
            :to="section.link.to"
            class="info-link"
          >
            {{ section.link.text }}
          </RouterLink>

          <a 
            v-if="section.file"
            href="#"
            class="download-link"
            @click.prevent="handleDownload(section.file)"
          >
            <img :src="downloadIcon" class="download-link-icon" alt="download" />
            {{ section.fileLinkText || 'Скачать файл' }}
          </a>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
.for-new {
  display: flex;
  flex-direction: column;
  gap: 40px;
}

.page-header{
  align-items: center;
}

h1{
  background: linear-gradient(135deg, #1a4d8c 0%, #f67001 100%);
  background-clip: text;
  -webkit-background-clip: text;
  color: transparent;
}

.page-subtitle {
  font-size: 18px;
  color: $blue;
  margin: 0;
}

.welcome-card {
  background: linear-gradient(135deg, #f8f9fa 0%, rgba(246, 112, 1, 0.10));
  border-radius: $border-radius;
  padding: 32px;
  text-align: center;
  border: 1px solid $light-gray;
  box-shadow: 0 0 6px rgba(0, 0, 0, 0.15);
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.welcome-icon img{
  height: 60px;
  object-fit: cover;
}

.welcome-card h2 {
  font-size: 28px;
  color: $blue;
}

.welcome-text {
  font-size: 18px;
  line-height: 1.6;
  color: $gray;
}

.welcome-quote {
  font-style: italic;
  font-size: 18px;
}

.stats-section {
  display: flex;
  flex-direction: column;
  gap: 32px;
  background: #fff;
  border-radius: $border-radius;
  padding: 32px;
  border: 1px solid $light-gray;
  box-shadow: 0 0 6px rgba(0, 0, 0, 0.15);
}

.section-title {
  font-size: 24px;
  color: $blue;
  text-align: center;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
}

.stat-card {
  display: flex;
  flex-direction: column;
  gap: 16px;
  text-align: center;
  padding: 24px 12px;
  background: #f8f9fa;
  border-radius: 16px;
  transition: all 0.3s ease;
}

.stat-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 0 6px rgba(0, 0, 0, 0.15);
}

.stat-icon {
  height: 48px;
  object-fit: contain;
}

.stats-text{
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.stat-value {
  font-size: 28px;
  font-weight: 800;
  color: $orange;
}

.stat-label {
  font-size: 14px;
  color: $gray;
}

.info-card {
  background: white;
  border-radius: 24px;
  padding: 32px;
  border: 1px solid $light-gray;
  box-shadow: 0 0 6px rgba(0, 0, 0, 0.15);
}

.info-item {
  margin-bottom: 8px;
  
  &:last-child {
    margin-bottom: 0;
  }
  
  &.with-border {
    border-bottom: 1px solid $light-gray;
    margin-bottom: 8px;
    padding-bottom: 8px;
  }
}

.info-header {
  width: 100%;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px 0;
  background: none;
  border: none;
  cursor: pointer;
  text-align: left;
  transition: all 0.2s ease;
  font-family: 'TT Travels';
  margin-bottom: 8px;
  
  &:hover {
    .section-title-text {
      color: $orange;
    }
  }
}

.header-left {
  display: flex;
  align-items: center;
  gap: 16px;
  flex: 1;
}

.section-icon {
  width: 24px;
  height: 24px;
  object-fit: contain;
}

.section-title-text {
  font-size: 18px;
  font-weight: 600;
  color: $blue;
  transition: color 0.2s;
  padding-top: 2px;
}

.toggle-arrow {
  width: 16px;
  height: 16px;
  transition: transform 0.3s ease;
  
  &.rotated {
    transform: rotate(90deg);
  }
}

.info-body {
  display: flex;
  flex-direction: column;
  gap: 16px;
  padding-bottom: 16px;
  animation: fadeIn 0.3s ease;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-8px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.content-list {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.content-line {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  font-size: 16px;
  color: $gray;
}

.content-icon {
  font-size: 18px;
  min-width: 24px;
  text-align: center;
}

.content-image-icon {
  width: 24px;
  height: 20px;
  object-fit: contain;
}

.content-text {
  flex: 1;
  color: $gray;
}

.info-link {
  display: inline-block;
  margin-top: 4px;
  color: $orange;
  font-weight: 500;
  transition: 0.4s ease-out;
    
  &:hover {
    transform: translateY(-2px);
  }
}

.download-link {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  margin-top: 4px;
  color: $orange;
  font-weight: 500;
  transition: 0.4s ease-out;
  cursor: pointer;
  
  &:hover {
    transform: translateY(-2px);
  }
}

.download-link-icon {
  width: 18px;
  height: 18px;
}

@media (max-width: 768px) {
  .for-new {
    padding: 16px;
    gap: 20px;
  }
  
  .page-title {
    font-size: 32px;
  }
  
  .welcome-card {
    padding: 20px;
  }
  
  .stats-grid {
    grid-template-columns: repeat(2, 1fr);
    gap: 12px;
  }
  
  .stat-value {
    font-size: 22px;
  }
  
  .section-title-text {
    font-size: 16px;
  }
  
  .info-item {
    margin-bottom: 4px;
    
    &.with-border {
      margin-bottom: 4px;
      padding-bottom: 4px;
    }
  }
  
  .info-header {
    padding: 8px 0;
  }
  
  .info-body {
    padding-bottom: 12px;
  }
  
  .content-line {
    gap: 8px;
    padding: 2px 0;
  }
  
  .content-icon {
    font-size: 16px;
    min-width: 20px;
  }
  
  .content-text {
    font-size: 14px;
  }
}
</style>
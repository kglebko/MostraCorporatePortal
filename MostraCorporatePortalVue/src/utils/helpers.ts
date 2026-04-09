import defaultNewsImage from '@/assets/images/news_default.png'

export function formatDate(date: string | Date) {
  return new Date(date).toLocaleDateString('ru-RU', {
    day: 'numeric',
    month: 'long',
    year: 'numeric'
  })
}

export function getNewsImage(path?: string) {
  return path
    ? `https://localhost:5078/images/news_photo/${path}`
    : defaultNewsImage
}


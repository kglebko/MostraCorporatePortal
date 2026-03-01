# Интеграция авторизации во фронтенде

## Установка зависимостей

```bash
npm install
```

Это установит `oidc-client-ts` для работы с Identity Server.

## Структура сервисов

### 1. `src/services/authService.ts`
Сервис для работы с авторизацией через Duende Identity Server:
- `login()` - перенаправление на страницу входа
- `logout()` - выход из системы
- `handleCallback()` - обработка callback после авторизации
- `getAccessToken()` - получение access token
- `isAuthenticated()` - проверка авторизации
- `getUser()` - получение информации о пользователе

### 2. `src/services/apiService.ts`
Сервис для работы с API:
- Автоматически добавляет Bearer token к каждому запросу
- Обрабатывает ошибки 401 (перенаправляет на логин)
- Методы: `get()`, `post()`, `put()`, `delete()`, `patch()`

### 3. `src/services/collaboratorsService.ts`
Пример сервиса для работы с сотрудниками через API.

### 4. `src/stores/auth.ts`
Pinia store для управления состоянием авторизации:
- `user` - текущий пользователь
- `isAuthenticated` - computed свойство для проверки авторизации
- `userClaims` - claims пользователя
- `login()`, `logout()`, `loadUser()` - методы для работы с авторизацией

## Использование

### В компонентах Vue:

```vue
<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
import { collaboratorsService } from '@/services/collaboratorsService'

const authStore = useAuthStore()

// Проверка авторизации
if (authStore.isAuthenticated) {
  console.log('Пользователь авторизован')
  console.log('Имя:', authStore.userClaims?.fullName)
}

// Выход
async function handleLogout() {
  await authStore.logout()
}

// Работа с API
async function loadCollaborators() {
  try {
    const collaborators = await collaboratorsService.getAll()
    console.log(collaborators)
  } catch (error) {
    console.error('Ошибка загрузки сотрудников:', error)
  }
}
</script>
```

## Защита маршрутов

Все маршруты защищены через `router.beforeEach`. Если пользователь не авторизован, он автоматически перенаправляется на страницу входа Identity Server.

## Callback страница

После успешной авторизации пользователь перенаправляется на `/callback`, где обрабатывается результат авторизации и происходит редирект на главную страницу.

## Конфигурация

Настройки Identity Server находятся в `src/services/authService.ts`:
- `authority`: `https://localhost:5001` - адрес Identity Server
- `client_id`: `corporate-portal-bff`
- `redirect_uri`: автоматически определяется из `window.location.origin`

API endpoint настраивается в `src/services/apiService.ts`:
- `API_BASE_URL`: `https://localhost:7001/api`


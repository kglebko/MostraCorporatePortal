# Настройка авторизации через Duende Identity Server с BFF паттерном

## Структура проектов

1. **CorporatePortal.AuthServer** - Сервер авторизации на базе Duende Identity Server и ASP.NET Core Identity
2. **CorporatePortal.Api** - API с BFF (Backend for Frontend) паттерном
3. **MonstraCorporatePortal** - Основной проект (можно использовать для миграций старой схемы)

## Установка и настройка

### 1. Восстановление пакетов NuGet

Выполните в корне решения:
```bash
dotnet restore
```

### 2. Создание миграций для AuthServer

```bash
cd CorporatePortal.AuthServer
dotnet ef migrations add InitialIdentityServerMigration --context ApplicationDbContext
dotnet ef database update
```

### 3. Настройка базы данных

Убедитесь, что строка подключения в `appsettings.json` правильная:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=monstraportal_db;Username=postgres;Password=1234567899"
  }
}
```

### 4. Запуск проектов

#### AuthServer (порт 5000/5001):
```bash
cd CorporatePortal.AuthServer
dotnet run
```

#### Api (порт 7000/7001):
```bash
cd CorporatePortal.Api
dotnet run
```

## Тестовый пользователь

При первом запуске AuthServer автоматически создается тестовый пользователь:
- **Логин**: `k_glebko`
- **Пароль**: `12345678`

## Настройка фронтенда (Vue.js)

Для интеграции с фронтендом используйте библиотеку `oidc-client` или `@auth0/auth0-spa-js`.

Пример конфигурации:
```javascript
import { UserManager } from 'oidc-client';

const userManager = new UserManager({
  authority: 'https://localhost:5001',
  client_id: 'corporate-portal-bff',
  redirect_uri: 'http://localhost:5173/callback',
  response_type: 'code',
  scope: 'openid profile email api',
  post_logout_redirect_uri: 'http://localhost:5173/',
  automaticSilentRenew: true,
  filterProtocolClaims: true,
  loadUserInfo: true
});
```

## Важные замечания

1. **Модель Collaborator** теперь наследуется от `IdentityUser` и хранится в таблице `AspNetUsers`
2. **Поле Photo** добавлено в модель Collaborator
3. **BFF паттерн** обеспечивает безопасную работу с токенами на сервере
4. Все API endpoints в CorporatePortal.Api требуют авторизации

## Порты по умолчанию

- AuthServer: `https://localhost:5001` / `http://localhost:5000`
- Api: `https://localhost:7001` / `http://localhost:7000`
- Vue App: `http://localhost:5173`

## Совместимость пакетов

Все пакеты совместимы с .NET 8.0:
- Duende.IdentityServer: 7.0.0
- Duende.BFF: 2.0.0
- Microsoft.AspNetCore.Identity.EntityFrameworkCore: 8.0.2
- Npgsql.EntityFrameworkCore.PostgreSQL: 8.0.2


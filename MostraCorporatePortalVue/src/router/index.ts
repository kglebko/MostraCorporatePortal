import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const routes = [
  { 
    path: '/callback', 
    name: 'callback', 
    component: () => import('@/views/Callback.vue') 
  },
  { 
    path: '/', 
    name: 'home', 
    component: () => import('@/views/Home.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/corporate-life',
    name: 'corporateLife',
    component: () => import('@/views/corporate-life/CorporateLife.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/corporate-life/:id',
    name: 'corporateLifeItem',
    component: () => import('@/views/corporate-life/CorporateLifeItem.vue'),
    meta: { requiresAuth: true }
  },
  { 
    path: '/training', 
    name: 'training', 
    component: () => import('@/views/training/Training.vue'),
    meta: { requiresAuth: true }
  },
  { 
    path: '/events', 
    name: 'events', 
    component: () => import('@/views/events/Events.vue'),
    meta: { requiresAuth: true }
  },
  { 
    path: '/for-new', 
    name: 'forNew', 
    component: () => import('@/views/for-new/ForNew.vue'),
    meta: { requiresAuth: true }
  },
  { 
    path: '/employees', 
    name: 'employees', 
    component: () => import('@/views/employees/EmployeesList.vue'),
    meta: { requiresAuth: true }
  },
  { 
    path: '/employees/:id', 
    name: 'employeeDetails', 
    component: () => import('@/views/employees/EmployeeDetails.vue'),
    meta: { requiresAuth: true }
  },
  { 
    path: '/departments/:id', 
    name: 'departmentDetails', 
    component: () => import('@/views/departments/DepartmentDetails.vue'),
    meta: { requiresAuth: true }
  },
  { 
    path: '/organizations/:id', 
    name: 'organizationDetails', 
    component: () => import('@/views/organizations/OrganizationDetails.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/logout-callback',
    name: 'logoutCallback',
    component: () => import('@/views/LogoutCallbackView.vue')
  }
  ]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// Защита маршрутов
router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore()
  
  // Загружаем пользователя если еще не загружен
  if (!authStore.user) {
    await authStore.loadUser()
  }
  
  // Если маршрут требует авторизации
  if (to.meta.requiresAuth) {
    if (!authStore.isAuthenticated) {
      // Пользователь не авторизован - перенаправляем на логин
      await authStore.login()
      return
    }
  }
  
  next()
})

export default router

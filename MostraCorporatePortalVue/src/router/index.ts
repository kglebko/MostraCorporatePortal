import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const routes = [
  { path: '/callback', name: 'callback', component: () => import('@/views/Callback.vue') },
  { path: '/', name: 'home', component: () => import('@/views/Home.vue'), meta: { requiresAuth: true } },
  { path: '/training', name: 'training', component: () => import('@/views/training/Training.vue'), meta: { requiresAuth: true } },
  { path: '/events', name: 'events', component: () => import('@/views/events/Events.vue'), meta: { requiresAuth: true } },
  { path: '/for-new', name: 'forNew', component: () => import('@/views/for-new/ForNew.vue'), meta: { requiresAuth: true } },
  { path: '/employees', name: 'employees', component: () => import('@/views/employees/EmployeesList.vue'), meta: { requiresAuth: true } },
  { path: '/employees/:id', name: 'employeeDetails', component: () => import('@/views/employees/EmployeeDetails.vue'), meta: { requiresAuth: true } },
  { path: '/profile/:id', name: 'profileDetails', component: () => import('@/views/employees/ProfileDetails.vue'), meta: { requiresAuth: true } },
  { path: '/departments/:id', name: 'departmentDetails', component: () => import('@/views/departments/DepartmentDetails.vue'), meta: { requiresAuth: true } },
  { path: '/organizations/:id', name: 'organizationDetails', component: () => import('@/views/organizations/OrganizationDetails.vue'), meta: { requiresAuth: true } },
  { path: '/logout-callback', name: 'logoutCallback', component: () => import('@/views/LogoutCallbackView.vue') },
  { path: '/news', name: 'newsList', component: () => import('@/views/news/NewsList.vue'), meta: { requiresAuth: true } },
  { path: '/news/:id', name: 'newsItem', component: () => import('@/views/news/NewsItem.vue'), meta: { requiresAuth: true } },
  { path: '/chats', name: 'chats', component: () => import('@/views/chats/ChatsPage.vue'), meta: { requiresAuth: true } },
  { path: '/404', name: 'notFound', component: () => import('@/views/errors/NotFound.vue') },
  { path: '/:pathMatch(.*)*', redirect: '/404' }
]

const router = createRouter({
  history: createWebHistory(),
  routes,

  scrollBehavior() {
    return {
      top: 0,
      behavior: 'smooth'
    }
  }
})

router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore()

  if (!authStore.authReady) {
    await authStore.init()
  }

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    return authStore.login()
  }

  next()
})

router.onError(() => {
  router.push({ name: 'notFound' })
})

export default router
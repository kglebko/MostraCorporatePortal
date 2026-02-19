

import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  { path: '/', 
    name: 'home', 
    component: () => import('@/views/Home.vue') },

  {
    path: '/corporate-life',
    name: 'corporateLife',
    component: () => import('@/views/corporate-life/CorporateLife.vue')
  },
  
  {
    path: '/corporate-life/:id',
    name: 'corporateLifeItem',
    component: () => import('@/views/corporate-life/CorporateLifeItem.vue')
  },

  { path: '/training', 
    name: 'training', 
    component: () => import('@/views/training/Training.vue') },

  { path: '/events', 
    name: 'events', 
    component: () => import('@/views/events/Events.vue') },

  { path: '/for-new', 
    name: 'forNew', 
    component: () => import('@/views/for-new/ForNew.vue') },

  { path: '/employees', 
    name: 'employees', 
    component: () => import('@/views/employees/EmployeesList.vue') },

  { path: '/employees/:id', 
    name: 'employeeDetails', 
    component: () => import('@/views/employees/EmployeeDetails.vue') },

  { path: '/departments/:id', 
    name: 'departmentDetails', 
    component: () => import('@/views/departments/DepartmentDetails.vue') },

  { path: '/organizations/:id', 
    name: 'organizationDetails', 
    component: () => import('@/views/organizations/OrganizationDetails.vue') }
]

export default createRouter({
  history: createWebHistory(),
  routes
})

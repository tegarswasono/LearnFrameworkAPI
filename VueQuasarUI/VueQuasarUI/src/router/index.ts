import { createRouter, createWebHistory } from 'vue-router'
import AppLayout from '../layouts/AppLayout.vue'
import PublicLayout from '../layouts/PublicLayout.vue'

//System
import LoginIndexView from '../views/system/login/login-index.vue'
import forgotPassword from '@/views/system/login/forgot-password.vue'

//Booking
import BookingIndexView from '../views/transaction/booking/booking-index.vue'

//Master
import ProductIndexView from '../views/master/product/product-index.vue'
import CategoryIndexView from '../views/master/category/category-index.vue'

//Configuration
import UserIndexView from '../views/configuration/user/user-index.vue'
import RoleIndexView from '../views/configuration/role/role-index.vue'
import SystemConfigurationIndexView from '../views/configuration/system-configuration/system-configuration-index.vue'
import SmtpSettingIndexView from '../views/configuration/smtp-setting/smtp-setting-index.vue'
import MyProfileIndexView from '../views/configuration/my-profile/my-profile-index.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'loginindex',
      component: LoginIndexView
    },
    {
      path: '/forgotPassword',
      name: 'forgotpassword',
      component: forgotPassword
    },
    {
      path: '/booking',
      component: AppLayout,
      children: [
        {
          path: '',
          name: 'bookingindex',
          component: BookingIndexView
        }
      ]
    },
    {
      path: '/master',
      component: AppLayout,
      children: [
        {
          path: 'product',
          name: 'productindex',
          component: ProductIndexView
        },
        {
          path: 'category',
          name: 'categoryindex',
          component: CategoryIndexView
        }
      ]
    },
    {
      path: '/configuration',
      component: AppLayout,
      children: [
        {
          path: 'user',
          name: 'userindex',
          component: UserIndexView
        },
        {
          path: 'role',
          name: 'roleindex',
          component: RoleIndexView
        },
        {
          path: 'systemconfiguration',
          name: 'systemconfigurationindex',
          component: SystemConfigurationIndexView
        },
        {
          path: 'smtpsetting',
          name: 'smtpsettingindex',
          component: SmtpSettingIndexView
        },
        {
          path: 'myprofile',
          name: 'myprofileindex',
          component: MyProfileIndexView
        }
      ]
    }
  ]
})

export default router

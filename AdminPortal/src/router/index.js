import Vue from 'vue'
import VueRouter from 'vue-router'
import AdminPage from '../components/AdminPage.vue'
import SignIn from '../components/SignIn.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'SignIn',
    component: SignIn
  },
  {
    path: '/AdminPage',
    name: 'AdminPage',
    // component: () => import('../components/AdminPage.vue')
    component: AdminPage
  },

]

const router = new VueRouter({
  routes
})

export default router

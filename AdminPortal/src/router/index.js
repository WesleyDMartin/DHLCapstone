import Vue from 'vue'
import VueRouter from 'vue-router'
import AdminPage from '../components/AdminPage.vue'
import SignIn from '../components/SignIn.vue'
import ProjectPage from '../components/ProjectPage/index.vue'
import SplashPage from '../components/SplashPage.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/SignIn',
    name: 'SignIn',
    component: SignIn
  },
  {
    path: '/AdminPage',
    name: 'AdminPage',
    // component: () => import('../components/AdminPage.vue')
    component: AdminPage
  },
  {
    path: '/ProjectPage',
    name: 'ProjectPage',
    // component: () => import('../components/AdminPage.vue')
    component: ProjectPage
  },
  {
    path: '/',
    name: 'SplashPage',
    // component: () => import('../components/AdminPage.vue')
    component: SplashPage
  },

]

const router = new VueRouter({
  routes
})

export default router

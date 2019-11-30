import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Product from '../views/Product.vue'
import ShopBasket from '../views/ShopBasket.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'home',
    component: Home
  },
  {
    path: '/Product/:id',
    name: 'Product',
    component: Product
  },
  {
    path: '/ShopBasket',
    name: 'ShopBasket',
    component: ShopBasket
  },
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router

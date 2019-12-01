import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)
function updateLocalStoreage(data) {
  localStorage.setItem('basket', JSON.stringify(data))
}
let data = localStorage.getItem('basket')
if (data && JSON.parse(data)) {
  data = JSON.parse(data)
} else {
   data = []
}
// JSON.parse(localStorage.getItem('basket'))

export default new Vuex.Store({
  state: {
    basket: data
  },
  getters: {
    getBasket (state) {
        return state.basket
    },
    isInBasket: state => id => {
     return  !!state.basket.find(item => item.id === id)
    }
  },
  mutations: {
    addToBasket(state, data) {
      let product = state.basket.find(item => item.id === data.id)
      if (product) {
        if ((product.basketQuantity + 1) <= product.quantity) {
          product.basketQuantity =  product.basketQuantity + 1
        }
      } else {
        data.basketQuantity = 1
        state.basket.push(data)
      }
      updateLocalStoreage(state.basket)
    },
    updateQuantity(state, id,) {
      let item =state.basket.find(item => item.id === id)
      if (item) {
        item.basketQuantity =  item.basketQuantity + 1
      }
      updateLocalStoreage(state.basket)
    },
    setBasket(state,data) {
      state.basket = data
      updateLocalStoreage(state.basket)
    }
  },
  actions: {
  }
})

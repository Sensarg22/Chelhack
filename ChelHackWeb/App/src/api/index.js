import Api from './fetch.js'

const routeUrl = 'api/account/'
const goodsUrl = '/Goods'
const categoriesUrl = '/Categories'
const brandsUrl = '/Brands'
const basketUrl = '/Basket'

export default {
    getGoods: (params) => {
        return Api.get(goodsUrl, {
            params: params
        })
    },
    getCategories: () => {
      return Api.get(categoriesUrl)
    },
    getBrands: () => {
        return Api.get(brandsUrl)
    },
    getProduct: (id) => {
        return Api.get(goodsUrl + '/' + id)
    },
    order: (ids) => {
        return Api.post(basketUrl + '/submit', ids)
    }
}
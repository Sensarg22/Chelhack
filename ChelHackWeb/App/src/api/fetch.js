import axios from 'axios'
const service = axios.create({
    baseURL: (window.location.host === 'localhost:8080') ? 'http://localhost:5000' : window.location.origin,
})
service.interceptors.request.use(function (config) {
    return config
}, function (error) {
    return Promise.reject(error)
})
// respone
service.interceptors.response.use(
    response => {
        return response.data
    }, error => {
        console.error(error)
        return error
    }
)

export default service

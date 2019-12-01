<template>
    <!--    <div class="catalog">-->
    <!--        <div class="catalog__container">-->
    <!--            <aside class="aside">-->
    <!--                <div class="aside__subtitle">-->
    <!--                    Категории-->
    <!--                </div>-->
    <!--                <div class="aside__nav">-->
    <!--                    <ul>-->
    <!--                        <li>cat 1</li>-->
    <!--                        <li>cat 2</li>-->
    <!--                        <li>cat 3</li>-->
    <!--                        <li>cat 4</li>-->
    <!--                    </ul>-->
    <!--                </div>-->
    <!--                <div class="aside__subtitle">-->
    <!--                    Фильтры-->
    <!--                </div>-->
    <!--                <div class="aside__nav">-->
    <!--                    <ul>-->
    <!--                        <li>f 1</li>-->
    <!--                        <li>f 2</li>-->
    <!--                        <li>f 3</li>-->
    <!--                        <li>f 4</li>-->
    <!--                    </ul>-->
    <!--                </div>-->
    <!--            </aside>-->
    <!--            <section class="section">-->
    <!--                <div class="goods">-->
    <!--                    <div class="goods__item" v-for="item in Array(6)">-->
    <!--                        <div class="product-card" >-->
    <!--                                <a href="#" class="product-card__thumb ">-->
    <!--                                      <img src="../assets/img/iphone.jpg" alt="">-->
    <!--                                </a>-->
    <!--                                <div class="product-card__title">-->
    <!--                                  <a href="#">Apple iPhone 8 Special Edition 64GB</a>-->
    <!--                                </div>-->
    <!--                                <div class="product-card__prices">53 990 ₽</div>-->
    <!--                                <a href="#" class="product-card__add">-->
    <!--                                   <button>В корзину</button>-->
    <!--                                </a>-->
    <!--                        </div>-->
    <!--                    </div>-->
    <!--                </div>-->
    <!--            </section>-->
    <!--        </div>-->
    <!--    </div>-->


    <div class="slider slider-tovar" >
        <div class="wrapper" v-loading.fullscreen.lock="loading">
            <aside class="aside">
                <div class="aside__subtitle">
                    Брэнды
                </div>
                <div class="aside__nav">
                    <ul>
                        <li v-for="item in brands">{{item}}</li>
                    </ul>
                </div>
                <!--<div class="aside__subtitle">
                    Фильтры
                </div>
                <div class="aside__nav">
                    <ul>
                        <li>f 1</li>
                        <li>f 2</li>
                        <li>f 3</li>
                        <li>f 4</li>
                    </ul>
                </div>-->
            </aside>
            <div class="slider-cont">
                <!--                <div class="title">-->
                <!--                    <span>Наши товары</span>-->
                <!--                </div>-->
                <el-input v-model="form.textFilter" :clearable="true" @change="getGoods" class="m-b-20" :placeholder="'Поиск'">

                </el-input>
                <div class="slider__block">
                    <div class="slider__block-list">

                        <router-link  :key="key" class="slider-block__item" href="#" v-for="(item,key) in list" :to="{name:'Product', params: {id: item.id}}">
                            <figure class="slider-block__item-img">
                                <img :src="item.imageUrl"/></figure>
                            <div class="slider-block__item-info">
                                <div class="slider-block__info-title">{{item.title}}
                                </div>
                                <div class="slider-block__info-dop">{{item.brand}}</div>
                                <div class="slider-block__info-price">{{item.price}} Р</div>
                                <div class="slider-block__info-price--sale">{{item.finalPrice}}</div>
                                <div class="slider-block__info-btn ico-cart"></div>
                            </div>
                        </router-link>
                        <!--<a class="slider-block__item" href="#">
                            <figure class="slider-block__item-img"><img src="../assets/img/photos/2.jpg"/></figure>
                            <div class="slider-block__item-info">
                                <div class="slider-block__info-title">UMA i7-7500U 8GB 1040 /1EP98EA/</div>
                                <div class="slider-block__info-dop">HP</div>
                                <div class="slider-block__info-price">109 990 Р</div>
                                <div class="slider-block__info-price&#45;&#45;sale">120 990 Р</div>
                                <div class="slider-block__info-btn ico-cart"></div>
                            </div>
                        </a>
                        <a class="slider-block__item" href="#">
                            <figure class="slider-block__item-img"><img src="../assets/img/photos/3.jpg"/></figure>
                            <div class="slider-block__item-info">
                                <div class="slider-block__info-title">Игровой ноутбук Omen 15-ce016ur /2CR89EA/ intel i7
                                    7700HQ/8Gb/1Tb/15.6FHD IPS/GTX 1060 6Gb/Win10 Shadow black
                                </div>
                                <div class="slider-block__info-dop">HP</div>
                                <div class="slider-block__info-price">100 990 Р</div>
                                <div class="slider-block__info-price&#45;&#45;sale">120 990 Р</div>
                                <div class="slider-block__info-btn ico-cart"></div>
                            </div>
                        </a><a class="slider-block__item" href="#">
                        <figure class="slider-block__item-img"><img src="../assets/img/photos/4.jpg"/></figure>
                        <div class="slider-block__item-info">
                            <div class="slider-block__info-title">Игровой ноутбук 15-dp0097ur /5AS66EA/ intel i5
                                8300H/8Gb/1Tb+128Gb SSD/15.6FHD IPS/DVDRW/GTX 1060 3Gb/Win10
                            </div>
                            <div class="slider-block__info-dop">HP</div>
                            <div class="slider-block__info-price">79 990 Р</div>
                            <div class="slider-block__info-price&#45;&#45;sale">87990 Р</div>
                            <div class="slider-block__info-btn ico-cart"></div>
                        </div>
                    </a><a class="slider-block__item" href="#">
                        <figure class="slider-block__item-img"><img src="../assets/img/photos/5.jpg"/></figure>
                        <div class="slider-block__item-info">
                            <div class="slider-block__info-title">Игровой ноутбук GL63 intel i5
                                9300H/8Gb/256Gb/15.6FHD/GTX165 черный
                            </div>
                            <div class="slider-block__info-dop">MSI</div>
                            <div class="slider-block__info-price">74 990 Р</div>
                            <div class="slider-block__info-price&#45;&#45;sale">83 500 Р</div>
                            <div class="slider-block__info-btn ico-cart"></div>
                        </div>
                    </a><a class="slider-block__item" href="#">
                        <figure class="slider-block__item-img"><img src="../assets/img/photos/6.jpg"/></figure>
                        <div class="slider-block__item-info">
                            <div class="slider-block__info-title">Игровой ноутбук GF63 8RC-046RU /9S7-16R112-046/
                                intel i5 8300H/8Gb/1Tb/15.6FHD IPS/NV GTX1050 4G/Win10
                            </div>
                            <div class="slider-block__info-dop">MSI</div>
                            <div class="slider-block__info-price">69 990 Р</div>
                            <div class="slider-block__info-price&#45;&#45;sale">85 300 Р</div>
                            <div class="slider-block__info-btn ico-cart"></div>
                        </div>
                    </a><a class="slider-block__item" href="#">
                        <figure class="slider-block__item-img"><img src="../assets/img/photos/7.jpg"/></figure>
                        <div class="slider-block__item-info">
                            <div class="slider-block__info-title">Игровой ноутбук FX705GD-EW070T /90NR0112-M02970/
                                intel i5 8300H/8Gb/1Tb/17.3FHD IPS/GTX1050 4Gb/Win10 Black
                            </div>
                            <div class="slider-block__info-dop">ASUS</div>
                            <div class="slider-block__info-price">69 990 Р</div>
                            <div class="slider-block__info-price&#45;&#45;sale">85 300 Р</div>
                            <div class="slider-block__info-btn ico-cart"></div>
                        </div>
                    </a><a class="slider-block__item" href="#">
                        <figure class="slider-block__item-img"><img src="../assets/img/photos/8.jpg"/></figure>
                        <div class="slider-block__item-info">
                            <div class="slider-block__info-title">Yoga 530-14ARR /81H90006RU/ RYZEN 5
                                2500U/8Gb/256Gb/14FHD IPS Touch/Vega 8/Win10 Onyx Black
                            </div>
                            <div class="slider-block__info-dop">Lenovo</div>
                            <div class="slider-block__info-price">69 990 Р</div>
                            <div class="slider-block__info-price&#45;&#45;sale">85 690 Р</div>
                            <div class="slider-block__info-btn ico-cart"></div>
                        </div>
                    </a><a class="slider-block__item" href="#">
                        <figure class="slider-block__item-img"><img src="../assets/img/photos/9.jpg"/></figure>
                        <div class="slider-block__item-info">
                            <div class="slider-block__info-title">Игровой ноутбук GF63 8RC-046RU /9S7-16R112-046/
                                intel i5 8300H/8Gb/1Tb/15.6FHD IPS/NV GTX1050 4G/Win10
                            </div>
                            <div class="slider-block__info-dop">MSI</div>
                            <div class="slider-block__info-price">69 990 Р</div>
                            <div class="slider-block__info-price&#45;&#45;sale">85 300 Р</div>
                            <div class="slider-block__info-btn ico-cart"></div>
                        </div>
                    </a><a class="slider-block__item" href="#">
                        <figure class="slider-block__item-img"><img src="../assets/img/photos/10.jpg"/></figure>
                        <div class="slider-block__item-info">
                            <div class="slider-block__info-title">Игровой ноутбук FX705GD-EW070T /90NR0112-M02970/
                                intel i5 8300H/8Gb/1Tb/17.3FHD IPS/GTX1050 4Gb/Win10 Black
                            </div>
                            <div class="slider-block__info-dop">ASUS</div>
                            <div class="slider-block__info-price">69 990 Р</div>
                            <div class="slider-block__info-price&#45;&#45;sale">85 300 Р</div>
                            <div class="slider-block__info-btn ico-cart"></div>
                        </div>
                    </a><a class="slider-block__item" href="#">
                        <figure class="slider-block__item-img"><img src="../assets/img/photos/11.jpg"/></figure>
                        <div class="slider-block__item-info">
                            <div class="slider-block__info-title">Игровой ноутбук 15-bc421ur /4GY88EA/ intel i5
                                8300H/8Gb/1Tb/15.6FHD/GTX 1050 2Gb/ Win10 черный
                            </div>
                            <div class="slider-block__info-dop">HP</div>
                            <div class="slider-block__info-price">59 990 Р</div>
                            <div class="slider-block__info-price&#45;&#45;sale">73 530Р</div>
                            <div class="slider-block__info-btn ico-cart"></div>
                        </div>
                    </a><a class="slider-block__item" href="#">
                        <figure class="slider-block__item-img"><img src="../assets/img/photos/12.jpg"/></figure>
                        <div class="slider-block__item-info">
                            <div class="slider-block__info-title">Ноутбук 15-da1066ur /6VK42EA/ intel i5
                                8265U/8Gb/1Tb+128Gb SSD/15.6/GeForce MX110 2Gb/Win10 Fog blue
                            </div>
                            <div class="slider-block__info-dop">HP</div>
                            <div class="slider-block__info-price">59 990 Р</div>
                            <div class="slider-block__info-price&#45;&#45;sale">73 530Р</div>
                            <div class="slider-block__info-btn ico-cart"></div>
                        </div>
                    </a>-->
                    </div>
                    <div v-if="loading === false && list.length === 0" class="m-t-40">
                        Ничего не найдено :(
                    </div>
                  <!--  <el-row class="m-t-20">
                        <el-col align="center">
                            <el-pagination
                                    background
                                    layout="prev, pager, next"
                                    :page="form.page">
                            </el-pagination>
                        </el-col>
                    </el-row>-->
                </div>
            </div>
        </div>
    </div>


</template>

<script>
    // @ is an alias to /src
    import Api from '../api'

    export default {
        name: 'home',
        data() {
            return {
                loading: false,
                list: [],
                brands: [],
                categories: [],
                form: {
                    page: 1,
                    pageSize: 10,
                    sortField: '',
                    sortOrder: '',
                    textFilter: ''
                }
            }
        },
        methods: {
            getCategories() {
                this.loading = true
                Api.getCategories().then(res => {
                    this.categories = res
                    this.loading = false
                }).catch(() => {
                    this.loading = false
                })
            },
            getBrands() {
                // this.loading = true
                Api.getBrands().then(res => {
                    this.brands = res
                    // this.loading = false
                }).catch(() => {
                    // this.loading = false
                })
            },
            getGoods() {
                this.loading = true
                Api.getGoods(this.form).then(res => {
                    this.list = res
                    this.loading = false
                }).catch(() => {
                    this.loading = false
                })
            }
        },
        created() {
            this.getBrands()
            this.getGoods()
            this.getCategories()
            // this.loading = true
        }
    }
</script>

<template>
    <div class="product" v-loading="loading">

        <el-main v-if="data">
            <router-link :to="{name: 'home'}" class="m-b-40 back-url">
               <i class="el-icon-back"></i> Обратно к каталогу
            </router-link>


        <el-row :gutter="40">
            <el-col  :md="8" :xs="24">
                <div class="product__image">
                    <el-image
                            style="width: 100%;"
                            :src="data.imageUrl"
                            :fit="'cover'"></el-image>
                </div>
            </el-col>
            <el-col :md="10" :xs="24">
                <h1 class="product__title">
                    {{data.title}}
                </h1>
                <div class="product__price">
                    <el-row :gutter="20">
                        <el-col :span="12">
                            {{data.finalPrice}} ₽
                        </el-col>
                        <el-col :span="12">
                            <el-button type="primary" round @click="add(data)" >Добавить в корзину</el-button>
                        </el-col>

                    </el-row>
                </div>
                <div v-if="isInBasket(data.id)" class="m-t-20">
                    <router-link :to="{name: 'ShopBasket'}"></router-link>
                </div>
                <hr>
                <div v-for="item in data.parameters">
                    <el-row :gutter="20">
                        <el-col :span="12" align="left">
                            {{item.title}}
                        </el-col>
                        <el-col :span="12">
                            {{item.value}}
                        </el-col>
                    </el-row>
                    <hr>
                </div>

            </el-col>
        </el-row>
        </el-main>
    </div>
</template>
<script>
    import Api from '../api'
    import { mapGetters, mapMutations } from 'vuex';
    export default {
        data () {
            return {
                data: null,
                quantity: 1,
                loading: false
            }
        },
        computed: {
            ...mapGetters(['isInBasket'])
        },
        // watch:{
        //     isInBasket (nV) {
        //         if (nV === true) {
        //             quantity
        //         }
        //     }
        // },
        methods: {
            ...mapMutations(['addToBasket']),
            add (data) {
                this.$notify({
                    title: 'Добавлено',
                    message: data.title,
                    type: 'success'
                });
                this.addToBasket(data)
            },
            getData () {
                this.loading = true
                Api.getProduct(this.$route.params.id).then(res => {
                    console.log(res)
                    this.data = res
                    this.loading = false
                }).catch(() => {
                    this.loading = false
                })
            }
        },
        created() {
            this.getData()
        }
    }
</script>
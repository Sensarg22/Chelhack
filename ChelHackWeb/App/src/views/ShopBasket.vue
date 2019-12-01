<template>
    <div class="basket">
        <el-main>
            <div  v-if="list.length">
            <el-table class="m-t-40 m-b-40" :data="list">

                <el-table-column
                        prop="title"
                        label="Изображение"
                        width="140"
                >
                    <template slot-scope="scope">
                        <span style="margin-left: 10px">{{ scope.row.date }}</span>
                        <el-image
                                style="width: 50px; height: 50px"
                                :src="scope.row.imageUrl"
                                :fit="'contain'"></el-image>
                    </template>
                </el-table-column>
                <el-table-column
                        prop="title"
                        label="Название"
                >
                </el-table-column>
                <el-table-column
                        prop="finalPrice"
                        label="Цена"
                        width="180">
                </el-table-column>
                <el-table-column
                        prop="title"
                        label="Количество"
                        width="250"
                >
                    <template slot-scope="scope">
                        <el-input-number v-model="list[scope.$index].basketQuantity" @change="changeQuantity" :min="1"
                                         :max="scope.row.quantity"></el-input-number>
                    </template>
                </el-table-column>

                <el-table-column
                        prop="title"
                        label="Удалить"
                        width="100"
                >
                    <template slot-scope="scope">
                        <el-button type="danger" icon="el-icon-delete" circle
                                   @click="deleteItem(scope.$index)"></el-button>
                    </template>
                </el-table-column>
            </el-table>
            <el-row>
                <el-col :md="12" :xs="24">
                 <div class="cup" @click="clear">  Очистить корзину</div>
                </el-col>
                <el-col align="right" :md="12" :xs="24">
                    Цена к оплате: <h2> {{totalPrice}} ₽</h2>
                </el-col>
            </el-row>

            <el-row :gutter="20" class="m-t-40">
                <el-col>
                    <el-input v-model="phone" placeholder="Телефон"></el-input>
                </el-col>
            </el-row>
            <el-row :gutter="20" class="m-t-20">
                <el-col>
                    <el-input v-model="address" placeholder="Адресс"></el-input>
                </el-col>
            </el-row>
            <el-row :gutter="20" class="m-t-20">
                <el-col>
                    <el-button @click="order">Оформить</el-button>
                </el-col>

            </el-row>
            </div>
            <div v-else>
                Корзина пуста :(
            </div>
        </el-main>

    </div>
</template>
<script>
    import Api from '../api'
    import {mapGetters, mapMutations} from 'vuex';

    export default {
        data() {
            return {
                loading: false,
                list: [],
                totalPrice: 0,
                phone: '',
                address: ''
            }
        },
        computed: {

        },
        methods: {
            ...mapGetters([
                'getBasket',
            ]),
            ...mapMutations(['setBasket']),
            clear () {
                this.list = []
                this.total()
                this.setBasket(this.list)
            },
            total() {
                let total = 0
                if (this.list.length) {
                    this.list.reduce((a, item) => {
                        total = total + item.finalPrice * item.basketQuantity
                    }, total)
                }
                this.totalPrice = total
            },
            changeQuantity(value) {
                console.log(value)
                this.setBasket(this.list)
                this.total()
            },
            order () {
                let ids = this.list.map(item => item.id)
                console.log(ids)
                this.loading = true
                Api.order({
                    phone: this.phone,
                    address: this.address,
                    id: ids
                }).then(() => {
                    this.$message({
                        message: 'Ваш заказ принят!',
                        type: 'success'
                    });
                    this.loading = false
                }).catch(() => {
                    this.loading = false
                })
                //TODO
            },
            getData() {
                this.loading = true
                Api.getBasket().then(res => {
                    this.list = res
                    this.loading = false
                }).catch(() => {
                    this.loading = false
                })
            },
            deleteItem(index) {
                // console.log(this.list)
                this.list.splice(index, 1)
                this.setBasket(this.list)
                this.total()
            }
        },
        created() {
            this.list = [...this.getBasket()]
            this.total()
            // this.getData()
            // this.loading = true
        }
    }
</script>
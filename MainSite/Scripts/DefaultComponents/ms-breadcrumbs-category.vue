<template>
    <div class="navHeader">
        <span class="navHeader-item"
                href="#"
                @click.prevent.self="backMainView">Новости</span>
        <span href="#"
                class="navHeader-item"
                v-for="(item, index) in breadcrumbs"
                :key="index"
                v-on:click.prevent.self="clickEvent(item)"
                v-bind:class="CheckLastItem(index)">{{item.name}}</span>
    </div>
</template>

<script>
    import { mapMutations, mapActions, mapState } from 'vuex';

    export default {
        name: 'ms-breadcrumbs-category',
        data(){
            return {
            }
        },
        computed: {
            ...mapState('menu', [
                'breadcrumbs'
            ])
        },
        watch: {
            $route: 'fetchData'
        },
        methods: {
            ...mapActions('menu', [
                'GET_CATEGORIES_BY_BREADCRUMBS',
            ]),
            ...mapMutations('menu', [
                'SET_OR_UPDATE_ACTIVE_CATEGORY'
            ]),
            fetchData() {
                this.getBreadCrumbs();
            },
            clickEvent(item) {
                if (this.$route.params.categoryId !== item.id)
                    this.$router.push({ name: 'categoryList', params: { categoryId: item.id } });
            },
            backMainView() {
                this.SET_OR_UPDATE_ACTIVE_CATEGORY(null);
                this.$router.push('/');
            },
            CheckLastItem(indexItem) {
                if (this.breadcrumbs.length > 0) {
                    if (this.breadcrumbs.length - 1 == indexItem)
                        return 'bold';
                }

                return '';
            },
            getBreadCrumbs() {
                this.GET_CATEGORIES_BY_BREADCRUMBS(this.$route.params.categoryId);
            }
        },
        created() {
            this.getBreadCrumbs();
        }
    }
</script>

<style lang="scss" scoped>
    .navHeader {
        white-space: pre-wrap;
        display:block;
        font-size: 14px;
        line-height: 1;
        &-item {
            color: #9e9e9e;
            display: inline;
            height: 37px;
            cursor: pointer;
            font-size: 14px;

            &:before {
                content: '\E5CC';
                color: #9e9e9e;
                vertical-align: middle;
                display: inline-block;
                font-family: 'Material Icons';
                font-weight: normal;
                font-style: normal;
                font-size: 14px;
                margin: 0 3px 0 3px;
                -webkit-font-smoothing: antialiased;
            }

            &:first-child:before {
                display: none;
            }
        }
    }
</style>
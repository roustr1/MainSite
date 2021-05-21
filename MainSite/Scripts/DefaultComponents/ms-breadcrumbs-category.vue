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
        props: {
            items: {
                type: Array,
                default: () => { return [] }
            }
        },
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
                if (this.items.length > 0) {
                    if (this.items.length - 1 == indexItem)
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
    }

    .navHeader-item {
        display: inline;
        height: 37px;
        cursor: pointer;
    }
</style>
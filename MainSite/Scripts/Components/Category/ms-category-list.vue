<template>
    <div>
        <msBreadCrumbs
            :items="breadcrumbs"
         />
        <div style="display:flex;flex-wrap:wrap;flex-direction:row;justify-content:space-between;">
            <msCategoryItem
                v-for="item in getCategoryChildren"
                :key="item.id"
                :category_item="item"
                style="flex-basis: 50%;" 
            />
        </div>
    </div>
</template>

<script>
    import msCategoryItem from '../Category/ms-category-item.vue';
    import msBreadCrumbs from '../../DefaultComponents/ms-breadcrumbs-category.vue';
    import { mapActions, mapState } from 'vuex';
    export default {
        props: {
            category: {
                type: Object,
                default: () => { return {} }
            }
        },
        data() {
            return {
            }
        },
        components: {
            msCategoryItem,
            msBreadCrumbs
        },
        watch: {
            $route: 'fetchData'
        },
        computed: {
            ...mapState('menu',[
                'breadcrumbs',
                'categories'
            ]),
            getCategoryChildren() {
                if (!this.$route.params.categoryId) return [];

                if (Object.keys(this.category).length == 0) {
                    let children = this.getCategoryById(this.$route.params.categoryId);
                    return children;
                }

                return this.category.children;
            }
        },
        methods: {
            ...mapActions('menu', [
                'GET_CATEGORIES_BY_BREADCRUMBS',
            ]),
            fetchData() {
                this.GET_CATEGORIES_BY_BREADCRUMBS(this.$route.params.categoryId);           
            },
            getCategoryById(categoryId) {
                if (!this.categories.length) return [];

                let set = new Set;
                function arrMaper(arr) {
                    arr.forEach(element => {
                        if (element != null) {
                            if (element.children !== []) arrMaper(element.children);
                            set.add(element);
                        }
                    })
                };
                arrMaper(this.categories.slice());
                let categoryes = Array.from(set).find(function (item) {
                    if (item.id == categoryId) return item;
                });

                return Object.keys(categoryes).length == 0 ? [] : categoryes.children; 
            },
        },
        mounted() {
            this.GET_CATEGORIES_BY_BREADCRUMBS(this.$route.params.categoryId); 
        }
    }
</script>

<style>

</style>
<template>
    <div>
        <msBreadCrumbs 
                       :items="BREADCRUMBS"
                       />
        <div style="display:flex;flex-wrap:wrap;flex-direction:row;justify-content:space-between;">
            <msCategoryItem 
                            v-for="item in getCategoryChildren"
                            :key="item.id"
                            :category_item="item"
                            style="flex-basis: 50%;" />
        </div>
    </div>
</template>

<script>
    import msCategoryItem from '../Category/ms-category-item.vue'
    import msBreadCrumbs from '../../DefaultComponents/ms-breadcrumbs-category.vue';
    import { mapActions, mapGetters } from 'vuex';
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
        watch: {
            $route: 'fetchData'
        },
        computed: {
            ...mapGetters([
                'BREADCRUMBS',
                'CATEGORIES'
            ]),
            getCategoryChildren() {
                if (Object.keys(this.category).length == 0) {
                    let children = this.getCategoryById(this.$route.params.categoryId);
                    return children;
                }

                return this.category.children;
            }
        },
        methods: {
            ...mapActions([
                'GET_CATEGORIES_BY_BREADCRUMBS',
            ]),
            fetchData() {
                this.GET_CATEGORIES_BY_BREADCRUMBS(this.$route.params.categoryId);           
            },
            getCategoryById(categoryId) {
                if (!this.CATEGORIES.length) return [];

                let set = new Set;
                function arrMaper(arr) {
                    arr.forEach(element => {
                        if (element != null) {
                            if (element.children !== []) arrMaper(element.children);
                            set.add(element);
                        }
                    })
                };
                arrMaper(this.CATEGORIES.slice());
                let categoryes = Array.from(set).find(function (item) {
                    if (item.id == categoryId) return item;
                });

                return Object.keys(categoryes).length == 0 ? [] : categoryes.children; 
            },
        },
        components: {
            msCategoryItem,
            msBreadCrumbs
        },
        mounted() {
            if (!this.BREADCRUMBS.length) {
                this.GET_CATEGORIES_BY_BREADCRUMBS(this.$route.params.categoryId); 
            }
        }
    }
</script>

<style>

</style>
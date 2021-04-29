<template>
    <div class="row">
        <msBreadCrumbs
            :items="breadcrumbs"
         />
        <div class="row flexCategoryList">
            <msCategoryItem
                v-for="item in getCategoryChildren"
                :key="item.id"
                :category_item="item"
            >
            </msCategoryItem>
            <div 
                class="card-panel ms-category-add_item" 
                @click="createCategory"
            >
                <h6 class="text-center">
                    Создать новый подраздел...
                </h6>
                <div class="text-center">
                    <i class="material-icons large">
                        note_add
                    </i>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import msCategoryItem from '../Category/ms-category-item.vue';
    import msBreadCrumbs from '../../DefaultComponents/ms-breadcrumbs-category.vue';
    import msCategoryAddItem from '../Category/ms-category-item_create.vue';
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
            msBreadCrumbs,
            msCategoryAddItem
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
            createCategory() {
                let parentCategoryName = this.breadcrumbs && this.breadcrumbs.length > 0 ?
                    this.breadcrumbs[this.breadcrumbs.length - 1].name
                    :
                    ""
                ;

                this.$router.push({ name: 'createCategory', params: { parentCategoryId: this.$route.params.categoryId, parentCategoryName: parentCategoryName } });
            }
        },
        mounted() {
            this.GET_CATEGORIES_BY_BREADCRUMBS(this.$route.params.categoryId); 
        }
    }
</script>

<style scoped lang="scss">
    .ms-category-add_item {
        cursor: pointer;
        margin: 10px 10px;
        width: 47%;
        height:240px;
        display: flex;
        flex-direction: column;
        justify-content: center;
    }

    .flexCategoryList {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        margin: 0 -10px;
        align-items: stretch;
    }
</style>
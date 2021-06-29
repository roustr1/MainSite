<template>
    <div class="row" style="margin-top:10px;">
        <msBreadCrumbs/>
        <div class="flexCategoryList">
            <msCategoryItem
                v-for="item in getCategoryChildren"
                :key="item.Id"
                :category_item="item"
            />
            <div 
                class="card-panel ms-category-add_item" 
                @click="createCategory"
                v-if="isEditer"
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
                isEditer : false
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
                let children = [];

                if (Object.keys(this.category).length == 0) {
                    children = this.getCategoryById(this.$route.params.categoryId);
                }
                else {
                    children = this.category.Children;
                }

                return children;
            }
        },
        methods: {
            ...mapActions('menu', [
                'GET_CATEGORIES_BY_BREADCRUMBS',
            ]),
            ...mapActions('user', ['GET_PERMISSION_BY_CATEGORY']),
            fetchData() {
                this.GET_CATEGORIES_BY_BREADCRUMBS(this.$route.params.categoryId);           
            },
            getCategoryById(categoryId) {
                if (!this.categories.length) return [];

                let set = new Set;
                function arrMaper(arr) {
                    arr.forEach(element => {
                        if (element != null) {
                            if (element.Children !== []) arrMaper(element.Children);
                            set.add(element);
                        }
                    })
                };
                arrMaper(this.categories.slice());
                let categoryes = Array.from(set).find(function (item) {
                    if (item.Id == categoryId) return item;
                });

                return Object.keys(categoryes).length == 0 ? [] : categoryes.Children; 
            },
            createCategory() {
                let parentCategoryName = this.breadcrumbs && this.breadcrumbs.length > 0 ?
                    this.breadcrumbs[this.breadcrumbs.length - 1].name
                    :
                    ""
                ;

                this.$router.push({ name: 'createCategory', params: { parentCategoryId: this.$route.params.categoryId, parentCategoryName: parentCategoryName } });
            },
            async getInfoByPermission() {
                this.isEditer = await this.GET_PERMISSION_BY_CATEGORY(this.$route.params.categoryId);
            },
        },
        mounted() {
            this.GET_CATEGORIES_BY_BREADCRUMBS(this.$route.params.categoryId); 
            this.getInfoByPermission();
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
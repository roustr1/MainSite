<template>
    <div>
        <ms-birthday v-if="IsNews" />
        <div id="newsComponent">
            <msBreadCrumbs
                   v-if="!IsNews"
                   :items="BREADCRUMBS"
                   />
            <msCreaterNewsItem 
                               :categoryId="$route.params.categoryId" 
                               @addNew="addNew"
                               />
            <msNewsItem v-for="item in newsByCategory.news"
                :key="item.id"
                :news_item="item" />
            <msPage
            v-show="AreViewPageList"
            :pageIndex="newsByCategory.pager.PageIndex"
            :currentPage="CurrentPage"
            :totalPages="newsByCategory.pager.TotalPages"
            :individualPagesDisplayedCount="newsByCategory.pager.IndividualPagesDisplayedCount"
            :list="newsByCategory.pager.ViewPageList"
            @changePage="changePage"
            />
        </div>
    </div>
</template>

<script>
    import msNewsItem from './ms-news-item.vue';
    import msCreaterNewsItem from './ms-creater_news-item.vue';
    import msPage from '../../DefaultComponents/ms-page.vue';
    import { mapActions, mapGetters } from 'vuex';
    import msBirthday from '../Birthday/ms-birthday.vue';
    import msBreadCrumbs from '../../DefaultComponents/ms-breadcrumbs-category.vue';

    export default {
        name: "ms-news",

        props: {
            newsByCategory: {
                type: Object,
                default: () => {
                    return {
                        news: new Array(),
                        pager: {}
                    }
                }
            }
        },
        computed: {
            ...mapGetters([
                'BREADCRUMBS'
            ]),
            AreViewPageList() {
                if (this.newsByCategory.pager != undefined) {
                    if (this.newsByCategory.pager.ViewPageList != undefined) {
                        if (this.newsByCategory.pager.ViewPageList.length) {
                            return true;
                        }
                    }
                }

                return false;
            },
            IsNews() {
                return typeof (this.$route.params.categoryId) === 'undefined';
            },
            CurrentPage() {
                return this.$route.params.page;
            }
        },
        components: {
            msNewsItem,
            msCreaterNewsItem,
            msPage,
            msBirthday,
            msBreadCrumbs
        },
        watch: {
            $route: 'fetchData'
        },
        methods: {
            ...mapActions([
                'GET_NEWS',
                'GET_CATEGORIES_BY_BREADCRUMBS'
            ]),
            fetchData() {
                this.getNewsForCategory();
                if (!this.IsNews) {
                    this.GET_CATEGORIES_BY_BREADCRUMBS(this.$route.params.categoryId);
                }
            },
            getNewsForCategory() {
                this.GET_NEWS(
                    {
                        page: this.$route.params.page,
                        categoryId: this.$route.params.categoryId
                    }
                ).then(responce => {
                    this.newsByCategory.news = responce.News;
                    this.newsByCategory.pager = responce.PagerModel;
                });
            },
            changePage(new_page) {
                if (new_page !== this.$route.params.page) {
                    this.$route.params.page = new_page;
                    let routerParams = typeof (this.$route.params.categoryId) === 'undefined' ?
                    { name: 'news', params: { page: this.$route.params.page } }
                    :
                    { name: 'categoryDetails', params: { page: this.$route.params.page, categoryId: this.$route.params.categoryId } }

                    this.$router.push(routerParams);
                }
            },
            addNew(item) {
                this.newsByCategory.news.unshift(item);
            }
        },
        created() {
            this.getNewsForCategory();
            this.GET_CATEGORIES_BY_BREADCRUMBS(this.$route.params.categoryId);
        }
    };
</script>

<style scoped lang="scss">
</style>
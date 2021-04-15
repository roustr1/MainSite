<template>
    <div>
        <ms-birthday v-if="IsNews" />
        <div id="newsComponent">
            <msBreadCrumbs
                   v-if="!IsNews"
                   :items="breadcrumbs"
                   />
            <msCreaterNewsItem 
                               :categoryId="$route.params.categoryId"
                               />
            <msNewsItem v-for="item in news"
                :key="item.id"
                :news_item="item" />
            <msPage
                v-if="pager.ViewPageList && pager.ViewPageList.length"
                :pageIndex="pager.PageIndex"
                :parentPage="pager.CurrentPage"
                :totalPages="pager.TotalPages"
                :individualPagesDisplayedCount="pager.IndividualPagesDisplayedCount"
                :list="pager.ViewPageList"
                @changePage="changePage"
            />
        </div>
    </div>
</template>

<script>
    import msNewsItem from './ms-news-item.vue';
    import msCreaterNewsItem from './ms-creater_news-item.vue';
    import msPage from '../../DefaultComponents/ms-page.vue';
    import { mapActions, mapGetters, mapMutations, mapState } from 'vuex';
    import msBirthday from '../Birthday/ms-birthday.vue';
    import msBreadCrumbs from '../../DefaultComponents/ms-breadcrumbs-category.vue';

    export default {
        name: "ms-news",

        computed: {
            ...mapState('news', [
                'news',
                'pager'
            ]),
            ...mapState('menu', [
                'breadcrumbs'
            ]),
            IsNews() {
                return typeof (this.$route.params.categoryId) === 'undefined';
            },
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
            ...mapActions('news',[
                'GET_NEWS',
            ]),
            ...mapActions('menu',[
                'GET_CATEGORIES_BY_BREADCRUMBS',
            ]),
            ...mapMutations('news',[
                'DELETE_CURRENT_NEWS'
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
                )
            },
            changePage(new_page) {
                if (new_page !== this.$route.params.page) {

                    let news_page = this.$route.name == 'news' ? new_page : 1;
                    let routerParams = typeof (this.$route.params.categoryId) === 'undefined' ?
                        { name: 'news', params: { page: news_page } }
                    :
                        { name: 'categoryDetails', params: { page: new_page, categoryId: this.$route.params.categoryId } }

                    this.$router.push(routerParams);
                }
            },
        },
        created() {
            this.getNewsForCategory();
            if (!this.IsNews) {
                this.GET_CATEGORIES_BY_BREADCRUMBS(this.$route.params.categoryId);
            }
        },
        beforeDestroy() {
            this.DELETE_CURRENT_NEWS();
        }
    };
</script>

<style scoped lang="scss">
</style>
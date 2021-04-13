<template>
    <div>
        <ms-birthday v-if="IsNews" />
        <div id="newsComponent">
            <msCreaterNewsItem
                               :categoryId="categoryId"
                               />
            <msNewsItem v-for="item in NEWS"
                        :key="item.id"
                        :news_item="item" />
            <msPage
                    v-show="undefined !== PAGER.ViewPageList && PAGER.ViewPageList.length"
                    :pageIndex="PAGER.PageIndex"
                    :currentPage="CurrentPage"
                    :totalPages="PAGER.TotalPages"
                    :individualPagesDisplayedCount="PAGER.IndividualPagesDisplayedCount"
                    :list="PAGER.ViewPageList"
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

    export default {
        name: "ms-news",

        data() {
            return {
                newsByCategory: []
            }
        },
        computed: {
            ...mapGetters([
                'NEWS',
                'PAGER'
            ]),
            CurrentPage() {
                return this.$route.params.page;
            }
        },
        components: {
            msNewsItem,
            msCreaterNewsItem,
            msPage,
            msBirthday
        },
        watch: {
            $route: 'fetchData'
        },
        methods: {
            ...mapActions([
                'GET_NEWS'
            ]),
            fetchData() {
                this.getNewsForCategory();
            },
            getNewsForCategory() {
                this.setParams();
                this.GET_NEWS(
                    {
                        page: this.page,
                        categoryId: this.categoryId
                    }
                );
            },
            setParams() {
                this.categoryId = this.$route.params.categoryId;
                this.page = this.$route.params.page;
            },
            changePage(new_page) {
                if (new_page !== this.page) {
                    this.page = new_page;
                    let routerParams;

                    if (typeof (this.categoryId) === 'undefined') {
                        routerParams = { name: 'news', params: { page: this.page } };
                    }
                    else {
                        routerParams = { name: 'categoryDetails', params: { page: this.page, categoryId: this.categoryId } };
                    }

                    this.$router.push(routerParams);
                }
            }
        },
        mounted() {
            this.getNewsForCategory();
        }
    };
</script>

<style scoped lang="scss">
</style>
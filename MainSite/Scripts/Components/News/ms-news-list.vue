<template>
    <div id="newsComponent"> 
        <msNewsItem 
            v-for="(item, index) in news"
            :key="item.Id"
            :index="index"
            :isNews="IsNews"
            :news_item="item"
        />
        <msPage v-if="pager.viewPageList && pager.viewPageList.length" />
    </div>
</template>

<script>
    import msNewsItem from './ms-news-item.vue';
    import msCreaterNewsItem from './ms-creater_news-item.vue';
    import msPage from '../../DefaultComponents/ms-page.vue';
    import { mapActions, mapMutations, mapState } from 'vuex';
  
    export default {
        name: "ms-news-list",
        components: {
            msNewsItem,
            msCreaterNewsItem,
            msPage
        },
        data() {
            return {}
        },
        computed: {
            ...mapState('news', [
                'news',
                'pager'
            ]),
            IsNews() {
                return typeof this.$route.params.categoryId === 'undefined'
            }
        },
        watch: {
            $route: 'fetchData'
        },
        methods: {
            ...mapActions('news',[
                'GET_NEWS'
            ]),
            ...mapMutations('news', [
                'DELETE_CURRENT_NEWS'
            ]),
            fetchData() {
                this.getNews();
            },
            getNews() {
                this.GET_NEWS(
                    {
                        page: this.$route.params.page,
                        categoryId: this.$route.params.categoryId
                    }
                )
            }
        },
        created() {
            this.getNews();
        },
        beforeDestroy() {
            this.DELETE_CURRENT_NEWS();
        }
    };
</script>

<style scoped lang="scss">
</style>
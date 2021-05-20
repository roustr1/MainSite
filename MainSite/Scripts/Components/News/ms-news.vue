<template>
    <div class="row">
        <ms-calendar />
        <ms-birthday v-if="IsNews" />
        <div id="newsComponent">
            <msBreadCrumbs
                v-if="!IsNews"
                :items="breadcrumbs"
            />
            <msCreaterNewsItem 
                :categoryId="$route.params.categoryId"
            />
            <msNewsItem v-for="(item, index) in news"
                :key="item.Id"
                :index="index"
                :isNews="IsNews"
                :news_item="item" />
            <msPage
                v-if="pager.ViewPageList && pager.ViewPageList.length"
            />
        </div>
    </div>
</template>

<script>
    import msNewsItem from './ms-news-item.vue';
    import msCreaterNewsItem from './ms-creater_news-item.vue';
    import msPage from '../../DefaultComponents/ms-page.vue';
    import { mapActions, mapMutations, mapState } from 'vuex';
    import msBirthday from '../Birthday/ms-birthday.vue';
    import msBreadCrumbs from '../../DefaultComponents/ms-breadcrumbs-category.vue';
    import msCalendar from '../Calendar/ms-calendar.vue';

    export default {
        name: "ms-news",
        components: {
            msNewsItem,
            msCreaterNewsItem,
            msPage,
            msBirthday,
            msBreadCrumbs,
            msCalendar
        },
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
        watch: {
            $route: 'fetchData'
        },
        methods: {
            ...mapActions('news',[
                'GET_NEWS'
            ]),
            ...mapActions('menu',[
                'GET_CATEGORIES_BY_BREADCRUMBS',
            ]),
            ...mapMutations('news', [
                'DELETE_CURRENT_NEWS'
            ]),
            fetchData() {
                this.getNewsForCategory();
                this.getBreadCrumbs();
            },
            getNewsForCategory() {
                this.GET_NEWS(
                    {
                        page: this.$route.params.page,
                        categoryId: this.$route.params.categoryId
                    }
                )
            },
            getBreadCrumbs() {
                if (!this.IsNews) {
                    this.GET_CATEGORIES_BY_BREADCRUMBS(this.$route.params.categoryId);
                }
            }
        },
        created() {
            this.getNewsForCategory();
            this.getBreadCrumbs();
        },
        beforeDestroy() {
            this.DELETE_CURRENT_NEWS();
        }
    };
</script>

<style scoped lang="scss">
</style>
<template>
    <div class="row">
        <h4>Результат поиска:</h4>
        <msNewsItem
            v-for="(item, index) in news"
            :key="item.Id"
            :index="index"
            :isNews="IsNews"
            :news_item="item" 
        />
    </div>
</template>

<script>
    import msNewsItem from '../News/ms-news-item.vue';
    import { mapState, mapActions } from 'vuex';
    export default {
        name: 'ms-search-news',
        components: {
            msNewsItem
        },
        data() {
            return {}
        },
        watch: {
            $route: 'fetchData'
        },
        computed: {
            ...mapState('news', ['news'])
        },
        methods: {
            ...mapActions('news', [
                'GET_NEWS_BY_SEARCH'
            ]), 
            fetchData() {
                this.GET_NEWS_BY_SEARCH(this.$route.params.searchText);
            }
        },
        created() {
            this.GET_NEWS_BY_SEARCH(this.$route.params.searchText)
        }
    }
</script>

<style>
</style>

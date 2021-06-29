<template>
    <div>
         <ms-popup 
            v-if="isInfoPopupVisible"
            @closePopup="closePopupInfo"
            rightBtnTitle="Да"
            leftBtnTitle="Нет"
            @rightBtnAction="deleteNewsModal"
            popupTitle="Вы действительно хотите удалить запись?"
        >
            Это действие невозможно будет отменить, и все приложенные изображения и другие ресурсы так же будут удалены. Удалить запись?
         </ms-popup>
        <div id="newsComponent"> 
            <msNewsItem 
                v-for="(item, index) in news"
                :key="item.Id"
                :index="index"
                :isNews="IsNews"
                :news_item="item" 
                @deleteNews="deleteNews"
            />
            <msPage v-if="pager.viewPageList && pager.viewPageList.length" />
        </div>
    </div>
</template>

<script>
    import msNewsItem from './ms-news-item.vue';
    import msCreaterNewsItem from './ms-creater_news-item.vue';
    import msPage from '../../DefaultComponents/ms-page.vue';
    import msPopup from '../../DefaultComponents/ms-popup.vue';
    import { mapActions, mapMutations, mapState } from 'vuex';
  
    export default {
        name: "ms-news-list",
        components: {
            msNewsItem,
            msCreaterNewsItem,
            msPage,
            msPopup
        },
        data() {
            return {
                isInfoPopupVisible: false,
                infoNews: {}
            }
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
                'GET_NEWS',
                'DELETE_NEW'
            ]),
            ...mapMutations('news', [
                'DELETE_CURRENT_NEWS'
            ]),
            fetchData() {
                this.getNews();
            },
            closePopupInfo() {
                this.isInfoPopupVisible = false
            },
            deleteNews(infoNews) {
                this.isInfoPopupVisible = true
                this.infoNews = infoNews
            },
            deleteNewsModal() {
                this.DELETE_NEW(this.infoNews)
                this.infoNews = {}
                this.isInfoPopupVisible = false
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
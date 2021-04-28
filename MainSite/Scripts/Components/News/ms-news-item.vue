<template>
    <div class="card-panel row detailsNew">

        <div class="card_news card_news-details">
            <div class="card_news-image"><img :src="this.news_item.UrlIcon" alt="" /></div>
            <div class="card_news-main">
                <div v-if="isNews" class="card_news-main-header">
                    <span class="bold">{{this.news_item.Author}}</span>
                    <span class="bold" v-if="IsMessageDetails">{{this.Message}} запись в разделе</span>
                    <router-link
                                  :id="news_item.Id"
                                  :to="{name: 'categoryDetails', params: {categoryId : news_item.CategoryId, page: 1}}"
                                 >
                        {{this.news_item.Category}}
                    </router-link>
                </div>
                <div class="card_news-main-title"><a>{{this.news_item.Header}}</a></div>
                <div class="card_news-main-footer"><b v-if="!isNews">{{this.news_item.Author}}</b> {{RefactDate}}</div>
            </div>

            <div class="card_news-editor">
                <a href="#"><i class="material-icons">edit</i></a>
                <a href="#" class="error"><i class="material-icons">close</i></a>
            </div>
        </div>

        <div class="card_news-description">{{this.news_item.Description}}</div>

        <ul class="dropdownFiles">
            <li
                v-for="item in news_item.Files"
                :key="item.Id"
                >
                <a href="#" @click="downloadFile(item)">{{item.Name}}</a>
            </li>
        </ul>
    </div>
</template>

<script>
    import { mapActions } from 'vuex';
    export default {
        name: "ms-news-item",
        props: {
            news_item: {
                type: Object,
                default: () => { return {} }
            },
            isNews: {
                type: Boolean,
                default: () => { return true }
            }
        },
        data: () => {
            return {
            }
        },
        computed: {
            Message() {
                return this.news_item.isMessage ? "разместил" : "отредактировал";
            },
            RefactDate() {
                let options = {
                    day: 'numeric',
                    month: 'long',
                    year: 'numeric'
                }

                return new Date(this.news_item.CreatedDate).toLocaleDateString("ru", options);
            }
        },
        methods: {
            ...mapActions([
                'DOWNLOADFILE'
            ]),
            downloadFile(item) {
                this.DOWNLOADFILE(item);
            }
        },
        mounted() {
        }
    };
</script>

<style scoped lang="scss">
</style>
<template>
    <div class="card-panel row detailsNew" :id="GetUnicIdBlock">

        <div class="card_news card_news-details">
            <div class="card_news-image"><img :src="this.news_item.UrlIcon" alt="" /></div>
            <div class="card_news-main">
                <div class="card_news-main-header">
                    <span class="bold">{{this.news_item.Author}}</span>
                    <span class="bold" v-if="IsMessageDetails">{{this.Message}} запись в разделе</span>
                    <a v-if="IsMessageDetails" href="#" :id="news_item.Id">{{this.news_item.Category}}</a>

                </div>
                <div class="card_news-main-footer">{{this.news_item.CreateDate}}</div>
                <div class="card_news-main-title"><a>{{this.news_item.Header}}</a></div>
            </div>

            <div class="card_news-editor">
                <a href="#"><i class="material-icons">edit</i></a>
                <a href="#" class="error"><i class="material-icons">close</i></a>
            </div>
        </div>

        <div class="card_news-description" v-html="this.news_item.Description"></div>

        <ul class="dropdownFiles">
            <li
                v-for="item in news_item.Files"
                :key="item.Id"
                >
                <a href="#" @click="downloadFile(item)"><i class="material-icons">download</i> {{item.Name}}</a>
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
            }
        },
        data: () => {
            return {
                IsMessageDetails: true
            }
        },
        computed: {
            Message() {
                return this.news_item.isMessage ? "разместил" : "отредактировал";
            },
            GetUnicIdBlock() {
                return "new_" + this.news_item.Id;
            }
        },
        methods: {
            ...mapActions('news', [
                'DOWNLOADFILE'
            ]),
            downloadFile(item) {
                this.DOWNLOADFILE(item);
            },
            listenByAdvancedDesription() {
                let vm = this;
                document.querySelector("#" + vm.GetUnicIdBlock).addEventListener('click', function (e) {
                    e.preventDefault();   

                    let itemAdvancedEditor = {
                        Name: e.target.innerHTML,
                        Id: e.target.getAttribute('href')
                    }

                    vm.downloadFile(itemAdvancedEditor);
                });
            }
        },
        mounted() {
            this.news_item.IsAdvancedEditor = true;
            if (this.news_item.IsAdvancedEditor) {
                this.listenByAdvancedDesription();
            }
        }
    };
</script>

<style scoped lang="scss">
    .dropdownFiles a {
        display:flex;
        align-items:center;
        &:hover {
            i {
                color:#9e9e9e !important;
             }
        }
    }
</style>
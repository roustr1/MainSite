<template>
    <div class="card-panel detailsNew" :id="GetUnicIdBlock">
        <template v-if="!isEditer">
            <div class="card_news card_news-details">
                <div class="card_news-image"><img :src="this.news_item.urlIcon" alt="" /></div>
                <div class="card_news-main">
                    <div v-if="isNews" class="card_news-main-header">
                        <span class="bold">{{this.news_item.author}}</span>
                        <span class="bold">{{this.Message}} запись в разделе</span>
                        <router-link 
                            :id="news_item.id"
                            :to="{name: 'categoryDetails', params: {categoryId : news_item.categoryId, page: 1}}"
                        >
                            {{this.news_item.category}}
                        </router-link>
                    </div>
                    <div class="card_news-main-title"><a>{{this.news_item.header}}</a></div>
                    <div class="card_news-main-footer"><b v-if="!isNews">{{this.news_item.author}}</b> {{RefactDate}}</div>
                </div>

                <div class="card_news-editor">
                    <a href="#"  v-on:click.prevent="changeSectionEditer" title="Редактировать"><i class="material-icons">edit</i></a>
                    <a href="#" class="error" v-on:click.prevent="deleteNews" title="Удалить"><i class="material-icons">close</i></a>
                </div>
            </div>

            <div class="card_news-description" v-html="this.news_item.description"></div>

            <ul class="dropdownFiles">
                <li v-for="item in news_item.files"
                    :key="item.id">
                    <a href="#" @click="downloadFile(item)"><img src="/images/layout_icons/News/fileLoad.svg" style="padding-right:5px;" />  {{item.name}}</a>
                </li>
            </ul>
        </template>
        <template v-else>
            <div>
                <div class="card_news-editor" style="right:10px;">
                    <a href="#" v-on:click.prevent="changeSectionEditer" title="Назад"><i class="material-icons">reply</i></a>
                </div>
                <msChangeNewsForm 
                   :isAdvancedEditor="news_item.isAdvancedEditor"
                   :categoryId="news_item.categoryId"
                   @changeNew="changeNew" 
                   :editModel="news_item"
                   :editFiles="news_item.files"
                   textSubmit="Редактировать"
                />
            </div>
        </template>
</div>
</template>

<script>
    import msChangeNewsForm from './ms-change_news-form.vue';
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
            },
            index: {
                type: Number,
                default: () => { return null }
            }
        },
        data: () => {
            return {
                isEditer: false ,
                model: {}
            }
        },
        components: {
            msChangeNewsForm
        },
        computed: {
            Message() {
                return this.news_item.isMessage ? "отредактировал" : "разместил";
            },
            GetUnicIdBlock() {
                return "new_" + this.news_item.id;
            },
            RefactDate() {
                let options = {
                    day: 'numeric',
                    month: 'long',
                    year: 'numeric'
                }

                return new Date(this.news_item.createdDate).toLocaleDateString("ru", options);
            },
        },
        methods: {
            ...mapActions('news', [
                'DOWNLOADFILE',
                'UPDATE_NEW',
                'DELETE_NEW'
            ]),
            changeSectionEditer() {
                this.isEditer = !this.isEditer;
            },
            downloadFile(item) {
                this.DOWNLOADFILE(item);
            },
            deleteNews() {
                this.DELETE_NEW({ index: this.index, id: this.news_item.id });
            },
            async changeNew(result) {
                let res = await this.UPDATE_NEW({ data: result, index: this.index });
                if (res) {
                    await this.changeSectionEditer();
                    this.listenByAdvancedDesription();
                }
            },
            listenByAdvancedDesription() {
                let vm = this;
                for (var selector of  Array.from(document.querySelectorAll("#" + vm.GetUnicIdBlock + " > .card_news-description a"))) {
                    selector.addEventListener('click', function (e) {
                        e.preventDefault();

                        let itemAdvancedEditor = {
                            name: e.target.innerHTML,
                            id: e.target.getAttribute('href')
                        }

                        vm.downloadFile(itemAdvancedEditor)
                    });
                }
            }
        },
        mounted() {            
            if (this.news_item.isAdvancedEditor) {
                this.listenByAdvancedDesription();
            }
        }
    };
</script>

<style scoped lang="scss">
    .detailsNew {
        position:relative;
    }
    .card_news-description {
        font-size: 14px;
    }
    .dropdownFiles a {
        display: inline-flex;
        align-items: center;
        padding-bottom: 5px;
        font-size: 14px;
        & > img {
          width: 23px;
        }
        &:hover {
          color: #64b5f6 !important;
      }
    }
</style>
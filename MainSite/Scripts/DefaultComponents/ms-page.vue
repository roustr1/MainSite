<template>
    <div class="card-panel">
        <ul class="pagination">
            <li v-if="ShowFirst" class="first-page btn btn-defaultMainSite">
                <a href="#"  @click.prevent="changePage(1)" title="Первая страница">Первая</a>
            </li>
            <li v-if="ShowPrevious" class="previous-page btn btn-defaultMainSite">
                <a href="#" @click.prevent="changePage(Number(GetCurrentPage) - 1)" title="Предыдущая страница">Предыдущая</a>
            </li>
            <li 
                class="individual-page waves-effect"
                v-for="(item, index) in list"
                :key="index"
                v-bind:class="{ active: item.IsActive }"
                >
                <a 
                   href="#" 
                   @click.prevent="changePage(item.Index)"
                   :title="setTitle(item.Index)"
                   v-bind:class="{ disabled:item.IsActive }"
                >
                    {{item.Index}}
                </a>
            </li>
            <li v-if="ShowNext" class="next-page btn btn-defaultMainSite">
                <a href="#" @click.prevent="changePage(Number(GetCurrentPage) + 1)" title="Следующая страница">Далее</a>
            </li>
            <li v-if="ShowLast" class="last-page btn btn-defaultMainSite">
                <a href="#" @click.prevent="changePage(totalPages)" title="Последняя страница">Последняя</a>
            </li>
        </ul>
    </div>
</template>

<script>
    import { mapState } from 'vuex';

    export default {
        name: 'ms-page',
        props: {
        },
        data() {
            return {
            }
        },
        watch: {
            $route: 'fetchData'
        },
        computed: { 
            ...mapState('news', {
                pagerIndex: state => state.pager.PageIndex,
                totalPages: state => state.pager.TotalPages,
                individualPagesDisplayedCount: state => state.pager.IndividualPagesDisplayedCount,
                currentPage: state => state.pager.CurrentPage,
                list: state => state.pager.ViewPageList
            }),
            ShowFirst() {
                return ((this.pagerIndex >= 3) && (this.totalPages > this.individualPagesDisplayedCount))
            },
            ShowPrevious() {
                return this.pagerIndex > 0
            },
            ShowNext() {
                return ((this.pagerIndex + 1) < this.totalPages)
            },
            ShowLast() {
                return ((this.pageIndex + 3) < this.totalPages) && (this.totalPages > this.individualPagesDisplayedCount)
            },
            GetCurrentPage() {
                if (this.currentPage != undefined) return this.currentPage;
                return 0;
            }
        },
        methods: {
            fetchData() {
                if (this.currentPage == undefined)
                    this.currentPage = this.$route.params.page;
            },
            changePage(page) {
                let news_page = this.$route.name == 'news' ? page : 1;
                let routerParams = typeof (this.$route.params.categoryId) === 'undefined' ?
                    { name: 'news', params: { page: news_page } }
                    :
                    { name: 'categoryDetails', params: { page: new_page, categoryId: this.$route.params.categoryId } }

                this.$router.push(routerParams);
            },
            setTitle(number) {
                return "Страница " + number;
            }
        },
        mounted() {
        }
    }
</script>

<style lang="scss" scoped>
    a.disabled {
        pointer-events: none; /* делаем ссылку некликабельной */
    }
</style>
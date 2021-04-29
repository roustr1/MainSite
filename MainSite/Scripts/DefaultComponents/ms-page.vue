<template>
    <div class="card-panel row">
        <ul class="pagination">
            <li v-if="ShowFirst" class="first-page btn btn-defaultMainSite">
                <a href="#"  @click.prevent="() => $emit('changePage', 1)" title="Первая страница">Первая</a>
            </li>
            <li v-if="ShowPrevious" class="previous-page btn btn-defaultMainSite">
                <a href="#" @click.prevent="() => $emit('changePage', Number(GetCurrentPage) - 1)" title="Предыдущая страница">Предыдущая</a>
            </li>
            <li 
                class="individual-page waves-effect"
                v-for="(item, index) in list"
                :key="index"
                v-bind:class="{ active: item.IsActive }"
                >
                <a href="#" @click.prevent="() =>  $emit('changePage', item.Index)" :title="setTitle(item.Index)">{{item.Index}}</a>
            </li>
            <li v-if="ShowNext" class="next-page btn btn-defaultMainSite">
                <a href="#" @click.prevent="() => $emit('changePage', Number(GetCurrentPage) + 1)" title="Следующая страница">Далее</a>
            </li>
            <li v-if="ShowLast" class="last-page btn btn-defaultMainSite">
                <a href="#" @click.prevent="() => $emit('changePage', totalPages)" title="Последняя страница">Последняя</a>
            </li>
        </ul>
    </div>
</template>

<script>
    export default {
        name: 'ms-page',
        props: {
            parentPage: {
                type: Number,
                default: () => {
                    return undefined;
                }
            },
            pageIndex: {
                type: Number,
                default: () => {
                    return 0;
                }
            },
            totalPages: {
                type: Number,
                default: () => {
                    return 0;
                }
            },
            individualPagesDisplayedCount: {
                type: Number,
                default: () => {
                    return 0;
                }
            },
            list: {
                type: Array,
                default: () => {
                    return []
                }
            }
        },
        data() {
            return {
                currentPage: 0
            }
        },
        watch: {
            $route: 'fetchData'
        },
        computed: {  
            ShowFirst() {
                return ((this.pageIndex >= 3) && (this.totalPages > this.individualPagesDisplayedCount))
            },
            ShowPrevious() {
                return this.pageIndex > 0
            },
            ShowNext() {
                return ((this.pageIndex + 1) < this.totalPages)
            },
            ShowLast() {
                return ((this.pageIndex + 3) < this.totalPages) && (this.totalPages > this.individualPagesDisplayedCount)
            },
            GetCurrentPage() {
                if (this.parentPage != undefined) this.currentPage = this.parentPage;
                return this.currentPage;
            }
        },
        methods: {
            fetchData() {
                if (this.parentPage == undefined)
                    this.currentPage = this.$route.params.page;
            },
            changePage(page) {
                this.$emit("changePage", page);
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

</style>
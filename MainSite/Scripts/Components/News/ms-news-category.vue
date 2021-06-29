<template>
    <div class="row" style="margin-top:10px;">
        <msBreadCrumbs />
        <msCreaterNewsItem
         v-if="isEditer"
         :categoryId="$route.params.categoryId" />
        <msNewsList />
    </div>
</template>

<script>
    import msNewsList from './ms-news-list.vue';
    import msBreadCrumbs from '../../DefaultComponents/ms-breadcrumbs-category.vue';
    import msCreaterNewsItem from './ms-creater_news-item.vue';
    import { mapActions } from 'vuex';

    export default {
        name: "ms-news-category",
        data() {
            return {
                isEditer: false
            }
        },
        components: {
            msNewsList,      
            msBreadCrumbs,
            msCreaterNewsItem
        },
        watch: {
            $route: 'getInfoByPermission'
        },
        methods: {
            ...mapActions('user', [
                'GET_PERMISSION_BY_CATEGORY'
            ]),
            async getInfoByPermission() {
                this.isEditer = await this.GET_PERMISSION_BY_CATEGORY(this.$route.params.categoryId);
            },
        },
        mounted() {
            this.getInfoByPermission()
        }
    };
</script>

<style scoped lang="scss">
</style>
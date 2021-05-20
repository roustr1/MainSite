<template>
    <div class="row">
        <msBreadCrumbs />
        <msCreaterNewsItem :categoryId="$route.params.categoryId" />
        <msNewsList />
    </div>
</template>

<script>
    import msNewsList from './ms-news-list.vue';
    import msBreadCrumbs from '../../DefaultComponents/ms-breadcrumbs-category.vue';
    import msCreaterNewsItem from './ms-creater_news-item.vue';

    export default {
        name: "ms-news-category",
        components: {
            msNewsList,      
            msBreadCrumbs,
            msCreaterNewsItem
        }
    };
</script>

<style scoped lang="scss">
</style>
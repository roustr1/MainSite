<template>
    <div class="row">
        <ms-calendar />
        <ms-birthday/>
        <msCreaterNewsItem :categoryId="$route.params.categoryId" />
        <msNewsList />
    </div>
</template>

<script>
    import msNewsList from './ms-news-list.vue';
    import msCreaterNewsItem from './ms-creater_news-item.vue';
    import msBirthday from '../Birthday/ms-birthday.vue';
    import msCalendar from '../Calendar/ms-calendar.vue';

    export default {
        name: "ms-news",
        components: {
            msNewsList,
            msCreaterNewsItem,
            msBirthday,
            msCalendar
        }
    };
</script>

<style scoped lang="scss">
</style>
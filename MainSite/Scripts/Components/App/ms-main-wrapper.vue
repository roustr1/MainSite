<template>
    <router-view />
</template>

<script>
    import msNews from '../News/ms-news.vue';
    export default {
        name: 'ms-main-wrapper',
        components: {
            msNews
        },
        data() {
            return {}
        }
    }
</script>

<style>
</style>

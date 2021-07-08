<template>
    <div>
      <ms-modal-wrapper />      
      <router-view />
    </div>

</template>

<script>
    import msNews from '../News/ms-news.vue';
    import msModalWrapper from '../../DefaultComponents/Modal/ms-modal-wrapper';
    export default {
        name: 'ms-main-wrapper',
        components: {
            msNews,
            msModalWrapper
        },
        data() {
            return {}
        },
        mounted () {
        }
    }
</script>

<style lang="scss">
    .container {
        width: 60%;
    }
</style>

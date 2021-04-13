<template>
    <div>
        <VueBreadcrumbs />
        <keep-alive>
            <router-view />
        </keep-alive>
    </div>
</template>

<script>
    import VueBreadcrumbs from 'vue-breadcrumbs'
    export default {
        name: 'ms-category',
        props: {
            category: {
                type: Object,
                default: () => {
                    return {}
                }
            }
        },
        components: {
            VueBreadcrumbs
        },
        data() {
            return {}
        },
        mounted() {
        }
    }
</script>

<style>

</style>

<template>
    <div>
        <msCategoryItem v-for="item in category.children"
                    :key="item.id"
                    :category_item="item" />
    </div>
</template>

<script>
    import msCategoryItem from '../Category/ms-category-item.vue'
    export default {
        props: {
            category: {
                type: Object,
                default: () => { return {} }
            }
        },
        data() {
            return {}
        },
        components: {
            msCategoryItem
        },
        mounted() {
            console.log(this.$route);
        }
    }
</script>

<style>

</style>
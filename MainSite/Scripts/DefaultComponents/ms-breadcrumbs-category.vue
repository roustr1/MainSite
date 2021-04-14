<template>
    <div class="navHeader">
        <router-link class="navHeader-item"
                     :to="{name:'news'}">
            Новости
        </router-link>
        <a
           href="#"
            class="navHeader-item"
            v-for="(item, index) in items"
            :key="index"
            v-on:click.prevent.self="clickEvent(item)">
            {{item.name}}
        </a>
    </div>
</template>

<script>
    export default {
        name: 'ms-breadcrumbs-category',
        props: {
            items: {
                type: Array,
                default: () => { return [] }
            }
        },
        data(){
            return {
            }
        },
        methods: {
            clickEvent(item) {
                this.$router.push({ name: 'categoryList', params: { categoryId: item.id } });
            }
        }
    }
</script>

<style>

</style>
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
            v-on:click.prevent.self="clickEvent(item)"
            v-bind:class="CheckLastItem(index)"
           >
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
                if (this.$route.params.categoryId !== item.id)
                    this.$router.push({ name: 'categoryList', params: { categoryId: item.id } });
            },
            CheckLastItem(indexItem) {
                if (this.items.length > 0) {
                    if (this.items.length - 1 == indexItem)
                        return 'bold';
                }

                return '';
            }
        }
    }
</script>

<style>

</style>
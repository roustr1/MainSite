<template>
    <div class="navHeader">
        <a class="navHeader-item"
           href="#"
           @click.prevent.self="backMainView">
            Новости
        </a>
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
    import { mapMutations } from 'vuex';

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
            ...mapMutations('menu', [
                'SET_OR_UPDATE_ACTIVE_CATEGORY'
            ]),
            clickEvent(item) {
                if (this.$route.params.categoryId !== item.id)
                    this.$router.push({ name: 'categoryList', params: { categoryId: item.id } });
            },
            backMainView() {
                this.SET_OR_UPDATE_ACTIVE_CATEGORY(null);
                this.$router.push('/');
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
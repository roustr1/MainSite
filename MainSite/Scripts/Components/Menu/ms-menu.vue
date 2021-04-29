<template>
    <ul class="menu">
        <li>
            <a
               href="javascript:void(0)"
               @click="eventClickElementMenu(arguments[0])"
               v-bind:class="IsActive"
               >
                <span class="rectangle"></span>
                <div class="bold">Новости</div>
            </a>
        </li>
        <ms-menu-item
            v-for="category in categories"
            :key="category.id"
            :menu_item="category"
            >
        </ms-menu-item>

    </ul>
</template>

<script>
    import msMenuItem from './ms-menu-item.vue';
    import { mapActions, mapState, mapMutations } from 'vuex'
    //import { ItemMenuActive } from '../../Filters/Menu'
    
    export default {
        name: "ms-menu",
        data() {
            return {
                categoryId: undefined
            }
        },
        components: {
             msMenuItem
        },
        computed: {
            ...mapState('menu', [
                'categories',
                'activeCategoryId'
            ]),
            IsActive() {
                return this.categoryId == this.activeCategoryId && this.$route.params.categoryId == this.categoryId ? 'active' : '';
            }
        },
        methods: {
            ...mapActions('menu',[
                'GET_CATEGORIES'
            ]),
            ...mapMutations('menu', [
                'SET_OR_UPDATE_ACTIVE_CATEGORY'
            ]),
            eventClickElementMenu(e) {
                //ItemMenuActive.eventClickElementMenu(e);

                if (this.$route.params.categoryId) {
                    this.SET_OR_UPDATE_ACTIVE_CATEGORY(null);
                    this.$router.push({ name: "news", params: { page: 1 }});
                }
                
            }
        },
        created() {
            this.GET_CATEGORIES();
            if (this.categoryId != this.activeCategoryId) this.SET_OR_UPDATE_ACTIVE_CATEGORY(null);
        }
    };
</script>

<style scoped lang="scss">
    .rectangle {
        background-color: white;
    }
</style>
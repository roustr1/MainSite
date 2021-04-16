<template>
    <ul class="menu">
        <li>
            <a
               href="javascript:void(0)"
               @click="eventClickElementMenu(arguments[0])"
               :class="IsActive"
               >
                <span class="rectangle"></span>
                <div class="bold">Новости</div>
            </a>
        </li>
        <ms-menu-item
            v-for="category in categories"
            :key="category.id"
            :menu_item="category"
            @eventClickElementMenu="eventClickElementMenu"
            >
        </ms-menu-item>

    </ul>
</template>

<script>
    import msMenuItem from './ms-menu-item.vue';
    import { mapActions, mapState } from 'vuex'
    import { ItemMenuActive } from '../../Filters/Menu'

    export default {
        name: "ms-menu",
        data: () => {
            return {
                IsActiveLink: true,
                IsActive: 'active'
            }
        },
        components: {
             msMenuItem
        },
        computed: {
            ...mapState('menu', [
                'categories'
            ])
        },
        methods: {
            ...mapActions('menu',[
                'GET_CATEGORIES'
            ]),
            eventClickElementMenu(e) {
                ItemMenuActive.eventClickElementMenu(e);
                if (this.$route.params.categoryId) {
                    this.$router.push({ name: "news", params: { page: 1 }});
                }
                
            }
        },
        created() {
            this.GET_CATEGORIES();
        }
    };
</script>

<style scoped lang="scss">
    .rectangle {
        background-color: white;
    }
</style>
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

                      v-for="category in this.CATEGORIES"
                      :key="category.id"
                      :menu_item="category"
                      @eventClickElementMenu="eventClickElementMenu"
                      >

        </ms-menu-item>

    </ul>
</template>

<script>
    import msMenuItem from './ms-menu-item.vue';
    import { mapActions, mapGetters } from 'vuex'
    import { ItemMenuActive } from '../../Filters/Menu'

    export default {
        name: "ms-menu",
        data: () => {
            return {
                title: "Menu",
                IsActiveLink: true,
                IsActive: 'active'
            }
        },
        components: {
             msMenuItem
        },
        computed: {
            ...mapGetters({
                CATEGORIES: 'CATEGORIES'
            })
        },
        watch: {
            IsCurrent() {
                return this.IsCurrent;
            }
        },
        methods: {
            ...mapActions({
                GET_CATEGORIES: 'GET_CATEGORIES'
            }),
            eventClickElementMenu(e) {
                ItemMenuActive.eventClickElementMenu(e);
                if (this.$route.params.categoryId) {
                    this.$router.push({ name: "news" });
                }
                
            }
        },
        mounted() {
            this.GET_CATEGORIES();
        }
    };
</script>

<style scoped lang="scss">
    .rectangle {
        background-color: white;
    }
</style>
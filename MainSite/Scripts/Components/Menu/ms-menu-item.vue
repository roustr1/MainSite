<template>
    <li>
        <a href="javascript:void(0)"
           :title="menu_item.toolTip"
           v-bind:class="activeClass[IsActive]"
           @click="eventClickElementMenu(arguments[0])">
            <span src="/content/layout_icons/free-icon-ads-2625106.svg"></span>
            <div>{{menu_item.name}}</div>
        </a>

        <!--<ul
        v-if="menu_item.children && menu_item.children.length"
        >
        <ms-menu-item v-for="item in menu_item.children"
                      :key="item.id"
                      :menu_item="item"
                      >

        </ms-menu-item>
    </ul>-->
    </li>
</template>
<script>
    //import { ItemMenuActive } from '../../Filters/Menu'
    import { mapState, mapMutations } from 'vuex'

    export default {
        name: "ms-menu-item",
        props: {
            menu_item: {
                type: Object,
                default: () => { return {} }
            }
        },
        data:() => {
            return {
                activeClass: {
                    true: 'active',
                    false: ''
                }
            }
        },
        computed: {
            ...mapState('menu',[
                'activeCategoryId',
                'breadcrumbs'
            ]),
            IsActive() {
                if (this.$route.params.categoryId != undefined && this.$route.params.categoryId != this.menu_item.id) {
                    if (this.breadcrumbs.length) {
                        return this.breadcrumbs[0].id == this.menu_item.id ? true : false;
                    }
                }

                return this.menu_item.id == this.activeCategoryId || this.$route.params.categoryId == this.menu_item.id ? true : false;
            }
        },
        methods: {
            ...mapMutations('menu',[
                'SET_OR_UPDATE_ACTIVE_CATEGORY'
            ]),
            eventClickElementMenu(e) { 
                if (!this.IsActive) {
                    this.SET_OR_UPDATE_ACTIVE_CATEGORY(this.menu_item.id);

                    if (this.menu_item.children && this.menu_item.children.length) {
                        this.$router.push({ name: "categoryList", params: { categoryId: this.menu_item.id, category: this.menu_item } });
                    }
                    else {
                        this.$router.push({ name: "categoryDetails", params: { categoryId: this.menu_item.id, page: 1 } });
                    }
                }
            },
        }
    };
</script>

<style scoped lang="scss">
    span {
        background-color: black;
        height: 2px;
        width: 30px;
    }
</style>
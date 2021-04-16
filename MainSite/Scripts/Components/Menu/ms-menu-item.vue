<template>
    <li>
        <a href="javascript:void(0)"
           :title="menu_item.toolTip"
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
    import { ItemMenuActive } from '../../Filters/Menu'

    export default {
        name: "ms-menu-item",
        props: {
            menu_item: {
                type: Object,
                default: () => { return {} }
            },
        },
        data:() => {
            return {
                IsActiveLink: false,
            }
        },
        methods: {
            eventClickElementMenu(e) {
                ItemMenuActive.eventClickElementMenu(e);
                if (this.$route.params.categoryId !== this.menu_item.id) {
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
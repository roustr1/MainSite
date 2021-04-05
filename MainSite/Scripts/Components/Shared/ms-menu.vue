<template>
    <ul class="menu">
        <li>
            <a @class="isActive"
               href="#">
                <span class="rectangle" style="background-color: white;"></span>
                <div class="bold">Новости</div>
            </a>
        </li>
        <ms-menu-item

                      v-for="item in menuList"
                      :key="item.id"
                      :menu_item="item"
                      >

        </ms-menu-item>

    </ul>
</template>

<script>
    import msMenuItem from './ms-menu-item.vue';

    export default {
        name: "ms-menu",
        data() {
            return {
                menuList: [{id:1, name:'test1'}],
                title: "Menu",
                IsActiveLink: true
            }
        },
        components: {
             msMenuItem
        },
        computed: {
            IsAcive() {
                if (this.IsActiveLink) {
                    return 'active';
                }
                else {
                    return '';
                }
            }
        },
        methods: {
            menuItemClick(id) {
                console.log(id);
            }
        }
    };
</script>
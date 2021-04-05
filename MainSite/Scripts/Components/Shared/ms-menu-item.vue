<template>
    <li>
        <a
                  :class="isActive"
                  @click="ChangeActiveLink"
                  href="#"
                  >
            <span class="rectangle" style="background-color: white;"></span>
            <div class="bold">{{menu_item.name}}</div>
        </a>
    </li>
</template>
<script>
    export default {
        name: "ms-menu-item",
        props: {
            menu_item: {
                type: Object,
                default() {
                    return {}
                }
            },
            IsActiveRoot: {
                type: Boolean,
                default() {
                    return true;
                }
            }
        },
        computed: {
            isActive() {
                if (this.IsActiveLink) {
                    return 'active';
                }
                else return '';
            }
        },
        data:() => {
            return {
                IsActiveLink: false
            }
        },
        methods: {
            ChangeActiveLink() {
                this.IsActiveLink = true;
                this.$emit('menuItemClick', this.menu_item.id);
            }
        }
    };
</script>

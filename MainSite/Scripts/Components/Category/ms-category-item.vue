<template>
    <div class="card-panel ms-category-item"
         @click="toCategory">
        <h5 class="text-center ms-category-item_title bold">
            {{category_item.Name}}
        </h5>
        <div class="ms-category-item_image">
            <img v-bind:src="getPathImg" />
        </div>
    </div>
</template>

<script>
    export default {
        name: 'ms-category-item',
        props: {
            category_item: {
                type: Object,
                default: () => { return {} }
            }
        },
        data() {
            return {
                namePath: '',
                params: {}
            }
        },
        computed: {
            getPathImg() {
                if (this.category_item.UrlIcon) {
                    return this.category_item.UrlIcon;
                }

                return "/images/layout_icons/education.jpg";
            }
        },
        methods: {
            getNamePath() {
                if (this.category_item.Children.length > 0) {
                    return 'categoryList'
                }
                else {
                    return 'categoryDetails'
                }
            },
            setParamsPath() {
                this.params = {};
                this.params.categoryId = this.category_item.Id;
                this.params.page = 1;
                if (this.category_item.Children.length > 0) {
                    this.params.category = this.category_item;
                }
            },
            toCategory() {
                this.$router.push({ name: this.getNamePath(), params: this.params });
            }
        },
        mounted() {
            this.setParamsPath()
        }
    }
</script>

<style lang="scss">
    .ms-category-item {
        overflow: hidden;
        height: 240px;
        width: 47%;
        margin: 10px 10px;
        cursor: pointer;
        &_title

    {
        padding: 0px 5px;
        margin: 0px;
    }

    &_image {
        border-radius: 20px;
        overflow: hidden;
        position: relative;
        padding: 5px 5px;
        filter: blur(2px);
        height: 200px;
        & > img

    {
        width: 100%;
        height: 100%;
        display: inline-block;
    }

    }
    }
</style>

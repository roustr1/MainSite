<template>
    <div class="card-panel row">
        <h4>
            <router-link :to="{name: getNamePath(), params: this.params }">{{category_item.name}}</router-link>
        </h4>
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
        methods: {
            getNamePath() {
                if (this.category_item.children.length > 0) {
                    return 'categoryList'
                }
                else {
                    return 'categoryDetails'
                }
            },
            setParamsPath() {
                this.params = {};
                this.params.categoryId = this.category_item.id;
                this.params.page = 1;
                if (this.category_item.children.length > 0) {
                    this.params.category = this.category_item;
                }
            }
        },
        mounted() {
            this.setParamsPath()
        }
    }
</script>

<style>
    .ms-category-item {
        margin-right: 10px;
    }
</style>

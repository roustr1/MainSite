<template>
    <div>
        <div class="card-panel" style="padding:0px;overflow:hidden;height: 250px;display:flex;flex-direction:column;">
            <h5 class="text-center" style="margin:0px;padding:10px;">
                <router-link :to="{name: getNamePath(), params: this.params }">{{category_item.name}}</router-link>
            </h5>
            <div style="padding:0px; flex-basis:90%;">
                <img style="width: 100%; height:100%; display:inline-block;" src="/images/layout_icons/education.jpg"  />
            </div>
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

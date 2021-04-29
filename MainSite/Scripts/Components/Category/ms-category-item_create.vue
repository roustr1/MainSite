<template>
    <div class="col s12 m12 card-panel ms-category-item_create">
        <h5 class="header">Добавить подраздел</h5>
        <div>
            <form ref="formCreateNews" @submit="submit" action="/Admin/Menu/Create/" enctype="multipart/form-data" method="post">

                <div class="row">
                    <div class="input-field col s12 m12">
                        <input type="text" name="Name" id="Name" class="validate" />
                        <label asp-for="Name" for="Name">Наименование подраздела</label>
                    </div>
                    <div class="input-field col s12 m12">
                        <input type="hidden" name="ParentId" id="ParentId" :value="parentCategoryId"/>
                        <input disabled type="text" id="ParentName" class="validate" :value="parentCategoryName" />
                        <label for="ParentName" class="active">Родительский элемент</label>
                    </div>
                    <div class="input-field col  s12 m12">
                        <p>
                            <label>
                                <input name="IsActive" id="IsActive" type="checkbox" class="validate" />
                                <span>Отображать в списке меню</span>
                            </label>
                        </p>
                    </div>
                </div>
                <div class="input-field m12">
                    <input type="submit" value="Добавить" class="btn btn-defaultMainSite" />
                    <button class="btn btn-defaultMainSite" @click="backToCategoryList">Назад</button>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
    export default {
        name: "ms-category-item_create",
        props: {
            parentCategoryId: {
                type: String,
                default: () => { return '' }
            },
            parentCategoryName: {
                type: String,
                default: () => { return '' }
            }
        },
        data() {
            return {
            }
        },
        components: {
           
        },
        computed: {
        },
        methods: {
            createCategory() {

            },
            backToCategoryList() {
                this.$router.push({ name: "categoryList", params: { categoryId: this.parentCategoryId} });
            },
            submit() {

            }
        },
        mounted() {
           
        }
    }
</script>

<style lang="scss">
    .ms-category-add_item {
        cursor:pointer;
        margin: 5px 5px;
        width: 48%;
        height: 250px;
        display: flex;
        flex-direction: column;
        justify-content: center;
    }
</style>
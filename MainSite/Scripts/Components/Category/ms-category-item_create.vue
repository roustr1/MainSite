<template>
    <div class="col s12 m12 row card-panel ms-category-item_create">
        <h5 class="header">Добавить подраздел</h5>
        <br/>
        <form ref="formCreate" @submit="submit" action="/Admin/Menu/Create/" method="post">
            <div class="row">
                <div class="input-field col s12 m12">
                    <input type="hidden" name="ParentId" id="ParentId" :value="parentCategoryId" />
                    <input disabled type="text" id="ParentName" class="validate" :value="parentCategoryName" />
                    <label for="ParentName" class="active">Родительский элемент</label>
                </div>
                <div class="input-field col s12 m12">
                    <input type="text" name="Name" id="Name" class="validate" />
                    <label asp-for="Name" for="Name">Наименование подраздела</label>
                </div>
                <div class="file-field input-field col s12 m12">
                    <div class="btn btn-defaultMainSite">
                        <span>Выбрать изображение...</span>
                        <input type="file" name="UploadedFiles" multiple="multiple">
                     </div> 
                    <div class="file-path-wrapper" style="flex-grow: 1;">
                        <input disabled="disabled" type="text" placeholder="Наименование изображения" class="file-path" style="border: none; color: rgb(101, 147, 92);">
                    </div>
                </div>
                <div class="input-field col  s12 m12">
                    <p>
                        <label>
                            <input id="IsActive" type="checkbox" />
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
</template>

<script>
import { mapActions } from "vuex"

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
            ...mapActions('menu', [
                'ADD_CATEGORY',
            ]),
            createCategory() {

            },
            backToCategoryList() {
                this.$router.push({ name: "categoryList", params: { categoryId: this.parentCategoryId} });
            },
            submit(e) {
                e.preventDefault();
                let data = new FormData(this.$refs.formCreate);
                data.append("IsActive", document.getElementById('IsActive').checked);

                this.ADD_CATEGORY(data);
                this.$router.push({ name: "categoryList", params: { categoryId: this.parentCategoryId } });
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
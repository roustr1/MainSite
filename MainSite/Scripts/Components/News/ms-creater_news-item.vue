<template>
    <div class="creator">
        <div class="card-panel creator-main" v-bind:class="classObject">
            @await Component.InvokeAsync("CategoryPathComponent", @Model.CategoryId)
            <a class="error close" @click="showCreater(false, true)"><i class="material-icons">close</i></a>

            <form ref="formCreateNews" @submit="submit" action="/Home/Create/" enctype="multipart/form-data" method="post">
                <input  name="CategoryId" type="hidden" :value="this.categoryId" />
                <div class="s12 m12">
                    <div class="bold">Введите заголовок объявления:</div>
                    <input v-model="model.Header" name="Header" id="TextHeader" class="inputTextMainSite" type="text" />
                </div>
                <p class="s12 m12">
                    <div class="bold">Введите текстовое объявление:</div>
                    <textarea v-model="model.Description" name="Description" id="TextDescription" class="inputTextMainSite" height="300"></textarea>
                </p>
                <div class="file-field input-field creator-main-panel s12 m12">
                    <div class="btn btn-defaultMainSite">
                        <span>Прикрепить файл...</span>
                        <input ref="fileInput" type="file" name="UploadedFiles" multiple>
                    </div>
                    <div class="file-path-wrapper" style="flex-grow: 1;">
                        <input disabled class="file-path" style="border: none;color: #65935C;" type="text" placeholder="Список выбранных файлов">
                    </div>
                    <!--<button class="btn btn-defaultMainSite" >Опубликовать</button>-->
                    <input type="submit" class="btn btn-defaultMainSite" value="Опубликовать" />
                </div>
            </form>
        </div>
        <div v-bind:class="classObject" class="creator-menu">
            <button class="btn btn-defaultMainSite" @click="showCreater(true, false)">Новое сообщение</button>
        </div>
    </div>
</template>

<script>
    import { mapActions } from 'vuex';

    export default {
        name: "ms-creater_news-item",
        props: {
            categoryId: {
                type: String,
                default: () => { return '' }
            }
        },
        data: () => {
            return {
                classObject: {
                    'active': false,
                    'desActive': false
                },
                model: {
                    CategoryId: ''
                }
            }
        },
        methods: {
            ...mapActions([
                'CREATE_NEW'
            ]),
            showCreater(active, desActive) {
                this.classObject['active'] = active;
                this.classObject['desActive'] = desActive;
            },
            createFormData() {
                let formData = new FormData();

                for (var item in this.model) {
                    formData.append(item, this.model[item]);
                }

                for (var i = 0; i < this.$refs.fileInput.files.length; i++) {
                    let file = this.$refs.fileInput.files[i];
                    formData.append('UploadedFiles' + "[" + i + "]", file);
                }

                return formData;
            },
            submit(e) {
                e.preventDefault();
                this.model.CategoryId = this.categoryId;

                //let formData = this.createFormData();
                let formData = new FormData(this.$refs.formCreateNews);

                let result = {
                    params: formData,
                    action: e.target.action
                };

                this.CREATE_NEW(result);
            }
        }
    }
</script>

<style>

</style>
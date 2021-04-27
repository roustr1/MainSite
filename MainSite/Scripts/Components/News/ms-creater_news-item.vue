<template>
    <div class="creator">
        <div class="card-panel creator-main" v-bind:class="classObject">
            <a class="error close" @click="showCreater(false, true)"><i class="material-icons">close</i></a>
            <a class="changeEditor" @click="changeEditor" style="cursor:pointer;"><i title="Поменять режим ввода" class="material-icons">swap_horiz</i></a>

            <form ref="formCreateNews" @submit="submit" action="/Home/Create/" enctype="multipart/form-data" method="post">
                <input name="CategoryId" type="hidden" :value="this.categoryId" />
                <input name="IsAdvancedEditor" type="hidden"  :value="this.isAdvancedEditor"/>
                <div class="s12 m12">
                    <div class="bold">Введите заголовок объявления:</div>
                    <input v-model="model.Header" name="Header" id="TextHeader" class="inputTextMainSite" type="text" />
                </div>
                <template v-if="isAdvancedEditor">
                    <ms-wysiwyg 
                      v-model="textEditor"
                      :fileList="fileList" 
                      @changeFileList="changeFileList"
                    />
                    <input type="hidden" v-model="textEditor" name="Description" id="TextDescription"/>
                    <input style="display: flex; margin-left: auto;" type="submit" class="btn btn-defaultMainSite" value="Опубликовать" />
                </template>
                <template v-else>
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
                        <input type="submit" class="btn btn-defaultMainSite" value="Опубликовать" />
                    </div>
                </template>
            </form>
        </div>
        <div v-bind:class="classObject" class="creator-menu">
            <button class="btn btn-defaultMainSite" @click="showCreater(true, false)">Новое сообщение</button>
        </div>
    </div>
</template>

<script>
    import { mapActions } from 'vuex';
    import MsWysiwyg from '../../DefaultComponents/ms-wysiwyg.vue';

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
                },
                textEditor: '',
                fileList: [],
                isAdvancedEditor: true
            }
        },
        computed: {
        },
        components: {
            MsWysiwyg
        },
        methods: {
            ...mapActions('news',[
                'CREATE_NEW'
            ]),
            changeEditor() {
                this.isAdvancedEditor = !this.isAdvancedEditor;
            },
            changeFileList(changeFileListData) {
                this.fileList = changeFileListData;
            },
            showCreater(active, desActive) {
                this.classObject['active'] = active;
                this.classObject['desActive'] = desActive;
            },
            submit(e) {
                e.preventDefault();
                this.model.CategoryId = this.categoryId;
                
                let formData = new FormData(this.$refs.formCreateNews);
                if (this.isAdvancedEditor) {
                    let i = 0;
                    for (var i = 0; i < this.fileList.length; i++) {
                        formData.append(this.fileList[i].Id, this.fileList[i].FormFile, this.fileList[i].FormFile.name);
                    }
                }

                let result = {
                    params: formData,
                    action: e.target.action
                };

                this.CREATE_NEW(result);
            }
        }
    }
</script>

<style lang="scss">
    .changeEditor {
        position: absolute;
        right: 35px;
        cursor: pointer;
        z-index: 10;
        top: 0px;
    }
</style>
<template>
    <form ref="formCreateNews" @submit="submit" action="/Home/Create/" enctype="multipart/form-data" method="post">
        <input name="CategoryId" type="hidden" :value="categoryId" />
        <input name="IsAdvancedEditor" type="hidden" :value="this.isAdvancedEditor" />
        <div class="s12 m12">
            <div class="bold">Введите заголовок объявления:</div>
            <input v-model="model.Header" name="Header" id="TextHeader" class="inputTextMainSite" type="text" />
        </div>
        <template v-if="isAdvancedEditor">
            <ms-wysiwyg v-model="textEditor"
                        :fileList="fileList"
                        @changeFileList="changeFileList" />
            <input type="hidden" v-model="textEditor" name="Description" id="TextDescription" />
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
                    <input ref="fileInput" type="file" name="UploadedFiles" multiple />
                </div>
                <div class="file-path-wrapper" style="flex-grow: 1;">
                    <input ref="fileInputNameList" disabled class="file-path" style="border: none;color: #65935C;" type="text" placeholder="Список выбранных файлов">
                </div>
                <input type="submit" class="btn btn-defaultMainSite" value="Опубликовать" />
            </div>
        </template>
    </form>
</template>

<script>
    import MsWysiwyg from '../../DefaultComponents/ms-wysiwyg.vue';

    export default {
        name: 'ms-change_news-form',
        props: {
            isAdvancedEditor: {
                type: Boolean,
                default: () => { return false }
            },
            categoryId: {
                type: String,
                default: () => { return '' }
            }
        },
        data() {
            return {
                model: {},
                textEditor: '',
                fileList: []
            }
        },
        components: {
            MsWysiwyg
        },
        methods: {
            changeFileList(changeFileListData) {
                this.fileList = changeFileListData;
            },
            submit(e) {
                e.preventDefault();
                this.model.CategoryId = this.CategoryId;

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

                this.$emit('createNew', result);
                

                this.fileList = [];
                if (!this.isAdvancedEditor) {
                    this.model = {};
                    this.$refs.fileInputNameList.value = '';
                }
            }
        }
    }
</script>

<style lang="scss" scoped>

</style>
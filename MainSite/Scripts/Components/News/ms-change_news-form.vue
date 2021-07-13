<template>
  <form ref="formCreateNews" v-on:submit.prevent="submit" action="/Home/Create/" enctype="multipart/form-data" method="post">
    <input name="Id" type="hidden" v-model="model.id" />
    <input name="CategoryId" type="hidden" :value="categoryId" />
    <input name="IsAdvancedEditor" type="hidden" :value="isAdvancedEditor" />

    <div class="s12 m12">
      <div class="bold">Введите заголовок объявления:</div>
      <input v-model="model.header" name="Header" id="TextHeader" class="inputTextMainSite" type="text" />
    </div>
    <ms-wysiwyg v-model="textEditor"
      :fileList="fileList"
      :parentTextEditor="getDescription"
      @changeFileList="changeFileList" />
    <input type="hidden" v-model="textEditor" name="Description" id="TextDescription" />
    <input style="display: flex; margin-left: auto;" type="submit" class="btn btn-defaultMainSite" :value="textSubmit" />
  </form>
</template>

<script>
  import MsWysiwyg from '../../DefaultComponents/ms-wysiwyg.vue';
  import { mapActions } from 'vuex';
  
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
      },
      textSubmit: {
        type: String,
        default: () => { return 'Опубликовать' }
      },
      editModel: {
        type: Object,
        default: () => { return null }
      },
      editFiles: {
        type: Array,
        default: () => { return [] }
      }
    },
    data: () => {
      return {
        model: {},
        textEditor: '',
        fileList: []
      }
    },
    computed: {
      getDescription() {
        if (this.editModel != null) return this.editModel.description;

        return "";
      }
    },
    components: {
      MsWysiwyg
    },
    methods: {
      ...mapActions('news', [
        'GET_FILE'
      ]),
      GetEditFileNameInput(index, key) {
        return "Files[" + index + "]." + key;
      },
      changeFileList(changeFileListData) {
        this.fileList = changeFileListData;                         
      },
      GenerateUUID() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
          var r = Math.random()*16|0, v = c == 'x' ? r : (r&0x3|0x8);
          return v.toString(16);
        }).toUpperCase();
      },
      submit(e) {
        e.preventDefault();
        this.model.CategoryId = this.CategoryId;

        let formData = new FormData(this.$refs.formCreateNews);

        if (this.isAdvancedEditor) {
          let i = 0;
          for (var i = 0; i < this.fileList.length; i++) {
            if(typeof this.fileList[i].dataBaseName == 'undefined') {
              formData.append(`Files[${this.GenerateUUID()}]`, this.fileList[i]);  
            }
            else {
              let dataBaseName = typeof this.fileList[i].dataBaseName != 'undefined' ? this.fileList[i].dataBaseName : this.GenerateUUID();
              let file = new File([], this.fileList[i].name + this.fileList[i].extension, { type: this.fileList[i].mimeType, name: dataBaseName})
              if(this.fileList[i].dataBaseName.includes('Files')) {
                formData.append(dataBaseName, file);  
              }
              else {
                formData.append(`Files[${this.GenerateUUID()}]`, file);  
              }
            }                
          }
        }

        let result = {
          params: formData,
          action: e.target.action
        };

        this.$emit('changeNew', result);
      
        this.fileList = [];
        this.$refs.formCreateNews.reset();
        this.model = {};
        if (!this.isAdvancedEditor && this.editModel == null) {
          this.$refs.fileInputNameList.value = '';
        }
      }
    },
    created() {
        if (this.editModel != null) this.model = this.editModel;
        if(this.editFiles.length) {
          this.fileList = this.editFiles.map((item, index) => {
            //let element = this.GET_FILE(item.id)
            //console.log(item)
            //let file = new File([element], item.name + item.extension, { type: item.mimeType, name: item.dataBaseName})

            return item
          })
        }
    }
  }
</script>

<style lang="scss" scoped>

</style>
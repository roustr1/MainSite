<template>
  <div class="uploadedFiles_item" :title="fileItem.name">
    <div class="uploadedFiles_item__content">
      <div class="uploadedFiles_item__content-delete" @click="deleteFile">
        <i class="material-icons">close</i>
      </div>
      <div class="uploadedFiles_item__content__body">
        <div class="uploadedFiles_item__content__body-name">
          {{fileItem.name}}
        </div>
        <div class="uploadedFiles_item__content__body-icone">
          <img 
            v-bind:src="getIcon" 
            width="50" 
            height="50" 
          />
        </div>
        <div class="uploadedFiles_item__content__body-size">
          {{formatBytes(fileItem.size, 1)}}
        </div>
      </div>
    </div>                      
  </div>
</template>

<script>
export default {
    name:'ms-loader-item',
    props:{
      fileItem: {
        type: [Object, File],
        default: () => { return {} }
      },
    },
    data: () => {
      return {
        iconPathList: {
          'image': '/images/layout_icons/type_files/file-image.png',
          'pdf': '/images/layout_icons/type_files/file-pdf.png',
          'word': '/images/layout_icons/type_files/file-word.png',
          'excel': '/images/layout_icons/type_files/file-excel.png',
          'spreadsheetml': '/images/layout_icons/type_files/file-excel.png'
        },
        currentIconType: ''
      }
    },
    computed: {
      getIcon() {
        if(this.fileItem.type) {
          for(let key in this.iconPathList) {
            if(this.fileItem.type.includes(key)) {
              return this.iconPathList[key]
            }
          }
        }

        return '/images/layout_icons/type_files/file-document.png'
      }
    },
    methods: {
      formatBytes(bytes,decimals = 0) {
        if(typeof bytes == 'undefined') return ''
        if(bytes == 0) return '0 B'
        let k = 1024,
          dm = decimals <= 0 ? 0 : decimals || 2,
          sizes = ['B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
          i = Math.floor(Math.log(bytes) / Math.log(k))
        return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i]
      },
      showBtnNextOverflow() {
        let blockUploadedFiles = document.getElementsByClassName('uploadedFiles')[0]
        let blockUploadedFilesList = document.getElementsByClassName('uploadedFiles_list')[0]
        let btn = document.getElementsByClassName('uploadedFiles_next')[0]
        if(blockUploadedFiles.offsetWidth < blockUploadedFilesList.scrollWidth) {
          btn.classList.remove('uploadedFiles_hidden')
          btn.classList.add('uploadedFiles_vissible')
        }
      },
      deleteFile() {
        this.$emit('deleteFile', this.fileItem)
      }
    },
    mounted() {
      this.showBtnNextOverflow()
    }
}
</script>

<style lang="scss">
    .uploadedFiles_item {
        margin-right: 20px;    
        padding-bottom: 5px;
        width: 140px;
        &__content {
            width: 128px;
            height: 96px;
            border: 1px solid #9e9e9e;
            border-radius: 8px;
            padding: 5px;
            background-color: #eeeeee;
            color:black;
            font-size:14px;
            position: relative;
            &-delete {
                cursor: pointer;
                display: flex;
                align-items: center;
                justify-content: center;
                position: absolute;
                top: 0;
                right: -10px;
                transform: translateY(-10px);
                background-color: white;
                width: 24px;
                height: 24px;
                border-radius: 24px;
                border: 1px solid #9e9e9e;

                i {
                    line-height: 16px;
                    font-size: 15px;
                }
            }
            &__body {
                display: flex;
                overflow: hidden;
                flex-direction: column;
                flex-wrap: nowrap;
                &-name {
                    white-space: nowrap;
                }
                &-icone {
                    white-space: nowrap;
                    text-align: center;
                }
                &-size {
                    opacity: 0;
                    background: transparent;
                    position: absolute;
                    left: 0;
                    right: 0;
                    bottom: 0;
                    box-sizing: border-box;
                    height: 52px;
                    padding: 12px 6px 0;
                    background-image: linear-gradient(to bottom, rgba(255, 255, 255, 0), rgba(245, 245, 245, 0.8) 30%, #f0f0f0);
                    display: inline-flex;
                    align-items: center;
                }
            }

            &:hover {
                & .uploadedFiles_item__content__body-size {
                    opacity: 1;
                }
            }
        }
    }
</style>
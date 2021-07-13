<template>
    <div>
        <div class="file-field input-field" style="display:inline-block;">
            <div class="btn-attachFile">
                <i class="material-icons">attach_file</i> Прикрепить файл
                <input ref="uploadedFiles" type="file" multiple />
                <input class="file-path" type="hidden" />
            </div>
        </div>
        <div class="uploadedFiles">
            <div class="uploadedFiles_list">
                <ms-loader-item
                    v-for="(item, index) in files"
                    :key="index" 
                    :fileItem="item"
                    @deleteFile="deleteFile"
                />
            </div>
            <div class="uploadedFiles_next uploadedFiles_hidden">
                <button type="button" class="uploadedFiles__btn" @click="nextFiles">
                    <div class="uploadedFiles__btn-icone">
                        <div class="">
                            <svg width="16" height="16" viewBox="0 0 16 16" class="base-0-2-1" ie-style="">
                                <path fill-rule="evenodd" d="M6.635 11.772A1 1 0 015 11V5a1 1 0 011.635-.772l3.954 2.964a.999.999 0 010 1.616l-3.954 2.964z"></path>
                            </svg>
                        </div>
                    </div>
                </button>
            </div>
            <div class="uploadedFiles_previous uploadedFiles_hidden" @click="previousFiles">
                <button type="button" class="uploadedFiles__btn">
                    <div class="uploadedFiles__btn-icone">
                        <div class="">
                            <svg width="16" height="16" viewBox="0 0 16 16" class="base-0-2-1" ie-style="">
                                <path fill-rule="evenodd" d="M9.365 4.228A1 1 0 0111 5v6a1 1 0 01-1.635.772L5.411 8.808a.999.999 0 010-1.616l3.954-2.964z"></path>
                            </svg>
                        </div>
                    </div>
                </button>
            </div>
        </div>
        <div style="font-size:14px" v-if="files.length">Количество файлов: {{files.length}}, Общий размер: {{getAllSizeFiles}} <a href="" @click.prevent="deleteAllFiles">удалить файлы</a></div>
    </div>
</template>

<script>
import msLoaderItem from './ms-loader-item.vue'
export default {
    name:'ms-loader-files',
    props: {
        fileList: {
            type: Array,
            default: () => { return [] }
        }
    },
    data: () => {
        return {
            translateValue : 0,
            constScrollValue: 140,
            differenceWidthBlock: 0,
            btnPrevious: {},
            btnNext: {},
            files: []
        }
    },
    components: {
        msLoaderItem
    },
    computed: {
        getAllSizeFiles() {
            if(this.files.length == 0) return '0 b'

            let allSumSize = this.files.reduce((sum, item) => {
                if(typeof item.size == 'undefined') return 0
                return sum + item.size
            }, 0)
            
            return this.formatBytes(allSumSize)
        }
    },
    methods: {
        formatBytes(bytes,decimals = 0) {
            if(bytes == 0) return '0 B'
            let k = 1024,
                dm = decimals <= 0 ? 0 : decimals || 2,
                sizes = ['B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
                i = Math.floor(Math.log(bytes) / Math.log(k))
            return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i]
        },
        nextFiles() {
            if(this.translateValue < 0) this.visibleBtn(this.btnPrevious)

            this.transformUploadedFileList(this.constScrollValue * -1)
        },
        previousFiles() {
            this.transformUploadedFileList(this.constScrollValue)
            if(this.translateValue == 0) {
                this.hiddenBtn(this.btnPrevious)
                this.visibleBtn(this.btnNext)
            }
            else if (this.btnNext.classList.contains('uploadedFiles_hidden')) {
                this.visibleBtn(this.btnNext)
            }
        },
        visibleBtn(btn) {
            btn.classList.remove('uploadedFiles_hidden')
            btn.classList.add('uploadedFiles_vissible')
        },
        hiddenBtn(btn) {
            btn.classList.remove('uploadedFiles_vissible')
            btn.classList.add('uploadedFiles_hidden')
        },
        transformUploadedFileList(value) {
            let blockUploadedFilesList = document.getElementsByClassName('uploadedFiles_list')[0]
            blockUploadedFilesList.style.transform = `translateX(${this.translateValue + value}px)`
            this.translateValue += value

            if(this.translateValue < 0) this.visibleBtn(this.btnPrevious)    
            if( (blockUploadedFilesList.scrollWidth - blockUploadedFilesList.offsetWidth) * -1 >= this.translateValue) {
                this.hiddenBtn(this.btnNext)
            }
        },
        deleteFile(fileItem) {
            this.files = this.files.filter(item => item != fileItem)
            this.changeFileList()

            let blockUploadedFilesList = document.getElementsByClassName('uploadedFiles_list')[0]
            let maxCountItem = Math.floor(blockUploadedFilesList.offsetWidth / 140)
                      
            if(this.files.length <= maxCountItem) {
                this.hiddenBtn(this.btnNext)
                this.hiddenBtn(this.btnPrevious)
                this.translateValue = 0
                blockUploadedFilesList.style.transform = 
                    `translateX(${this.translateValue}px)`
            }
            else {
                if(this.translateValue != 0) {
                    blockUploadedFilesList.style.transform = 
                        `translateX(${this.translateValue + 140}px)`
                    this.translateValue += this.constScrollValue

                    if(this.translateValue == 0) this.hiddenBtn(this.btnPrevious)
                }
            }              
        },
        deleteAllFiles() {
            this.files = []
            this.changeFileList()
        },
        changeFileList() {
            this.$emit('changeFileList', this.files)
        }
    },
    mounted() {
        this.files = [...this.fileList]
        let vm = this
        this.btnNext = document.getElementsByClassName('uploadedFiles_next')[0]
        this.btnPrevious = document.getElementsByClassName('uploadedFiles_previous')[0]

        this.$refs.uploadedFiles.addEventListener('change', function(el) {
            vm.files = [...vm.files,...el.target.files]
            vm.changeFileList()
        })
    }
}
</script>

<style lang="scss">

    .file-field {
        margin: 0;
    }

    .btn-attachFile{
        cursor: pointer;
        display: flex;
        align-items: center;
        background: transparent;
        border: none;
        padding: 10px 10px 10px 0px;
        color: #9e9e9e;
        &:hover {
            background: rgba(0,0,0,.04);
        }
    }

    .uploadedFiles {
        padding-top: 10px;
        overflow: hidden;
        position: relative;

        &_list {
            display: flex;
            transition: 1s transform;
        }

        &_visible {
            opacity: 1;
            visibility: visible;
            /*transition: all .2s ease-in;*/
        }

        &_hidden {
            opacity: 0;
            visibility: hidden;
            /*transition: all .2s ease-out;*/
        }

        &_previous, &_next {
            position: absolute;
            top: 0;
            bottom: 0;
            width: 40px;
            background: rgba(255,255,255,.88);
        }

        &_next {
            right: 0;
        }

        &_previous {
            left: 0;
        }

        &__btn, &__btn:focus {
            border: 0;
            cursor: pointer;
            display: inline-flex;
            flex-direction: row;
            align-items: center;
            text-align: left;
            position: relative;
            overflow: hidden;
            background: transparent;
            color: #333;
            fill: #333;
            transition: background-color .1s ease-in;
            width: 100%;
            height: 100%;
        }
    }

</style>
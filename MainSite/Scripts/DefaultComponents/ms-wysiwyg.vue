<template>
    <div class='texteditor'>
        <div class='banner'>
            <div id="toolBar1">
                <ms-loader-files
                    @changeFileList="changeFileList"
                    :fileList="fileList"
                />
            </div>
            <div id="toolBar2">
                <i class="material-icons intLink" title="Очистить" @click="clear" onmousedown="return false" onselectstart="return false">cleaning_services</i>
                <i class="material-icons intLink" title="Назад" @click="formatDoc('undo');" onmousedown="return false" onselectstart="return false">undo</i>
                <i class="material-icons intLink" title="Вперед" @click="formatDoc('redo');" onmousedown="return false" onselectstart="return false">redo</i>
                <i title="Жирный" class="material-icons intLink" @click="formatDoc('bold');" onmousedown="return false" onselectstart="return false">format_bold</i>
                <i title="Курсивный" class="material-icons intLink" @click="formatDoc('italic');" onmousedown="return false" onselectstart="return false">format_italic</i>
                <i title="Подчеркнутый" class="material-icons intLink" @click="formatDoc('underline');" onmousedown="return false" onselectstart="return false">format_underline</i>
                <i title="Выравнивание с лева" class="material-icons intLink" @click="formatDoc('justifyleft');" onmousedown="return false" onselectstart="return false">format_align_left</i>
                <i title="Выравнивание по центру" class="material-icons intLink" @click="formatDoc('justifycenter');" onmousedown="return false" onselectstart="return false">format_align_center</i>
                <i title="Выравнивание с права" class="material-icons intLink" @click="formatDoc('justifyright');" onmousedown="return false" onselectstart="return false">format_align_right</i>
                <i title="Нумерованный список" class="material-icons intLink" @click="formatDoc('insertorderedlist');" onmousedown="return false" onselectstart="return false">format_list_numbered</i>
                <i title="Маркированный список" class="material-icons intLink" @click="formatDoc('insertunorderedlist');" onmousedown="return false" onselectstart="return false">format_list_bulleted</i>
                <i title="Блок с цитатой" class="material-icons intLink" @click="formatDoc('formatblock','blockquote');" onmousedown="return false" onselectstart="return false">format_quote</i>
                <i title="Уменьшить на единицу отступ блока форматирования" class="material-icons intLink" @click="formatDoc('outdent');" onmousedown="return false" onselectstart="return false">format_indent_decrease</i>
                <i title="Увеличить на единицу отступ блока форматирования" class="material-icons intLink" @click="formatDoc('indent');" onmousedown="return false" onselectstart="return false">format_indent_increase</i>
                <i title="Ссылка" class="material-icons intLink" @click="setLink" onmousedown="return false" onselectstart="return false">link</i>
                <label class="file_loader_label">
                    <input ref="file" type="file" id="inputFileToLoad" />
                    <i title="Добавить картинку" class="material-icons intLink" onmousedown="return false" onselectstart="return false">image</i>
                </label>
                <ms-select 
                    :selected="formatBlockSelected.name"
                    :options="formatBlockList"
                    @select="changeSelectFormatBlock" 
                    :isCursorEdit="true"
                />
            </div>
        </div>
        <div class='holder'>
            <div v-on:input="changeTextEditor" contentEditable="true" name='wysiwyg' class="wysiwyg" :id='GUUID' v-html="parentTextEditor"></div>
        </div>
    </div>
</template>

<script>
    import msPopup from "./ms-popup.vue"
    import msSelect from './ms-select.vue'
    import msLoaderFiles from './ms-loader-files.vue'
    export default {
        name: "ms-wysiwyg",
        props: {
            parentTextEditor: {
                type: String,
                default: () => { return '' }
            },
            fileList: {
                type: Array,
                default: () => { return [] }
            }
        },
        data() {
            return {
                editor: {},
                textMessage: '',
                isInfoPopupVisible: false,
                actionLoad: '',
                isImage: false,
                formatBlockList: [{ name: "Обычный текст", value: "span" }, { name: "Название темы", value: "h1" },
                 { name: "Название раздела", value: "h2" }, { name: "Название подраздела", value: "h3" }
                ],
                formatBlockSelected: { name: "Обычный текст", value: "span" },
                fileListDropDownSelected: { name: "Список файлов &#129131;", value: "" },
                currentImage: {}
            }
        },
        components: {
            msPopup,
            msSelect,
            msLoaderFiles
        },
        computed: {
            GUUID() {
                return "wysiwyg_" + new Date();
            },
            formListDropDown() {
                return this.fileList.map(function(item, index){
                    return { name: item.FormFile.name, value:item.Id}
                })
            }
        },
        methods: {   
            changeTextEditor() {
                this.$emit('input', this.editor.innerHTML);
            },
            loadIframe() { 
                this.editor = document.getElementById(this.GUUID);
                if (this.parentTextEditor !== '') this.$emit('input', this.parentTextEditor);
                let vm = this;

                this.editor.addEventListener('click', function (e) {
                    e.preventDefault();
                    if (e.target.tagName === 'IMG' && !e.target.getAttribute('_moz_resizing')) {
                        if (Object.keys(vm.currentImage).length == 0) {
                            let blockImg = document.createElement('div');
                            blockImg.classList.add('resize');
                            blockImg.setAttribute('contentEditable', false);
                            blockImg.style.width = e.target.style.width;
                            blockImg.style.height = e.target.style.height;

                            e.target.style.width = '';
                            e.target.style.height = '';
                            e.target.style.margin = '';

                            blockImg.appendChild(e.target.cloneNode());
                            if ("replaceNode" in e.target) {
                                // for IE
                                e.target.replaceNode(blockImg);
                            }
                            else {
                                // for other browsers
                                e.target.parentNode.replaceChild(blockImg, e.target);
                            }
                            vm.currentImage.data = blockImg;
                        }
                        else {
                            let newImg = vm.currentImage.data.querySelector('img').cloneNode();
                            newImg.style.width = vm.currentImage.data.style.width;
                            newImg.style.height = vm.currentImage.data.style.height;

                            vm.currentImage.data.parentNode.replaceChild(newImg, vm.currentImage.data);
                            vm.currentImage = {};
                        }

                        return false;
                    }

                    return false;
                }, false);
            },
            formatDoc(sCmd, sValue) {
                document.execCommand(sCmd, false, sValue);
                this.editor.focus();
            },
            changeSelectFormatBlock(selectDropDownElement) {                       
                this.formatBlockSelected = selectDropDownElement;
                var div = document.createElement('div');
                let text = window.getSelection().toString()
                if( selectDropDownElement.value === 'span') {
                    let checkContainsHeader = 
                        document.getSelection().anchorNode.parentElement.getAttribute('name') != 'wysiwyg' &&
                        document.getSelection().anchorNode.parentElement.tagName != 'SPAN'
                    if(checkContainsHeader) {
                        window.getSelection().anchorNode.parentElement.remove()
                    }
                }
                
                div.innerHTML = "<" + selectDropDownElement.value + ">" + text + "</" + selectDropDownElement.value + ">";
                this.formatDoc("insertHTML", div.innerHTML);
            },
            addFileForBody(file) {
                if (file) {
                    let vm = this;
                    let reader = new FileReader(file);
                    
                    reader.readAsDataURL(file);
                    if (file.type.match("image.*")) {
                        reader.onload = async function (e) {   
                            e.preventDefault();                     
                            let img = await new Promise((resolve, reject) => {
                                let elementImg = document.createElement('img')
                                let unicId = 'image_' + Date.now().toString();

                                elementImg.onload = () => resolve(elementImg)
                                elementImg.setAttribute('id', unicId);
                                elementImg.src = e.target.result;
                                //vm.fileList.push({ Id: unicId, FormFile: file });
                            });

                            let imgWidth = img.width;
                            let imgHeight = img.height;

                            let scale = imgWidth / imgHeight;
                            let tempHeight = vm.editor.offsetHeight * 3 / 5 - 30
                            let tempWidth = tempHeight * scale

                            
                            img.width = tempWidth;                        
                            img.height = tempHeight;
                            
                            vm.formatDoc("insertHTML", img.outerHTML);
                        }                        
                    }   
                }
                else {
                }
            },
            changeFileList(changeFileListData) {
                this.$emit('changeFileList', changeFileListData);
            },
            clear() {
                if (confirm('Очистить всю область?')) { this.editor.innerHTML = "" };
            },
            changeFormatBlockSelected() {
                this.formatDoc('formatblock', this.formatBlockSelected);
            },
            setLink() {
                let sLnk = prompt('Напишите ссылку', '');
                if (sLnk && sLnk != '') {
                    this.formatDoc('createlink', sLnk);
                }
            },
            listenerImageLoader() {
                let imageLoader = document.querySelector('.file_loader_label > input[type="file"]')
                let vm = this
                imageLoader.addEventListener('change', function() {
                    vm.addFileForBody(this.files[0])
                })
            }
        },
        beforeDestroy() {
            this.editor = {};
            this.formatBlockSelected =  "Обычный текст";
            this.changeTextEditor();
            this.$emit('changeFileList', []);
        },
        mounted() {
            this.loadIframe()
            this.listenerImageLoader()
        }
    };
</script>

<style lang="scss">
    .file_loader_label {
        input[type="file"] {
            display: none;
        }
    }

    #toolBar1 {
        margin-top: 10px;
    }

    #toolBar2 {
        display: flex;
        justify-content: space-between;
        align-items: center;

        & .ms-select {
            width: 170px;
            font-size: 14px;
            & .title {
                border-radius: 8px;
                line-height: 1;
            }
        }
    }

    .resize {
        position: relative;
        resize: both;
        overflow: hidden;
        width: 25px;
        height: 25px;
        display: inline-block;
        border: 1px solid black;
    }

    .resize > img {
        width: 100%;
        height: 100%;
    }

    .wysiwyg {
        padding: 15px;
        border: 1px solid #9e9e9e;
        border-radius: 8px;
        background-color: #eeeeee;
        color: #9e9e9e;
        height:400px;
        margin-bottom: 15px;
        overflow-y:scroll;
        &:focus {
           background-color: white;
        }
    }

    .wysiwyg {
        & ul, ol
        {
            padding-inline-start: 40px;
        }

        & ul {
            list-style: disc !important;
            & > li
            {
                list-style: disc !important;
                text-align:left;
            }
        }
    }

    .intLink {
        cursor: pointer;
        color: black;
    }

    img.intLink {
        border: 0;
    }


    #textBox {
        width: 540px;
        height: 200px;
        border: 1px #000000 solid;
        padding: 12px;
        overflow: scroll;
    }

    #textBox #sourceText {
        padding: 0;
        margin: 0;
        min-width: 498px;
        min-height: 200px;
    }

    #editMode label {
        cursor: pointer;
    }
</style>

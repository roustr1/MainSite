<template>
    <div class='texteditor'>
        <ms-popup 
            v-if="isInfoPopupVisible"
            @closePopup="closePopupInfo"
            rightBtnTitle="Добавить"
            @rightBtnAction="addImage"
            :popupTitle="popuTitle"
            :isMouseDown="true"
        >
            <template v-slot:default>
                <input ref="file" type="file" id="inputFileToLoad" />
            </template>      
        </ms-popup>
        <div class='banner'>
            <div id="toolBar1">
                <ms-select 
                    :selected="formatBlockSelected"
                    :options="formatBlockList"
                    @select="changeSelectFormatBlock" 
                    :isCursorEdit="true"
                />
                <ms-select 
                    :selected="sizeSelected"
                    :options="sizeList"
                    @select="changeSelectSize" 
                    :isCursorEdit="true"
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
                <i title="Добавить картинку" class="material-icons intLink" @click="showPopupInfo(true)" onmousedown="return false" onselectstart="return false">image</i>
                <i title="Привязать файл" class="material-icons intLink" @click="showPopupInfo(false)" onmousedown="return false" onselectstart="return false">upload</i>
            </div>
        </div>
        <div class='holder'>
            <div v-on:input="changeTextEditor" contentEditable="true" name='wysiwyg' class="wysiwyg" :id='GUUID' v-html="parentTextEditor"></div>
        </div>
    </div>
</template>

<script>
    import msPopup from "./ms-popup.vue";
    import msSelect from './ms-select.vue'
    export default {
        name: "ms-wysiwyg",
        props: {
            fileList: {
                type: Array,
                default: () => { return [] }
            },
            parentTextEditor: {
                type: String,
                default: () => { return '' }
            }
        },
        data() {
            return {
                editor: {},
                textMessage: '',
                isInfoPopupVisible: false,
                actionLoad: '',
                isImage: false,
                formatBlockList: [{ name: "-Формат-", value: "p" }, { name: "H1", value: "h1" }, { name: "H2", value: "h2" }, { name: "H3", value: "h3" },
                    { name: "H4", value: "h4" }, { name: "H5", value: "h5" }, { name: "SubTitle", value: "h6" }, { name: "Paragraph", value: "p" }
                ],
                formatBlockSelected: "-Формат-",
                sizeList: [{ name: "-Размер шрифта-", value: "3" }, { name: "10px", value: "1" }, { name: "12px", value: "2" }, { name: "14px", value: "3" },
                    { name: "16px", value: "4" }, { name: "18px", value: "5" }, { name: "21px", value: "6" }, { name: "26px", value: "7" },
                ],
                sizeSelected: "-Размер шрифта-",
                currentImage: {}
            }
        },
        components: {
            msPopup,
            msSelect
        },
        computed: {
            popuTitle() {
                if (this.isImage) {
                    return 'Добавить картинку';
                }
                else {
                    return 'Добавить файл';
                }
            },
            GUUID() {
                return "wysiwyg_" + new Date();
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
            changeSelectFormatBlock(selectElement) {
                this.formatBlockSelected = selectElement.name;
                if (document.getSelection().anchorNode.nodeValue) {
                    let html = `<${selectElement.value}>${document.getSelection().anchorNode.nodeValue}</${selectElement.value}>`
                    this.formatDoc("insertHTML", html);
                }
            },
            changeSelectSize(selectElement) {
                this.sizeSelected = selectElement.name;
                this.formatDoc("fontsize", selectElement.value);
            },
            addImage() {
                this.addFileForBody(this.$refs.file.files[0]);
                //Тестовый момент по удалению из request
                this.closePopupInfo();
            },
            showPopupInfo(isImage) {
                this.isImage = isImage;
                this.isInfoPopupVisible = true;
            },
            closePopupInfo() {
                this.isInfoPopupVisible = false;
            },
            addFileForBody(file) {
                if (file) {
                    let vm = this;
                    let reader = new FileReader(file);
                    reader.readAsDataURL(file);
                    if (file.type.match("image.*") && vm.isImage) {
                        let elementImg = document.createElement('img');
                        reader.onload = function (e) {
                            e.preventDefault();
                            let unicId = 'image_' + Date.now().toString();
                            //vm.fileList.push({ Id: unicId, FormFile: file });

                            elementImg.setAttribute('src', e.target.result);
                            //elementImg.style.width = '200px';
                            //elementImg.style.height = '200px';
                            elementImg.setAttribute('id', unicId);

                            vm.formatDoc("insertHTML", elementImg.outerHTML);
                        }

                    }
                    else if (!vm.isImage) {
                        reader.onload = function (e) {
                            e.preventDefault();
                            let unicId = 'file_' + Date.now().toString();
                            vm.fileList.push({ Id: unicId, FormFile: file });

                            let a = document.createElement('a');
                            a.setAttribute('href', unicId);
                            a.innerHTML = file.name;

                            vm.formatDoc("insertHTML", a.outerHTML);
                        }
                    }
                    
                }
                else {
                }
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
            }
        },
        beforeDestroy() {
            this.editor = {};
            this.formatBlockSelected =  "-Формат-";
            this.sizeSelected = "-Размер шрифта-";
            this.changeTextEditor();
            this.$emit('changeFileList', []);
        },
        mounted() {
            this.loadIframe();
        }
    };
</script>

<style lang="scss">
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

    #toolBar1 {
        display: flex;
        justify-content: space-between;
    }

    #toolBar1 select {
        font-size: 10px;
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

<template>
    <div class="popup_wrapper" ref="popup_wrapper">
        <div class="ms-popup">
            <div class="ms-popup_header">
                <span class="ms-popup_header__title">{{popupTitle}}</span>
                <span @click="closePopup"
                      class="close"><i class="material-icons">cancel</i></span>
            </div>
            <div class="ms-popup_content">
                <slot :inputModel="inputModel">
                    <p>Ошибка вызова модального окна</p>
                </slot>
            </div>
            <div class="ms-popup_footer">
                <button class="btn btn-default"
                        @click="closePopup">
                    Закрыть
                </button>
                <button class="btn btn-default"
                        @click="rightBtnAction">
                    {{rightBtnTitle}}
                </button>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name:'ms-popup',
    props:{
        rightBtnTitle: {
            type: String,
            default: 'Ok'
        },
        popupTitle: {
            type: String,
            default: 'PopupName'
        },
        isMouseDown: {
            type: Boolean,
            default: false
        }
    },
    data: () => {
        return {}
    },
    methods: {
        closePopup() {
            this.$emit("closePopup")
        },
        rightBtnAction() {
            this.$emit("rightBtnAction")
        }
    },
    mounted() {
        if (this.isMouseDown) {
            document.querySelector('.popup_wrapper').addEventListener('mousedown', function (e) {
                e.preventDefault();
            });
        }
    }
}
</script>

<style lang="scss">
    .popup_wrapper {
        display: flex;
        justify-content: center;
        align-items: center;
        position: absolute;
        right: 0;
        left: 0;
        top: 0;
        bottom: 0;
    }

    .ms-popup {
        border-radius: 8px;
        z-index: 10;
        padding: 16px;
        position: fixed;
        top: 150px;
        width: 500px;
        padding: 10px;
        background: #fff;
        box-shadow: 0 0 17px 0 #e7e7e7;
        &_header__title {
            font-size:30px;
        }
        &_header, &_footer {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        &_content {
            display: flex;
            justify-content: center;
            align-items: center;
            padding:20px 0px;
        }

        .submit_btn {
            padding: 8px;
            color: #2d2d2d;
            background: green;
            border: 1px solid #2d2d2d;
            border-radius:8px;
        }

        .close {
            cursor: pointer;
        }

        .close_btn {
            background: red;
            border: 1px solid #2d2d2d;
            padding: 8px;
            color: #2d2d2d;
            border-radius:8px;
        }
    }
</style>
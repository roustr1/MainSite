<template>
    <div class="popup_wrapper popup_wrapper__modal" ref="popup_wrapper">
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
                <button class="btn btn-defaultMainSite close_btn"
                        @click="closePopup">
                    {{leftBtnTitle}}
                </button>
                <button class="btn btn-defaultMainSite submit_btn"
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
        leftBtnTitle: {
            type: String,
            default: 'Закрыть'
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
        let vm = this
        document.addEventListener('click', function(item){
            if(item.target === vm.$refs['popup_wrapper']) {
                vm.closePopup()
            }
        })
        if (this.isMouseDown) {
            document.querySelector('.popup_wrapper').addEventListener('mousedown', function (e) {
                e.preventDefault()
            })
        }
    }
}
</script>

<style lang="scss">
    .popup_wrapper {
        position: fixed;
        overflow: auto;
        left: 0;
        top: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0,0,0,0.5);
        text-align: center;

        z-index: 1200;

        &__modal::after {
            display: inline-block;
            vertical-align: top;
            width: 0;
            height: 100%;
            content: '';
        }
    }

    .ms-popup {
        border-radius: 8px;
        z-index: 10;
        padding: 16px;
        display: inline-block;
        vertical-align: middle;
        width: 500px;
        padding: 10px;
        background: #fff;
        box-shadow: 0 0 17px 0 #e7e7e7;
        &_header__title {
            font-size:20px;
        }
        &_header, &_footer {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        &_content {
            text-align: left;
            display: flex;
            justify-content: center;
            align-items: center;
            padding:20px 0px;
        }

        .submit_btn {
            min-width: 100px;
        }

        .close {
            cursor: pointer;
        }

        .close_btn {
            min-width: 100px;
        }
    }
</style>
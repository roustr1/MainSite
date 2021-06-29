<template>
    <div class="ms-select">
        <slot>
            <p
                @click='reverseAreOptionsVisible'
                class="title"
                @mousedown="cursorEdit($event)"
                @selectstart="cursorEdit($event)"
                v-html="selected"
            />
            <div v-if="areOptionsVisible || isExpanded"
                class="options"
                >
                <div v-for="option in options"
                    :key="option.name"
                    @mousedown="cursorEdit($event)"
                    @selectstart="cursorEdit($event)"
                    @click="selectOption(option)"
                    v-html="option.name"
                />
            </div>
        </slot>
    </div>
</template>

<script>
export default {
    name: 'ms-select',
    props: {
        options : {
            type: Array,
            default: () => {
                return []
            }
        },
        selected: {
            type: String,
            default: () => {
                return ''
            }
        },
        isExpanded: {
            type: Boolean,
            default: () => {
                return false;
            }
        },
        isCursorEdit: {
            type: Boolean,
            default: () => {
                return false;
            }
        }
    },
    data: () => {
        return {
            areOptionsVisible: false,
        }
    },
    methods: {
        selectOption(option) {
            this.$emit('select', option);
            this.areOptionsVisible = false;
        },
        reverseAreOptionsVisible(){
            this.areOptionsVisible = !this.areOptionsVisible;
        },
        hideSelect(){
            this.areOptionsVisible = false;
        },
        cursorEdit(e) {
            if (this.isCursorEdit) e.preventDefault();
        }
    },
    mounted() {
        document.addEventListener('click', this.hideSelect.bind(this), true);
    },
    beforeDestroy() {
        document.removeEventListener('click', this.hideSelect);
    }
}
</script>

<style lang="scss">
    .ms-select {
        position: relative;
        margin: 16px 0;
        width: 300px;
        font-size: 18px;
        cursor: pointer;
        & .title {
            border: solid 1px #aeaeae;
            padding: 8px 8px;
            color: #000;
        }

        & p {
            margin: 0;
        }

        & .options {
            z-index: 20;
            background: #fff;
            border: solid 1px #aeaeae;
            position: absolute;
            top: 45px;
            width: 100%;
            & div {
                padding: 8px 8px;
                &:hover {
                    background: #e7e7e7;
                }
            }
        }
    }
</style>
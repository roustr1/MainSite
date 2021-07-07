<template>
    <div class="creator">
        <div v-if="showCreaterBlock" class="card-panel creator-main" v-bind:class="classObject">
            <a class="error close" @click="showCreater(false, true)"><i class="material-icons">close</i></a>
            <!--<a class="changeEditor" @click="changeEditor" style="cursor:pointer;"><i title="Поменять режим ввода" class="material-icons">swap_horiz</i></a>-->

            <msChangeNewsForm
                :isAdvancedEditor="isAdvancedEditor"
                :categoryId="categoryId"
                @changeNew="changeNew"
             />
        </div>
        <div v-bind:class="classObject" class="creator-menu">
            <button class="btn btn-defaultMainSite" @click="showCreater(true, false)">Новое сообщение</button>
        </div>
    </div>
</template>

<script>
    import { mapActions } from 'vuex';
    import msChangeNewsForm from './ms-change_news-form.vue';
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
                isAdvancedEditor: true
            }
        },
        computed: {
            showCreaterBlock() {
                if (!this.classObject['active'] && !this.classObject['desActive']) return false;

                if (this.classObject['active']) return true;

                return false;
            }
        },
        components: {
            msChangeNewsForm
        },
        methods: {
            ...mapActions('news',[
                'CREATE_NEW'
            ]),
            changeNew(result) {
                this.CREATE_NEW(result);
                this.showCreater(false, true);
            },
            /*changeEditor() {
                this.isAdvancedEditor = !this.isAdvancedEditor;
            },*/
            showCreater(active, desActive) {
                this.classObject['active'] = active;
                this.classObject['desActive'] = desActive;
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
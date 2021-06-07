<template>
    <header class="navbar-fixed">
        <nav class="nav-header">
            <div class="container">
                <div class="nav-wrapper">
                    <div class="valign-wrapper">
                        <a style="cursor: pointer;" @click="routerPushMainView" class="valign-wrapper">
                            <img :src="GetApplicationIcon" width="50" height="50" /> 
                            <span class="bold" style="padding-left:10px;">{{GetApplicationName}}</span>
                        </a>
                        <a id="openMenu" data-target="mobile-demo" class="sidenav-trigger" style="float: right;cursor:pointer;"><i class="material-icons">menu</i></a>
                        <ul class="hide-on-med-and-down col m12 s12 l9 secondMenu">
                            <li>
                                <div class="secondMenu-search">
                                    <span class="bold">Поиск на сайте:</span>
                                    <input v-model="searchText" style="flex-grow:1" class="inputTextMainSite" type="text" />
                                    <button style="flex-grow:0" class="btn btn-default" @click="searchNews">Найти</button>
                                </div>
                            </li>
                            <li style="display:flex; align-items:center;">
                                <div class="secondMenu-user">
                                    <a data-target='dropdown1' class="dropdown-trigger valign-wrapper" style="padding: 0px;">
                                        <span class="secondMenu-infoUser" style="padding-right:10px;">{{currentUser.Name}}</span>
                                        <img src="/images/layout_icons/userLogout.svg" alt="" />
                                        <!--<i class="material-icons">keyboard_arrow_down</i>-->
                                    </a>
                                    <ul id='dropdown1' class='dropdown-content secondMenu-settingsUser'>
                                        <li><a href="#!"><i class="material-icons">home</i>Личный кабинет</a></li>
                                        <li><a href="#!"><i class="material-icons">cloud</i>Управление сервисами</a></li>
                                    </ul>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="progress" id="progressLoad" style="background-color:transparent; margin:0px;" v-bind:style="isShow[isActive]">
                <div class="indeterminate" style="background-color:#64b5f6;"></div>
            </div>
        </nav>
    </header>
</template>

<script>
    import { mapState, mapActions, mapMutations } from 'vuex';

    export default {
        name: 'ms-header',
        data() {
            return {
                userName: 'Незарегистрированный пользователь',
                searchText: '',
                isShow: {
                    true: { display: 'block' },
                    false: {display : 'none'}
                }
            }
        },
        computed: {
            ...mapState('settings', ['settings']),
            ...mapState('user', ['currentUser']),
            ...mapState('preLoader', ['isActive']),
            GetApplicationName() {
                return this.searchSettingByName("Application.Name", "WebSite");
            },
            GetApplicationIcon() {
                return this.searchSettingByName("Application.Icon", "/images/layout_icons/header.png");
            }
        },
        methods: {
            ...mapActions('settings', ['GET_SETTINGS']),
            ...mapActions('user', ['GET_INFO_BY_CURRENT_USER']),
            ...mapMutations('menu', ['SET_OR_UPDATE_ACTIVE_CATEGORY']),
            searchNews() {
                if (Object.keys(this.$route.params).length == 0 || this.searchText != '') {
                    let routerParams = { name: 'search', params: { searchText: this.searchText } };

                    this.searchText = '';
                    this.$router.push(routerParams);
                }
            },
            searchSettingByName(name, defaultName) {

                let item = this.settings.find(function (item) {
                    if (item.Name == name && item.Value != '') {
                        return item;
                    }
                });

                return typeof item == 'undefined' || item == null ? defaultName : item.Value

            },
            routerPushMainView() {
                if (this.$route.name != 'main' ) {
                    this.SET_OR_UPDATE_ACTIVE_CATEGORY(null);
                    this.$router.push('/');
                }
            }
        },
        mounted() {
            this.GET_SETTINGS();
            this.GET_INFO_BY_CURRENT_USER();
        }
    }
</script>

<style lang="scss" scoped>
    .isActive {
        display:block;
    }
</style>

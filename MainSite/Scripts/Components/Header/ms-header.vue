<template>
    <header class="navbar-fixed">
        <nav class="nav-header">
            <div class="container">
                <div class="nav-wrapper">
                    <div class="valign-wrapper">
                        <a style="cursor: pointer;" asp-area="" asp-controller="Home" asp-action="Index" class="valign-wrapper">
                            <!--<img src="@service.SettingsDictionary.GetValueOrDefault(" Application.Icon","~ /images/layout_icons/header.png")" width="50" height="50" />-->
                            <!--<span class="bold">@service.SettingsDictionary.GetValueOrDefault("Application.Name", "WebSite")</span>-->
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
                            <li>
                                <div class="secondMenu-user">
                                    <a data-target='dropdown1' class="dropdown-trigger valign-wrapper" style="padding: 0px;">
                                        <span class="secondMenu-infoUser">{{userName}}</span>
                                        <i class="material-icons">keyboard_arrow_down</i>
                                        <img class="rectangle" src="/images/layout_icons/user.png" alt="" />
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
            <div class="progress" id="progressLoad" style="background-color:transparent; margin:0px; display:none;">
                <div class="indeterminate" style="background-color:#64b5f6;"></div>
            </div>
        </nav>
    </header>
</template>

<script>
    export default {
        name: 'ms-header',
        data() {
            return {
                userName: 'Незарегистрированный пользователь',
                searchText: ''
            }
        },
        methods: {
            searchNews() {
                let routerParams = { name: 'search', params: { searchText: this.searchText } };
                this.$router.push(routerParams);
            }
        },
        mounted() {
        }
    }
</script>

<style>
</style>

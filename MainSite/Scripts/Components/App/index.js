import Vue from 'vue';
import msMenu from '../Menu/ms-menu.vue';
import store from '../../Vuex/allStore';
import router from '../../Router/router';
import msMainWrapper from '../App/ms-main-wrapper.vue';

Vue.config.devtools = true;
new Vue({
    el:'#vueRootComponent',
    store,
    router,
    components: {
        /*msBirthday,
        msMenu,
        msNews,*/
        msMainWrapper,
        msMenu
    },
});
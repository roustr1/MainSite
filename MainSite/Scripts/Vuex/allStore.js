import Vue from 'vue'
import Vuex from 'vuex'
import menuStore from '../Vuex/MenuStore/store'
import newsStore from '../Vuex/NewsStore/store'

Vue.use(Vuex);

let store = new Vuex.Store({
    modules: {
        menu: menuStore,
        news: newsStore
    }
});

export default store;
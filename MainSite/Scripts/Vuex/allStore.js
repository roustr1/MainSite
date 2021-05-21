import Vue from 'vue'
import Vuex from 'vuex'
import menuStore from '../Vuex/MenuStore/store'
import newsStore from '../Vuex/NewsStore/store'
import settingsStore from '../Vuex/SettingsStore/store'
import userStore from '../Vuex/UserStore/store'
import planCalendarStore from '../Vuex/PlanCalendarStore/store'
import preLoaderStore from '../Vuex/PreLoaderStore/store'
Vue.use(Vuex);

let store = new Vuex.Store({
    modules: {
        menu: menuStore,
        news: newsStore,
        settings: settingsStore,
        user: userStore,
        planCalendar: planCalendarStore,
        preLoader: preLoaderStore
    }
});

export default store;
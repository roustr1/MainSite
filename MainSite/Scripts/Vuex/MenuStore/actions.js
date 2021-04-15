import axios from 'axios'

export default {
    async GET_CATEGORIES({ commit }) {
        try {
            let result = await axios('/api/ApiMenu/categories', {
                method: 'GET'
            });
            commit('SET_CATEGORIES', result.data);
        }
        catch (ex) { }
    },
    CHANGE_IS_ACTIVE_COMPONENT() {
        return false;
    },
    async GET_CATEGORIES_BY_BREADCRUMBS({ commit }, categoryId) {
        try {
            let result = await axios('/api/ApiMenu/breadcrumbs', {
                method: 'GET',
                params: {
                    categoryId: categoryId
                }
            });
            commit('SET_BREADCRUMBS', result.data);
        }
        catch (ex) {}
    }
}
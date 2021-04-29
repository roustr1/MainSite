import axios from 'axios'

export default {
    async GET_CATEGORIES({ commit }) {
        document.getElementById('progressLoad').style.display = 'none';
        try {
            let result = await axios('/api/ApiMenu/categories', {
                method: 'GET'
            });
            commit('SET_CATEGORIES', result.data);
        }
        catch (ex) { }
        document.getElementById('progressLoad').style.display = 'block';
    },
    CHANGE_IS_ACTIVE_COMPONENT() {
        return false;
    },
    async ADD_CATEGORY({ commit }, data) {
        document.getElementById('progressLoad').style.display = 'none';
        try {
            let result = await axios('/Home/CreateCategory', {
                method: 'POST',
                data: data
            });
            if (result.data.trim()) commit('ADD_CATEGORY', JSON.parse(result.data));
        }
        catch (ex) { }
        document.getElementById('progressLoad').style.display = 'block';
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
        catch (ex) { }
    }
}
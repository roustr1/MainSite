import axios from 'axios'

let menuStore = {
    namespaced: false,
    state: {
        categories: [], 
        breadcrumbs: []
    },
    actions: {
        GET_CATEGORIES({ commit }) {
            return axios('/api/ApiMenu/categories', {
                method: 'GET'
            })
            .then(responce => {
                commit('SET_CATEGORIES', responce.data);
                return responce.data;
            });
        },
        CHANGE_IS_ACTIVE_COMPONENT() {
            return false;
        },
        GET_CATEGORIES_BY_BREADCRUMBS({ commit }, categoryId) {
            /*set = new Set;       
            this.ARR_MAPPER(this.getters.CATEGORIES.slice(), set);
            let categoryes = Array.from(set).find();
            return Array.from(set);
            */

            return axios('/api/ApiMenu/breadcrumbs', {
                method: 'GET',
                params: {
                    categoryId: categoryId
                }
            })
            .then(responce => {
                commit('SET_BREADCRUMBS', responce.data);
            })
            .catch(function (error) {
            });

        }
    },
    mutations: {
        SET_CATEGORIES: (state, categories) => {
            state.categories = categories;
        },
        SET_BREADCRUMBS: (state, breadcrumbs) => {
            state.breadcrumbs = breadcrumbs;
        }
    },
    getters: {
        CATEGORIES(state) {
            return state.categories;
        },
        BREADCRUMBS(state) {
            return state.breadcrumbs;
        }
    },
};

export default menuStore;
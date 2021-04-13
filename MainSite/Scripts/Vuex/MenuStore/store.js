import axios from 'axios'

let menuStore = {
    namespaced: false,
    state: {
        categories: [], 
    },
    actions: {
        GET_CATEGORIES({ commit }) {
            return axios('/api/ApiMenu/categories', {
                method: 'GET'
            })
            .then(responce => {
                commit('SET_CATEGORIES', responce.data);
            });
        },
        CHANGE_IS_ACTIVE_COMPONENT() {
            return false;
        }
    },
    mutations: {
        SET_CATEGORIES: (state, categories) => {
            state.categories = categories;
        }
    },
    getters: {
        CATEGORIES(state) {
            return state.categories;
        }
    },
};

export default menuStore;
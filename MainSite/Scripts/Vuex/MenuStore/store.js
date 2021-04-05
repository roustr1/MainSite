import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex);

let store = {
    state: {
        categoryies: []
    },
    actions: {
        /*GET_USERS_FROM_API({ commit }) {
            return axios('http://localhost:3000/users', {
                method: 'GET'
            })
                .then(responce => {
                    commit('SET_USERS_TO_VUEX', responce.data);
                });
        }*/
    },
    mutations: {
       /* SET_USERS_TO_VUEX: (state, users) => {
            state.users = users;
        }*/
    },
    getters: {
        CATEGORYIES(state) {
            return state.categoryies;
        }
    },
};

export default store;
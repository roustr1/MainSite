import axios from 'axios'

let newsStore = {
    namespaced: false,
    state: {
        news: [],
        pager: {}
    },
    actions: {
        GET_NEWS({ commit }, data) {
            axios.get(`/api/ApiNews/newsItems/?`, {
                params: {
                    category: data.categoryId ? data.categoryId : null,
                    page: data.page
                }
            })
            .then(responce => {
                commit('SET_PAGER', responce.data.PagerModel);
                commit('SET_NEWS', responce.data.News/*{ data: responce.data, category: data.category }*/);
            })
            .catch(function (error) {
                alert(error);
            });
        },
        CREATE_NEW({ commit }, data) {
            axios(
                {
                    method: 'post',
                    url: data.action,
                    data: data.params,
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                }
            )
                .then(responce => {
                    commit('ADD_NEW', responce.data);
            })
            .catch(function (error) {
                alert(error);
            });
        },
        DOWNLOADFILE({ commit}, item) {
            return axios.get(`/GetFile/`, {
                params: {
                    fileId: item.Id
                }
            }).then(response => {
                var fileURL = window.URL.createObjectURL(new Blob([response.data]));
                var fileLink = document.createElement('a');

                fileLink.href = fileURL;
                fileLink.setAttribute('download', item.Name);

                document.body.appendChild(fileLink);

                fileLink.click();
            }).catch(console.error);
        }
    },
    mutations: {
        SET_NEWS: (state, news) => {
            state.news = news;
            //state.news = news.data;
        },
        SET_PAGER: (state, pager) => {
            Object.assign(state.pager,pager);
            //state.news = news.data;
        },
        ADD_NEW: (state, news) => {
            state.news.push(news);
        }
    },
    getters: {
        NEWS(state) {
            return state.news;
        },
        PAGER(state) {
            return state.pager;
        }
    },
};

export default newsStore;
import axios from 'axios'

let newsStore = {
    namespaced: false,
    state: {
        newsModel: {
            news: [],
            pager: {}
        }
    },
    actions: {
        GET_NEWS({ commit }, data) {
            return axios.get(`/api/ApiNews/newsItems/`, {
                method: 'GET',
                params: {
                    category: data.categoryId ? data.categoryId : null,
                    page: data.page ? data.page: 1
                }
            })
            .then(responce => {
                commit('SET_PAGER', responce.data.PagerModel);
                commit('SET_NEWS', responce.data.News);
                return responce.data;
            })
            .catch(function (error) {
                alert(error);
            });
        },
        CREATE_NEW({ commit }, data) {
            return axios(
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
                return responce.data;
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
            state.newsModel.news = news;
            //state.news = news.data;
        },
        SET_PAGER: (state, pager) => {
            Object.assign(state.newsModel.pager,pager);
            //state.news = news.data;
        },
        ADD_NEW: (state, news) => {
            state.newsModel.news.unshift(news);
        }
    },
    getters: {
        NEWS(state) {
            return state.newsModel.news;
        },
        PAGER(state) {
            return state.newsModel.pager;
        },
        NEWS_MODEL(state) {
            return state.newsModel;
        }
    },
};

export default newsStore;
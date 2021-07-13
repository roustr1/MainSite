import axios from 'axios'

export default {
    async GET_NEWS(store, data) {
        store.rootState.preLoader.isActive = true;
        try {
            let result = await axios('/api/ApiNews/newsItems/',
                {
                    method: 'post',
                    params: {
                        category: data.categoryId ? data.categoryId : null,
                        page: data.page ? data.page : 1
                    }
                }
            );
            store.commit('SET_PAGE', result.data.pagerModel);
            store.commit('SET_NEWS', result.data.news);
        }
        catch (ex) {

        }
        store.rootState.preLoader.isActive = false;
    },
    async GET_NEWS_BY_SEARCH(store, data) {
        if (data === '') return;
        store.rootState.preLoader.isActive = true;
        try {

            let result = await axios('/api/ApiNews/search/',
                {
                    method: 'post',
                    params: {
                        search: data
                    }
                }
            );
            store.commit('SET_PAGE', {});
            store.commit('SET_NEWS', result.data);
        }
        catch (ex) {

        }
        store.rootState.preLoader.isActive = false;
    },
    async CREATE_NEW({rootState, commit}, data) {
        rootState.preLoader.isActive = true;
        try {
            let result = await axios(
                {
                    method: 'post',
                    url: data.action,
                    data: data.params,
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                }
            );
            if (result.data) {
                commit('ADD_NEW', result.data);
                M.toast({html: 'Запись добавлена!'})
            }
        }
        catch (ex) { }
        rootState.preLoader.isActive = false;
    },
    async UPDATE_NEW({ commit }, result) {
        try {
            let resultApiEditModel = await axios(
                {
                    method: 'post',
                    url: '/Home/Edit/',
                    data: result.data.params,
                    headers: {
                        'Content-Type': 'multipart/form-data',
                    }
                }
            );

            if (resultApiEditModel.data != null) {
                commit('UPDATE_NEW', resultApiEditModel.data);
                M.toast({html: 'Запись обновлена!'})
                return true;
            }
        }
        catch (ex) {
        }
        M.toast({html: 'Запись не обновлена. Произошла ошибка!'})
        return false;
    },
    async DELETE_NEW({ commit }, data) {
        try {
            let result = await axios(
                {
                    method: 'post',
                    url: '/Home/Delete',
                    params: { id: data.id}
                }
            );
            if(result.data) {
                commit('REMOVE_NEW_FOR_LIST', data.index)
                M.toast({html: 'Запись удалена!'})
                return true
            }
        }
        catch (ex) { }

        M.toast({html: 'Запись не удалена. Произошла ошибка!'})
        return false
    },
    async GET_FILE({ commit }, newsId) {
        try {
            let result = await axios
                (
                    {
                        method: 'get',
                        url: '/GetFile/',
                        params: { fileId: newsId },
                        responseType: 'blob'
                    }
            );

            return result.data
        }
        catch (ex) {}
    }
}
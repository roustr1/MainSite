import axios from 'axios'

export default {
    async GET_NEWS({ commit }, data) {
       // document.getElementById('progressLoad').style.display = 'block';
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
            commit('SET_PAGE', result.data.PagerModel);
            commit('SET_NEWS', result.data.News);
        }
        catch (ex) {

        }
        //document.getElementById('progressLoad').style.display = 'none';
    },
    async GET_NEWS_BY_SEARCH({ commit }, data) {
       // document.getElementById('progressLoad').style.display = 'block';
        try {
            let result = await axios('/api/ApiNews/search/',
                {
                    method: 'post',
                    params: {
                        search: data
                    }
                }
            );
            commit('SET_PAGE', {});
            commit('SET_NEWS', result.data);
        }
        catch (ex) {

        }
       // document.getElementById('progressLoad').style.display = 'none';
    },
    async CREATE_NEW({ commit }, data) {
        //document.getElementById('progressLoad').style.display = 'block';
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

            if (JSON.parse(result.data) != null) {
                var res = JSON.parse(result.data);
                commit('ADD_NEW', res);
            }
        }
        catch (ex) { }
        //document.getElementById('progressLoad').style.display = 'none';
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

            if (JSON.parse(resultApiEditModel.data) != null) {
                commit('UPDATE_NEW', JSON.parse(resultApiEditModel.data), result.index);
                return true;
            }
        }
        catch (ex) {
        }

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
            commit('REMOVE_NEW_FOR_LIST', data.index);
        }
        catch (ex) { }
    },
    async DOWNLOADFILE({ commit }, item) {
        try {
            let result = await axios
                (
                    {
                        method: 'get',
                        url: '/GetFile/',
                        params: { fileId: item.Id },
                        responseType: 'blob'
                    }
            );

            let fileURL = window.URL.createObjectURL(new File([result.data], item.Name, { type: result.data.type }));
            let fileLink = document.createElement('a');

            fileLink.href = fileURL;
            fileLink.setAttribute('download', item.Name);

            document.body.appendChild(fileLink);

            fileLink.click();
        }
        catch (ex) {}
    }
}
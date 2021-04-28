import axios from 'axios'

export default {
    async GET_NEWS({ commit }, data) {
        try {
            let result = await axios.get(`/api/ApiNews/newsItems/`, {
                method: 'GET',
                params: {
                    category: data.categoryId ? data.categoryId : null,
                    page: data.page ? data.page : 1
                }
            });
            commit('SET_NEWS_MODEL', result.data);
        }
        catch (ex) {

        }
    },
    async CREATE_NEW({ commit }, data) {
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
            commit('ADD_NEW', result.data);
        }
        catch (ex) {}
    },
    DOWNLOADFILE({ commit }, item) {
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
}
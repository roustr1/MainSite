import axios from 'axios'

export default {
    async GET_NEWS({ commit }, data) {
        document.getElementById('progressLoad').style.display = 'block';
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
        document.getElementById('progressLoad').style.display = 'none';
    },
    async CREATE_NEW({ commit }, data) {
        document.getElementById('progressLoad').style.display = 'block';
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
            commit('ADD_NEW', JSON.parse(result.data));
        }
        catch (ex) { }
        document.getElementById('progressLoad').style.display = 'none';
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
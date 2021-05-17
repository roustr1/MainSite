import axios from 'axios'

export default {
    async GET_SETTINGS({ commit}) {
        try {
            let result = await axios('/Admin/GetSettings/',
                {
                    method: 'get'
                }
            );
            commit('SET_SETTINGS', result.data);
        }
        catch (ex) {

        }
    }
}
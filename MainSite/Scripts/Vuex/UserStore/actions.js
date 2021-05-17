import axios from 'axios';

export default {
    async GET_INFO_BY_CURRENT_USER({ commit}) {
        try {
            try {
                let result = await axios('/api/ApiUsers/InfoCurrentUser', {
                    method: 'GET'
                });
                commit('SET_USER', result.data);
            }
            catch (ex) {
            }
        }
        catch(ex) { }
    }
}
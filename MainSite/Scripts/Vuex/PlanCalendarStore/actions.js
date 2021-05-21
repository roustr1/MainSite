import axios from 'axios'

export default {
    async GET_PLAN_CALENDAR({ commit }) {
        try {
            let result = await axios('/api/ApiPlanCalendar/getPlanCalendar', {
                method: 'post',
            });
            commit("SET_PLAN_CALENDAR", result.data);
        }
        catch (ex) {}
    }
}
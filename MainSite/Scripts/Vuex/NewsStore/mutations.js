
export default {
    SET_NEWS(state, publications) {
        state.news = publications;
    },
    SET_PAGE(state, page) {
        state.pager = page;
    },
    ADD_NEW(state, news){
        state.news.unshift(news);
    },
    UPDATE_NEW(state, news, index) {
        state.news.splice(index, 1, news);
    },
    DELETE_CURRENT_NEWS(state) {
        state.news = [];
        state.pager = {};
    },
    REMOVE_NEW_FOR_LIST(state, index) {
        state.news.splice(index, 1);
    }
}
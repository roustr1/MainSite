
export default {
    SET_NEWS(state, publications) {
        state.news = publications;
    },
    SET_PAGE(state, page) {
        state.pager = page;
    },
    ADD_NEW(state, newsItem){
        state.news.unshift(newsItem);
    },
    UPDATE_NEW(state, newsItem, index) {
        state.news.splice(index, 1, newsItem);
    },
    DELETE_CURRENT_NEWS(state) {
        state.news = [];
        state.pager = {};
    },
    REMOVE_NEW_FOR_LIST(state, index) {
        state.news.splice(index, 1);
    }
}

export default {
    SET_NEWS: function (state, publications) {
        state.news = publications.map(function (item) {
            item.IsAdvanced = item.IsAdvancedEditor;
            return item;
        });
    },
    SET_PAGE: function (state, page) {
        state.pager = page;
    },
    ADD_NEW: (state, news) => {
        news.IsAdvanced = news.IsAdvancedEditor;
        state.news.unshift(news);
    },
    UPDATE_NEW: (state, news, index) => {
        news.IsAdvanced = news.IsAdvancedEditor;
        state.news.splice(index, 1, news);

    },
    DELETE_CURRENT_NEWS: (state) => {
        state.news = [];
        state.pager = {};
    },
    REMOVE_NEW_FOR_LIST: (state, index) => {
        state.news.splice(index, 1);
    }
}
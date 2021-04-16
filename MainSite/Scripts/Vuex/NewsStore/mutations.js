﻿export default {
    SET_NEWS_MODEL: (state, data) => {
        state.pager = data.PagerModel;
        state.news = data.News;
    },
    ADD_NEW: (state, news) => {
        state.news.unshift(news);
    },
    DELETE_CURRENT_NEWS: (state) => {
        state.news = [];
        state.pager = {};
    }
}
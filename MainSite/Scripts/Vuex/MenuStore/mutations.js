export default {
    SET_CATEGORIES: (state, categories) => {
        state.categories = categories;
    },
    SET_BREADCRUMBS: (state, breadcrumbs) => {
        state.breadcrumbs = breadcrumbs;
    },
    SET_OR_UPDATE_ACTIVE_CATEGORY: (state, categoryId) => {
        state.activeCategoryId = categoryId;
    }
}
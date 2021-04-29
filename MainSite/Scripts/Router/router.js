import Vue from 'vue'
import Router from 'vue-router'
import msNews from '../Components/News/ms-news.vue';
import msCategory from '../Components/Category/ms-category.vue';
import msCategoryList from '../Components/Category/ms-category-list.vue';
import msCategoryItemCreate from '../Components/Category/ms-category-item_create.vue';
Vue.use(Router);

let router = new Router({
    //mode:'history',
    routes: [
        {
            path: '/',
            name:'news',
            component: msNews,
            children: [
                {
                    path: '/page=:page',
                    name: 'news',
                    component: msNews,
                    props: true
                }
            ]
        },
        {
            path: '/category',
            name: 'category',
            component: msCategory,
            children: [
                {
                    path: '/categoryId=:categoryId/page=:page',
                    name: 'categoryDetails',
                    component: msNews,
                    props: true
                },
                {
                    path: 'category/create/parentCategoryId=:parentCategoryId',
                    name: 'createCategory',
                    component: msCategoryItemCreate,
                    props: true
                },
                {
                    path: '/category/categoryId=:categoryId',
                    name: 'categoryList',
                    component: msCategoryList,
                    props: true
                }
            ],
            props:true
        },
    ]
});

export default router;
import Vue from 'vue'
import Router from 'vue-router'
import msNews from '../Components/News/ms-news.vue';
import msCategory from '../Components/Category/ms-category.vue';
import msCategoryList from '../Components/Category/ms-category-list.vue';

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
            ],
            meta: {
                breadcrumb: 'Home Page',
            },
        },
        {
            path: '/category',
            name: 'category',
            component: msCategory,
            children: [
                {
                    path: '/page=:page/categoryId=:categoryId',
                    name: 'categoryDetails',
                    component: msNews,
                    props: true,
                    meta: {
                        breadcrumb: 'categoryDetails',
                    }
                },
                {
                    path: '/category/categoryId=:categoryId',
                    name: 'categoryList',
                    component: msCategoryList,
                    props: true,
                    meta: {
                        breadcrumb: 'categoryList',
                    }
                }
            ],
            props:true
        },
    ]
});

export default router;
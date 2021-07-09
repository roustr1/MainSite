<template>
    <div class="card-panel card_birthday">
        <div class="card_birthday-title bold">
            C днём рождения!
        </div>
        <div class="card_birthday-content">
            <div class="card_birthday-content_description"
                 v-for="item in users"
                 :key="item.Id">
                <span class="fio">{{item.FIO}}</span>
                <span class="subdivision">{{getSubDivision(item)}}</span>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios'

    export default {
        data() {
            return {
                users: []
            }
        },
        methods: {
            setUsers() {
                axios('/api/ApiUsers/GetBirthdayUsers', {
                    method: 'GET'
                })
                .then(responce => {
                    this.users = responce.data;
                });
            },
            getSubDivision(subDivision) {
                if (typeof subDivision == 'undefined' || subDivision == null) return 'ООНРиПНПК';

                return subDivision.DepartmentShortName !== '' ? subDivision.DepartmentShortName : subDivision.DepartmentFullName
            }
        },
        mounted() {
            this.setUsers();
        }
    };
</script>

<style lang="scss">
     /*Оформление блока день рождение(birthday)*/
    .card_birthday {
        position: absolute;
        right: 0;
        top: 0;
        margin: 0;
        transform: translateX(110%);
        width: 25%;

        @media (max-width: 1400px) {
            margin: 0.5rem 0 1rem 0;
            transform: translateX(0px);
            position: inherit;
            width: 100%;
        }

        &-title {
            text-align: center;
            color:#B12344;
            padding-bottom: 5px;
            @media (max-width: 1400px) {
                text-align: center;
                padding-bottom: 15px;
                width: 100%;
            }
        }

        &-content {
            font-size: 14px;
            display:flex;
            flex-direction:column;
            @media (max-width: 1400px) {
                flex-direction: row;
                flex-wrap: wrap;
                justify-content: inherit !important;
            }
            &_description {
                display:flex;
                flex-wrap:wrap;               
                @media (max-width: 1400px) {
                    padding:5px;
                }
                .fio {
                   padding-right: 10px;
                   font-weight: 600;
                   /*flex-basis:70%;*/
                    @media (max-width: 1400px) {
                        flex-basis:inherit;
                    }
                }
                .subdivision {
                   font-style: italic;
                   /*flex-basis:30%;*/
                   @media (max-width: 1400px) {
                        flex-basis:inherit;
                    }
                }
            }
        }
    }
</style>
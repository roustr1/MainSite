<template>
    <div class="card-panel card_birthday">
        <div class="card_birthday-title bold">
            C днём рождения!
        </div>
        <div class="card_birthday-content">
            <div class="card_birthday-content_description"
                 v-for="item in users"
                 :key="item.Id">
                <span class="fio">{{item.Fio}}</span>
                <span class="subdivision">{{getSubDivision(item.SubDivision)}}</span>
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
                        this.users =  responce.data;
                });
            },
            getSubDivision(subDivision) {
                if (typeof subDivision == 'undefined' || subDivision == null) return 'ООНРиПНПК';
                return subDivision;
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
        right: 5px;
        top: 64px;
        width: 13%;

        @media (max-width: 1400px) {
            position:inherit;
            right: none;
            top: none;
            padding-bottom: 15px;
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
                   flex-basis:50%;
                    @media (max-width: 1400px) {
                        flex-basis:inherit;
                    }
                }
                .subdivision {
                   font-style: italic;
                   flex-basis:50%;
                   @media (max-width: 1400px) {
                        flex-basis:inherit;
                    }
                }
            }
        }
    }
</style>
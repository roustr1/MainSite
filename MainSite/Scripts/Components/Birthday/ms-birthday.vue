<template>
    <div class="card-panel card_birthday">
        <div class="card_birthday-title bold">
            C днём рождения!
        </div>
        <div class="card_birthday-content">
            <div
                 v-for="item in users"
                 :key="item.Id"
                 >
                <img class="rectangle" :src="item.Photo" alt="" />
                <span>{{item.Fio}}</span>
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
            }
        },
        mounted() {
            this.setUsers();
        }
    };
</script>

<style lang="scss">
    .card_birthday-content {
        justify-content: space-between;
    }
</style>
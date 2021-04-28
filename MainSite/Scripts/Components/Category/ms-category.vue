<template>
    <div>
        <router-view />
    </div>
</template>

<script>
    export default {
        name: 'ms-category',
        props: {
            category: {
                type: Object,
                default: () => {
                    return {}
                }
            }
        },
        data() {
            return {}
        },
        mounted() {
        }
    }
</script>

<style>

</style>

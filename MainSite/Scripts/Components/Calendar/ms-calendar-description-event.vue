<template>
    <div class="ms-calendar-description-event">
        <div class="event-time">{{event.time}}</div>
        <div class="event-name">{{event.name}}</div>
        <div class="event-location">{{event.location}}</div>
    </div>
</template>

<script>
    export default {
        name: 'ms-calendar-description-event',
        props: {
            event: {
                type: Object,
                default: () => { return {} }
            }
        },
        data() {
            return {}
        },
        methods: {

        },
        created() {

        }
    }
</script>

<style lang="scss">
    .ms-calendar-description-event {
        color:black;
        font-size: 16px;
        display: flex;
        padding-bottom:5px;
        .event-time {
            padding-left:5px;
            padding-right:5px;
            font-style: italic;
            font-weight: 300;
            flex-basis:12%;
        }
        .event-name {
            padding-right:5px;
            flex-basis: 75%;
        }
        .event-location {
            flex-basis:15%;
            font-style: italic;
            font-weight: 300;
        }
    }
</style>
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
            padding-right:5px;
            font-style: italic;
            font-weight: 300;
            flex-basis:20%;
        }
        .event-name {
            padding-right:5px;
            flex-basis: 60%;
            display: flex;
            align-self: center;
        }
        .event-location {
            flex-basis:20%;
            font-style: italic;
            font-weight: 300;
        }
    }
</style>
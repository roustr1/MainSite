<template>
    <div class="ms-calendar_item" @click="eventClickDay">
        <b>{{dayNumber}}</b>
        {{getDayName}}
    </div>
</template>

<script>
    export default {
        name: 'ms-calendar-item',
        props: {
            dayNumber: {
                type: Number,
                default: () => { return null }
            },
            year: {
                type: Number,
                default: () => { return null }
            },
            month: {
                type: Number,
                default: () => { return null }
            }
        },
        data() {
            return {
                days: ['Вc', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб']
            }
        },
        computed: {
            getDayName() {
                if (this.year != null && this.month != null && this.dayNumber != null) {
                    let date = new Date(this.year, this.month, this.dayNumber);
                    return this.days[date.getDay()];
                }

                return '';
            }
        },
        methods: {
            eventClickDay() {
                this.$emit("eventClickDay", this.dayNumber);
            }
        }
    }
</script>

<style lang="scss">
    .ms-calendar_item {
        color: black;
    }
</style>
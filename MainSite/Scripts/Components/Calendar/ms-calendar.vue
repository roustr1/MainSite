<template>
    <div class="card-panel">
        <div class="ms-calendar">
            <a data-v-7fa05ddb="" href="#" class="error ms-calendar_back" @click.prevent="eventBackCalendar" v-if="IsDescriptionEvent"><i data-v-7fa05ddb="" class="material-icons">close</i></a>
            <h6 style="text-align:center;">ПЛАН-КАЛЕНДАРЬ: {{dayString}} {{planCalendar.year}} г.</h6>
            <!--<div class="ms-calendar_prev"><</div>-->
            <div class="ms-calendar_carusel">
                <div v-if="IsDescriptionEvent" class="ms-calendar_description">
                    <div class="text-center">{{messageEvent}}</div>
                    <p 
                        v-for="event in eventsCurrentDay"
                        :key="event.id"
                    >
                        {{event.name}}
                    </p>
                </div>
                <ms-calendar-item
                    v-for="n in daysInMonth"
                    :dayNumber="n"
                    :year="planCalendar.year"
                    :month="planCalendar.month"
                    :key="n"
                    @eventClickDay="eventClickDay"
                    v-else
                >
                </ms-calendar-item>
            </div>
            <!--<div class="ms-calendar_next">></div>-->
        </div>
    </div>
</template>

<script>
    import { mapState, mapActions } from 'vuex';
    import msCalendarItem from './ms-calendar-item.vue';
    export default {
        name: 'ms-calendar',
        data() {
            return {
                months: ["", "январь", "февраль", "март", "апрель", "май", "июнь", "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь"],
                eventsCurrentDay: [],
                IsDescriptionEvent: false,
                messageEvent: ''
            }
        },
        components: {
            msCalendarItem
        },
        computed: {
            ...mapState('planCalendar', [
                'planCalendar'
            ]),
            daysInMonth() {
                if (this.planCalendar.year) return 32 - new Date(this.planCalendar.year, 3, 32).getDate();

                return 0;
            },
            dayString() {
                return this.months[this.planCalendar.month];
            },
        },
        methods: {
            ...mapActions('planCalendar', [
                'GET_PLAN_CALENDAR'
            ]),
            eventClickDay(dayNumber) {
                this.eventsCurrentDay = this.getEventsFilterByDay(dayNumber);
                this.messageEvent = this.eventsCurrentDay.length ? `Мероприятия на ${dayNumber} число` : 'Информация по мероприятиям отсутствует';
                this.IsDescriptionEvent = true;
            },
            getEventsFilterByDay(day) {
                return this.planCalendar.events.filter(function (item) {
                    if (item.day == day) return item;
                });
            },
            eventBackCalendar() {
                this.eventsCurrentDay = [];
                this.IsDescriptionEvent = false;
            }
           /* setWidthByCarusel() {
                let parentBlockWidth = document.querySelector('.ms-calendar').offsetWidth;
                document.querySelector('.ms-calendar_carusel').style.width = (parentBlockWidth /1.1) + 'px';
            }*/
        },
        created() {
            this.GET_PLAN_CALENDAR();
            //this.setWidthByCarusel();
        }
    };
</script>

<style lang="scss">
    .ms-calendar {
        position: relative;
        margin: 0 auto;
        &_back {
          position:absolute;
          z-index:10;
          right:0;
          top:0;
        }
        &_carusel {
          position:relative;
          overflow:hidden;
          flex-direction: row;
          margin-left: 15px;
          margin-right: 15px;
          left: 0px; 
          transition-property: left; 
          transition-duration: 0.3s; 
          transition-timing-function: cubic-bezier(0, 0, 0.12, 0.89);
        }
        &_item {
            margin: 10px;
            width: 30px;
            text-align:center;
            float:left;
            cursor: pointer;
            &:hover {
                border-left:3px solid #64b5f6;
            }
        }
        &_prev {
            z-index: 3;
            background-color: white;
            border-radius:30px;
            box-shadow: 0 0 10px rgba(0,0,0,0.5);
            padding: 1px 10px;
            cursor:pointer;
        }
        &_next {
            z-index: 3;
            box-shadow: 0 0 10px rgba(0,0,0,0.5);
            padding: 1px 10px;
            background-color: white;
            border-radius:30px;
            cursor:pointer;
        }
    }
</style>
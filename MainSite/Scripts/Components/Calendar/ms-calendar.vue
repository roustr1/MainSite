<template>
    <div class="card-panel">
        <div class="ms-calendar">
            <h6 style="text-align:center;color: #1E57A5;">Основные мероприятия училища, предусмотренные планом-календарем на {{RefactDate}}</h6>
            <div class="ms-calendar_carusel">
                <div class="ms-calendar_description">
                    <template v-if="EventsFilterByDay.length">
                        <input type="checkbox" class="read-more-checker" id="read-more-checker" />
                        <div ref="limiter" class="limiter">
                            <ms-calendar-description-event
                              v-for="event in EventsFilterByDay"
                              :event="event"
                              :key="event.id"
                            />
                        </div>
                        <label v-if="IsOverflowed" for="read-more-checker" class="read-more-button"></label>
                    </template>
                    <template v-else>
                        <div class="text-center" style="color: #B12344;">Мероприятий не запланировано</div>
                    </template>                  
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { mapState, mapActions } from 'vuex';
    import msCalendarItem from './ms-calendar-item.vue';
    import msCalendarDescriptionEvent from './ms-calendar-description-event.vue';
    export default {
        name: 'ms-calendar',
        data() {
            return {
                months: ["", "январь", "февраль", "март", "апрель", "май", "июнь", "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь"],
                blockLimiter: {}
            }
        },
        components: {
            msCalendarItem,
            msCalendarDescriptionEvent
        },
        computed: {
            ...mapState('planCalendar', [
                'planCalendar'
            ]),
            EventsFilterByDay() {
                if (this.planCalendar && this.planCalendar.events) {
                    return this.getEventsFilterByDay(new Date().getDate());
                }

                return []
            },
            IsOverflowed() {
                this.$nextTick(() => {
                    this.blockLimiter = this.$refs.limiter                  
                });

                if (this.blockLimiter == null || this.blockLimiter == 'undefined') return false;

                return this.blockLimiter.scrollWidth > this.blockLimiter.offsetWidth || this.blockLimiter.scrollHeight > this.blockLimiter.offsetHeight;
            },
            RefactDate(){
                let options = {
                    day: 'numeric',
                    month: 'long',
                    year: 'numeric'
                }

                return new Date().toLocaleDateString("ru", options);
            }
        },
        methods: {
            ...mapActions('planCalendar', [
                'GET_PLAN_CALENDAR'
            ]),
            getEventsFilterByDay(day) {
                return this.planCalendar.events.filter(function (item) {
                    if (Number(item.day) == Number(day) && item.time.trim() !=='' && item.location.trim() !== '') return item;
                }).sort(function(a, b) {
                    let regExp = new RegExp(/\d{1,}.\d{1,}/g);
                    let resA = a.time.match(regExp)
                    let resB = b.time.match(regExp)
                    let result = 0;

                    if(resA != null && resB != null) {
                        for(let i = 0; i< resA.length; i++) {
                            if(result == 0 && typeof resB[i] != 'undefined') {
                                let timeFirstELement = Number(resA[i])
                                let timeTwoElement = Number(resB[i])

                                result = timeFirstELement - timeTwoElement
                            }
                        } 
                        return result
                    }
                    else if(resA == null) result = 100
                    else if(resB == null) result = -100

                    return result
                });
            }
        },
        created() {
            if (Object.keys(this.planCalendar).length == 0) this.GET_PLAN_CALENDAR();
        }
    };
</script>

<style lang="scss">
    .ms-calendar {
        font-size: 14px;
        position: relative;
        margin: 0 auto;
        &_back {
          position:absolute;
          z-index:10;
          right:0;
          top:0;
        }
        &_description {
            /* Стили для текстового поля с кнопкой "Далее" */
            .limiter {
                max-height: 100px;
                overflow: hidden;
                position: relative;
                & .bottom {
                    position: absolute;
                    bottom: 0;
                    /*background: linear-gradient(to bottom, #fff, #f7f7f7);*/
                    width: 100%;
                    height: 60px;
                    opacity: 1;
                    transition: 0.3s;
               }
            }
            .read-more-checker {
                opacity: 0;
                position: absolute;

                &:checked {
                    & ~ .limiter {
                        max-height: none;
                        & .bottom {
                            opacity: 0;
                            transition: 0.3s;
                        }
                    }
                    
                    & ~ .read-more-button {
                        &:before {
                            content: "Свернуть \2B9D";
                        }
                    }
                }

                & ~ .read-more-button {
                    &:before {
                        content: "Развернуть \2B9F";
                    }
                }
            }
            .read-more-button {
                cursor: pointer;
                text-align:center;
                display: block;
                color: rgb(30, 87, 165);
                font-size: 14px;
            }
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
    }
</style>
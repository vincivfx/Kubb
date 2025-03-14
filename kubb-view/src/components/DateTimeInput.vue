<script>
import InputBlock from "@/components/InputBlock.vue";
import Modal from "./Modal.vue";

export default {
  name: "DateTimeInput",
  props: ['modelValue', 'label'],
  components: { InputBlock, Modal },
  data: () => ({
    showPicker: false,
    days: [],
    monthLabel: '',
    year: 2024,
    month: 0,
    date: {},
    hours: '0',
    minutes: '0',
    dateLabel: ''
  }),
  methods: {
    today() {
      const todayDate = new Date();
      this.date = {
        year: todayDate.getFullYear(),
        month: todayDate.getMonth(),
        day: todayDate.getDate()
      }
      this.year = todayDate.getFullYear();
      this.month = todayDate.getMonth();
    },
    save() {
      const finalDate = new Date(this.date.year, this.date.month, this.date.day, this.hours, this.minutes);
      this.$emit('update:modelValue', finalDate);
      this.dateLabel = finalDate.toLocaleString();
      this.$refs.dateTimeModal.hide();
    },
    nextMonth(index = 1) {
      const prevMonth = this.month;
      this.month = (this.month + index + 12) % 12;
      if (prevMonth === 11 && this.month === 0) this.year += 1;
      if (prevMonth === 0 && this.month === 11) this.year -= 1;
      this.getMonthCalendar();
    },
    /**
     * generate a calendar by using month and year.
     * found on https://web.cs.dal.ca/~jamie/CS3172/Course/assig/zeller.html#zeller
     * weekday = ((13 × month − 1) ÷ 5 + year ÷ 4 + century ÷ 4 + day + year − 2 × century) % 7
     */
    getMonthCalendar() {
      const year = this.year;
      const month = this.month + 1;
      this.days = [];
      let monthDays = 31;
      if (month === 2) {
        monthDays = 28 + (year % 4 === 0 && year % 100 !== 0 || year % 400 !== 0 ? 1 : 0);
      }
      if ([4, 6, 9, 11].indexOf(month) !== -1) {
        monthDays = 30;
      }
      const century = Math.ceil(year / 100);
      const firstDay = (Math.ceil((13 * month - 1) / 5) + Math.ceil(year / 4) + Math.ceil(century / 4) + year - 2 * century) % 7;
      for (let i = 1; i <= firstDay; i += 1) this.days.push(0);
      for (let i = 1; i <= monthDays; i += 1) this.days.push(i);

      const months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
      this.monthLabel = months[month - 1];

    },
    showPickerFunction() {
      setTimeout(() => this.showPicker = true, 1000)
    },
    selectDate(day) {
      this.date = {
        day,
        month: this.month,
        year: this.year
      }
    }
  },
  mounted() {
    const today = new Date();
    this.month = today.getMonth();
    this.year = today.getFullYear();
    this.getMonthCalendar();

    if (this.modelValue !== '') {
      const date = new Date(this.modelValue);
      if (date === null) return;
      this.dateLabel = date.toLocaleString();
      this.date = {
        year: date.getFullYear(),
        month: date.getMonth(),
        day: date.getDate()
      };
      this.month = date.getMonth();
      this.year = date.getFullYear();
      this.hours = date.getHours();
      this.minutes = date.getMinutes();
      
    }
  }
}
</script>

<template>
  <div :click-outside="wow">
    <InputBlock v-model="dateLabel" :label="label" readonly="" @focusin="$refs.dateTimeModal.show()" placeholder="dd/mm/YYYY HH:MM">
      <slot />
    </InputBlock>
  </div>

  <Modal title="Pick a Date" ref="dateTimeModal">
      <div class="rows">
        <div class="col">
          <div class="datetime-calendar">
            <div class="datetime-year">
              {{ year }}
            </div>
            <div class="datetime-month">
              <button type="button" class="btn small" @click="nextMonth(-1)">&lt;</button>
              <span style="width: 120px; display: inline-block;">
                {{ monthLabel }}
              </span>
              <button type="button" class="btn small" @click="nextMonth()">&gt;</button>
            </div>

            <div class="datetime-days">
              <span v-for="(day, dayKey) in ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su']" class="day-of-week-span" :key="dayKey">{{ day }}</span>
              <button @click="selectDate(day)" type="button" :class="['btn small', {'secondary': day === date.day && month === date.month}]" v-for="(day, key) in days" :key="key" :disabled="day === 0">
                <span v-if="day !== 0">{{ day }}</span>
                <span v-else>&nbsp;</span>
              </button>
            </div>

          </div>

          <button @click="today()" class="btn" type="button">Set date to today</button>
        </div>
        <div class="col">
          <div class="datetime-time">
            <span class="datetime-time-label">
              Hour
            </span>
            <button type="button" @click="hours = (hours + 1) % 24" class="btn small">
              +
            </button>
            <InputBlock pattern="^2([0-3])|1([0-9])|([0-9])$" require v-model="hours" class="datetime-time-value" />
            <button type="button" @click="hours = (23 + hours) % 24" class="btn small">
              -
            </button>
          </div>

          <div class="datetime-time">
            <span class="datetime-time-label">
              Minutes
            </span>
            <button type="button" @click="minutes = (minutes + 1) % 60" class="btn small">
              +
            </button>
            <InputBlock pattern="^([0-5]?)([0-9])$" required v-model="minutes" class="datetime-time-value" />
            <button type="button" @click="minutes = (59 + minutes) % 60" class="btn small">
              -
            </button>
          </div>
        </div>
      </div>

      <p class="p-2">
        Selected date: {{ new Date(date.year, date.month, date.day, hours, minutes).toLocaleString() }}
      </p>

      <div class="m-2">
        <button class="btn primary" @click="save()">Save</button>
      </div>

  </Modal>
</template>
<script>
import InputBlock from "@/components/InputBlock.vue";

export default {
  name: "DateTimeInput",
  props: ['modelValue'],
  components: { InputBlock },
  data: () => ({
    showPicker: false,
    days: [],
    monthLabel: '',
    year: 2024,
    month: 0
  }),
  methods: {
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
    }
  },
  mounted() {
    const today = new Date();
    this.month = today.getMonth();
    this.year = today.getFullYear();
    this.getMonthCalendar();
  }
}
</script>

<template>
  <div :click-outside="wow">
    <InputBlock readonly="" @focusin="showPicker = true;" placeholder="dd/mm/YYYY HH:MM">
      <slot />
    </InputBlock>
    <div class="datetime-block">
      <div v-if="showPicker" class="datetime-calendar">

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
          <button type="button" class="btn small" v-for="(day, key) in days" :key="key" :disabled="day === 0">
            <span v-if="day !== 0">{{ day }}</span>
            <span v-else>&nbsp;</span>
          </button>
        </div>

      </div>
    </div>
  </div>
</template>
<script>
import InputBlock from "@/components/InputBlock.vue";

export default {
  name: "DateTimeInput",
  components: {InputBlock},
  data: () => ({
    showPicker: false,
    days: [],
    
  }),
  methods: {
    /**
     * generate a calendar by using month and year.
     * found on https://web.cs.dal.ca/~jamie/CS3172/Course/assig/zeller.html#zeller
     * weekday = ((13 × month − 1) ÷ 5 + year ÷ 4 + century ÷ 4 + day + year − 2 × century) % 7
     * @param year
     * @param month
     */
    getMonthCalendar(year, month) {
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
      
    }
  },
  mounted() {
    const today = new Date();
    this.getMonthCalendar(today.getFullYear(), today.getMonth() + 2);
  }
}
</script>

<template>
  <InputBlock @focusin="showPicker=true" v-model="modelValue" placeholder="dd/mm/YYYY HH:MM" ><slot /></InputBlock>
  <div class="datetime-block">
    <div v-if="showPicker" class="datetime-calendar">

      <div class="datetime-month">
        <button class="btn small">&lt;</button>
        feb
        <button class="btn small">&gt;</button>
      </div>

      <div class="datetime-days">
        <button class="btn small" v-for="(day, key) in days" :key="key" :disabled="day === 0">
          <span v-if="day !== 0">{{day}}</span>
          <span v-else>&nbsp;</span>
        </button>
      </div>

    </div>
  </div>
</template>

<style scoped>

</style>
<script>
import CheckBox from "@/components/CheckBox.vue";
import DateTimeInput from "@/components/DateTimeInput.vue";
import InputBlock from "@/components/InputBlock.vue";
import { SlTrash } from "vue-icons-plus/sl";

export default {
  name: "TestView",
  components: {CheckBox, DateTimeInput, InputBlock, SlTrash}
}
</script>

<template>
  <CheckBox>Ciao</CheckBox>
  <DateTimeInput />
  <div class="input-line">
    <InputBlock label="Question">
      <button @click="" class="btn danger small">
          <SlTrash />
      </button>
    </InputBlock>
    <div class="input-line-group">
      
    </div>
  </div>
</template>

<style scoped>

</style>
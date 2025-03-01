<script lang="ts">
import InputBlock from '@/components/InputBlock.vue';
import Select from '@/components/Select.vue';
import Tabs from "@/components/Tabs.vue";

export default {
  components: {Tabs, Select, InputBlock},
  data: () => ({
    page: 'send-answer'
  })
}

</script>

<template>

  <h2>Challenge name</h2>
  
    <Tabs v-model="page" :tabs="[{text: 'Send answer', id: 'send-answer'}, {text: 'Set Jolly', id: 'set-jolly'}, {text: 'Answer List', id: 'answer-list'}]">
      <div v-if="page === 'send-answer'">
        <h3>Send answer</h3>
        <Select :options="[{text: 'Hello', key: 'h'}, {text: 'Ciao', key: 'c'}]">
          Select team
        </Select>

        <Select :options="[{text: '#1 Problema', key: 0}]">
          Question
        </Select>

        <InputBlock>
          Answer to question
        </InputBlock>

        <button class="btn primary">Send</button>
      </div>
    </Tabs>

</template>
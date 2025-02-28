<script>
import Modal from '@/components/Modal.vue';
import Tabs from '@/components/Tabs.vue';
import { SlPlus } from 'vue-icons-plus/sl';

export default {
  data: () => ({
    challenges: [
      {name: 'Gara di Aprile', organizer: 'Rossi'}
    ],
    page: ''
  }),
  components: {Tabs, SlPlus},
  mounted() {
    let validTypes = [undefined, '', 'archive', 'joinable', 'my'];

    if (validTypes.indexOf(this.$route.params.type) === -1) {
      this.$router.push({name: 'error'});
    }

    this.page = this.$route.params.type === undefined ? '' : this.$route.params.type;

    switch(this.$route.params.type) {
      case(undefined):
        this.$http.get('/Home/Challenges').then(response => {
          console.log(response);
        })
        break;
    }
  }
}
</script>

<template>
  <Tabs v-model="page" :tabs="[{text: 'Running Challenges', id: ''}, {text: 'Archived Challenges', id: 'archived'}, {text: 'My Challenges', id: 'my', runIf: () => $authSession.getStored()}]">
    <div v-if="page === ''">
      <h2>Running Challenges</h2>
    </div>
    <div v-if="page === 'my'">
      <h2>My Challenges<button class="btn small"><SlPlus /> </button></h2>
    </div>
    <table class="table">
      <thead>
        <tr>
          <th>Challenge name</th>
          <th>Organizer</th>
          <th>Time</th>
          <th>Follow</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, key) in challenges" :key="key">
          <td>{{ item.name }}</td>
          <td>{{ item.organizer }}</td>
          <td>{{ item.date }}</td>
          <td class="button-cell">
            <button class="btn primary">Follow</button>
          </td>
        </tr>
      </tbody>
    </table>
  </Tabs>
</template>

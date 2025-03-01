<script lang="ts">
import Modal from '@/components/Modal.vue';
import {SlPencil, SlPlus, SlTrash} from 'vue-icons-plus/sl';
import Tabs from "@/components/Tabs.vue";
import InputBlock from "@/components/InputBlock.vue";

export default {
  components: {InputBlock, Tabs, SlPencil, SlPlus, SlTrash, Modal},
  data: () => ({
    teams: [{name: 'Scuola 1'}],
    page: 'overview',
  }),
  methods: {
  }
}
</script>

<template>

  <h2>Challenge name
    <button @click="$refs.editChallengeModal.show()" class="btn btn-primary small">
      <SlPencil/>
    </button>
  </h2>
  
  <Tabs v-model="page" :tabs="[{text: 'Challenge Overview', id: 'overview'}, {text: 'Teams', id: 'teams'}, {text: 'Challenge Options', id: 'options'}]">
    <div v-if="page === 'overview'">
      <p>
        Starting at: DATE, Ending at: DATE (duration: MINUTES)
      </p>
    </div>
    <div v-if="page === 'teams'">

      <h3>Teams
        <button @click="$refs.addTeamModal.show()" class="btn primary small">
          <SlPlus/>
        </button>
      </h3>
      <table class="table">
        <thead>
        <tr>
          <th>Team name</th>
          <th>User</th>
          <th></th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="(item, key) in teams" :key="key">
          <td>{{ item.name }}</td>
          <td><b>you</b></td>
          <td>
            <button class="btn small danger">
              <SlTrash/>
            </button>
          </td>

        </tr>
        </tbody>
      </table>
    </div>
    <div v-if="page === 'options'">
      
      <h4>Delete this challenge</h4>
      <p>
        If you request the challenge deletion, you will be not able to recover it.
      </p>
      <button class="btn danger" @click="$refs.deleteChallengeModal.show()">Delete Challenge</button>
    </div>
  </Tabs>
  
  <Modal title="Delete this challenge" ref="deleteChallengeModal">
    Are you sure to delete <b>challenge name</b> challenge?
    <InputBlock>
      Type the challenge name
    </InputBlock>
    <input type="submit" class="btn danger" value="Delete this challenge">
  </Modal>
  


  <Modal title="Add a new team" ref="addTeamModal">

  </Modal>

  <Modal title="Edit challenge settings" ref="editChallengeModal">
    
  </Modal>
</template>
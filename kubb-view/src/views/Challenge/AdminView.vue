<script lang="ts">
import Modal from '@/components/Modal.vue';
import { SlPencil, SlPlus, SlTrash } from 'vue-icons-plus/sl';
export default {
    components: {SlPencil, SlPlus, SlTrash, Modal},
    data: () => ({
        teams: [{name: 'Scuola 1'}]
    })
}
</script>

<template>

    <h2>Challenge name <button class="btn btn-primary small"><SlPencil /> </button></h2>
    <p>
        Starting at: DATE, Ending at: DATE (duration: MINUTES)
    </p>

    <h3>Teams <button @click="$refs.addTeamModal.show()" class="btn primary small"><SlPlus /></button></h3>
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
            <button class="btn small danger"><SlTrash /> </button>
          </td>
          
        </tr>
      </tbody>
    </table>

    <Modal title="Add a new team" ref="addTeamModal">

    </Modal>
</template>
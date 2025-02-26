<script>
export default {
  data: () => ({
    challenges: [
      {name: 'Gara di Aprile', organizer: 'Rossi'}
    ]
  })
}
</script>

<template>
  <div class="p-5 bg-primary text-white">
    <h2>{{ $route.query.archive !== undefined ? 'Archived Challenges' : 'Running Challenges' }}</h2>
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
  </div>
</template>

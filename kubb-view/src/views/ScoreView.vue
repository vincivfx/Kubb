<script>
import CheckBox from '@/components/CheckBox.vue';
import Modal from '@/components/Modal.vue';
import Scoreboard from '@/util/score';
import {SlSettings} from 'vue-icons-plus/sl';

export default {
  data: () => ({
    scoreboard: null,
    scrollTimeout: null,
    scroll: false,
    name: '',
    hiddenBar: false,
    fetcherInterval: null,
    scrollerInterval: null,
  }),
  components: {SlSettings, Modal, CheckBox},
  methods: {
    loadScoreboard() {
      if (this.$route.query.id === 'test') {
        this.scoreboard = Scoreboard.parseScoreboard(Scoreboard.testScoreboard());
        return;
      }

      this.$http.get('/Home/GetCache?key=' + encodeURIComponent(this.$route.query.id)).then(response => {
        this.scoreboard = Scoreboard.parseScoreboard(response.data);
      }).catch(error => {
        if (error.response.status === 404) {
          this.$router.push({
            name: 'challenges'
          })
        }
      })
    }
  },
  // clear intervals when you leave the page
  unmounted() {
    clearInterval(this.fetcherInterval);
    clearInterval(this.scrollTimeout);
  },
  mounted() {
    this.loadScoreboard();
    this.fetcherInterval = setInterval(() => this.loadScoreboard(), 15000);
    this.scrollerInterval = setInterval(() => {
      if (this.scroll) {
        if (window.innerHeight + window.scrollY >= document.documentElement.scrollHeight) {
          if (!this.scrollTimeout)
            this.scrollTimeout = setTimeout(() => window.scrollTo(0, 0), 2000);
        } else {
          window.scrollBy(0, 1);
          this.scrollTimeout = null;
        }
      }
    }, 30);
    if (this.$route.query.id === 'test') {
      this.name = 'developer mode';
      return;
    }
  }
}
</script>

<template>
  <Modal title="Settings" ref="settingsModal">
    <CheckBox>
      Enable automatic scroll
    </CheckBox>
    <input type="checkbox" v-model="scroll"> enable automatic scroll<br>
    <input type="checkbox" v-model="hiddenBar" @change="$emit('disableHeaderScoreView', $event.target.checked)"> disable header bar
  </Modal>

  <h1>{{ name }}
    <button @click="$refs.settingsModal.show();" class="btn small">
      <SlSettings/> 
    </button>
  </h1>
  <div class="scoreboard-container">
    <div class="scoreboard-table" v-if="scoreboard">
      <div class="scoreboard-header">
        <div></div>
        <div></div>
        <div></div>
        <div class="question" v-for="(question, key) in scoreboard.questions" :key="key">
          <span class="number">{{ key + 1 }}</span>
          <span class="points">{{ question.points }}</span>
          <span class="answers">{{ question.answers }}</span>
        </div>
      </div>
      <div class="team" v-for="(team, key) in scoreboard.teams" :key="key">

        <div class="team-position">
          {{ key + 1 }}
        </div>
        <div class="team-name">
          {{ team.name }}
        </div>
        <div class="team-points">
          {{ team.points }}
        </div>
        <div class="team-question" v-for="(question, key_question) in team.questions"
            :class="[{ 'positive': question.arrow === 'up' }, { 'negative': question.arrow === 'down' }, { 'animate': question.highlight }]"
            :key="key_question">
                  <span v-if="question != null">
                      {{ question.points }}
                      <span v-if="question.jolly" class="jolly">J</span>
                  </span>
        </div>
      </div>
    </div>
  </div>
</template>

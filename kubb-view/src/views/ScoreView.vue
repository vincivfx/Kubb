<script>
import Badge from '@/components/Badge.vue';
import CheckBox from '@/components/CheckBox.vue';
import Modal from '@/components/Modal.vue';
import Select from '@/components/Select.vue';
import Scoreboard from '@/util/score';
import {SlMinus, SlPlus, SlSettings} from 'vue-icons-plus/sl';

export default {
  data: () => ({
    challenge: {},
    scoreboard: null,
    scrollTimeout: null,
    scroll: false,
    name: '',
    hiddenBar: false,
    fetcherInterval: null,
    scrollerInterval: null,
    timer: setInterval(() => {}, 1000000),
    timer_text: '',
    zoomRatio: 1,
    updateTimer: 15,
    highlightTeams: [],
    showTop: 'all'
  }),
  components: {SlSettings, Modal, CheckBox, SlPlus, SlMinus, Select, Badge},
  methods: {
    loadScoreboard() {
      this.updateTimer = 15;
      if (this.$route.query.id === 'test') {
        this.scoreboard = Scoreboard.parseScoreboard(Scoreboard.testScoreboard());
        return;
      }

      if (new Date().getTime() < new Date(this.challenge.startTime).getTime()) return;

      this.$http.get('/Home/GetCache?key=' + encodeURIComponent(this.$route.query.id)).then(response => {
        this.scoreboard = Scoreboard.parseScoreboard(response.data, this.challenge);
      });
    },
    zoom(direction) {
      if (direction > 0) this.zoomRatio += 0.1;
      else this.zoomRatio -= 0.1;
    }
  },
  // clear intervals when you leave the page
  unmounted() {
    clearInterval(this.fetcherInterval);
    clearInterval(this.scrollTimeout);
    clearInterval(this.scrollerInterval);
    clearInterval(this.timer);
  },
  mounted() {
    this.fetcherInterval = setInterval(() => this.loadScoreboard(), 15000);
    this.scrollerInterval = setInterval(() => {
      if (this.scroll) {
        if (window.innerHeight + window.scrollY >= document.documentElement.scrollHeight) {
          if (!this.scrollTimeout)
            this.scrollTimeout = setTimeout(() => window.scrollTo(0, 0), 2000);
        } else {
          window.scrollBy(0, 1);
          clearTimeout(this.scrollTimeout);
          // this.scrollTimeout = null;
        }
      }
    }, 30);
    if (this.$route.query.id === 'test') {
      this.name = 'developer mode';
      return;
    }
    
    this.$http.get("Home/ChallengeSetup?challengeId=" + encodeURIComponent(this.$route.query.id)).then(response => {
      this.challenge = response.data;
      this.challenge.algorithmSettings = JSON.parse(this.challenge.algorithmSettings);
      this.loadScoreboard();
    });
    
    this.timer = setInterval(() => {
      const diff = Math.floor((new Date(this.challenge.endTime).getTime() - new Date().getTime())/1000);
      if (diff > 0)
        this.timer_text = Math.floor(diff / 3600) + "h " + Math.floor(diff / 60) % 60 + "m " + diff % 60 + "s"
      else 
        this.timer_text = 'finished'

      this.updateTimer -= 1;
    }, 1000);

  }
}
</script>

<template>
  <Modal title="Settings" ref="settingsModal">
    <h2>Display Settings</h2>
    <CheckBox v-model="scroll">
      enable automatic scroll
    </CheckBox>
    <CheckBox @change="(e) => $emit('disableHeaderScoreView', e)">
      disable header bar
    </CheckBox>

    <p>
      zoom settings:
    </p>
    <button @click="zoom(1)" class="btn small">
      <SlPlus />
    </button>
    &nbsp;
    <button @click="zoom(-1)" class="btn small">
      <SlMinus />
    </button>

    <hr />

    <Select readonly="" v-model="showTop" :options="[{key: 'all', text: 'Show all'}, {key: 10, text: '10'}, {key: 20, text: '20'}, {key: 30, text: '30'}, {text: 'Only Highlighted', key: 0}]" label="Top teams to show" />

    <h2>Filter by Team Name</h2>

    <Select @change="(e) => highlightTeams.indexOf(e) === -1 && scoreboard.teams.map(item => item.name).indexOf(e) >= 0 ? highlightTeams.push(e) : {}" :options="scoreboard.teams.map(item => ({text: item.name, key: item.name}))" label="Search by Team Name" />
    <p>Highlight: (click to remove)</p>
      <Badge @click="highlightTeams.splice(tid, 1)" class="m-1" type="warning" v-for="(team, tid) in highlightTeams" :key="tid">
      {{ team }}
      
    </Badge>
    <div style="height: 400px;"></div>
  </Modal>

  <h1>{{ challenge.name }} {{ name }}
    <button @click="$refs.settingsModal.show();" class="btn small">
      <SlSettings/>
    </button>
  </h1>
  <div :style="{'zoom': zoomRatio}" class="scoreboard-container" v-if="new Date().getTime() > new Date(this.challenge.startTime).getTime()">
    <div class="scoreboard-table" v-if="scoreboard">
      <div class="scoreboard-header">
        <div style="font-size: 24px; vertical-align: middle;">
          {{ updateTimer }}
        </div>
        <div style="vertical-align: middle;">
          {{ timer_text }}
        </div>
        <div></div>
        <div class="question" v-for="(question, key) in scoreboard.questions" :key="key">
          <span class="number">{{ key + 1 }}</span>
          <span class="points">{{ question.points }}</span>
          <span :class="['answers', {'blocked': question.blocked}]">{{ question.answers }}</span>
        </div>
      </div>
      <div :class="['team', {'highlight': highlightTeams.indexOf(team.name) >= 0}]" v-for="(team, key) in scoreboard.teams.filter((t, i) => showTop === 'all' || highlightTeams.indexOf(t.name) >= 0 ||  i < showTop)" :key="key">
        <div class="team-position">
          {{ team.position }}
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
  <div v-else>
    This challenge is not started yet!<br>
    Starting at: {{ new Date(this.challenge.startTime).toLocaleString() }}
  </div>
</template>

<script>
import Alert from '@/components/Alert.vue';
import ChallengeInfo from '@/components/ChallengeInfo.vue';
import Modal from '@/components/Modal.vue';
import { SlMagnifierAdd } from 'vue-icons-plus/sl';
export default {
    components: {SlMagnifierAdd, Modal, Alert, ChallengeInfo},
    data: () => ({
        available: [],
        availableCount: 0,
        challenges: [],
        filterByName: '',
        all: {challenges: [], count: -1}
    }),
    methods: {
        listJoinableChallenges() {
            this.$refs.findModal.show();
            this.$http.get("Challenge/Available").then(res => {
                this.available = res.data.challenges;
                this.availableCount = res.data.count;
            })
        },
        joinChallenge(challengeId) {
            this.$http.post("Challenge/JoinChallenge", {
                challengeId
            }).then(() => {
                this.$router.push({name: 'guest-admin', query: {id: challengeId}})
            }).catch(() => {
                alert('error');
            })
        }
    },
    mounted() {
        this.$http.get("Challenge/GetCurrentParticipations").then(res => {
            this.all = res.data;
        })
    }
}
</script>

<template>
    <InputBlock v-model="filterByName" label="Filter by name" placeholder="name..." />
    
    <Modal title="Find a Challenge" ref="findModal">
        <Alert type="warning" v-if="availableCount === 0">
            There are no joinable challenges.
        </Alert>
        <div class="bg-primary p-2 m-1" v-for="(challenge, key) in available" :key="key">
            {{ challenge.name }} ({{ new Date(challenge.startTime).toLocaleString() }})
            <button @click="joinChallenge(challenge.challengeId)" class="btn small">Join</button>
        </div>
    </Modal>
    <h2>
        Joined Challenges
        <button class="btn small" @click="listJoinableChallenges()"><SlMagnifierAdd /></button>
    </h2>
    <ChallengeInfo send="" guest="" v-for="(item, key) in all.challenges.filter(ch => ch.name.toLowerCase().indexOf(filterByName.toLowerCase()) >= 0)" :challenge="item" :key="key" />
</template>
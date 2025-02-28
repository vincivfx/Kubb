<script lang="ts">
import Alert from './Alert.vue';
import Badge from './Badge.vue';
export default {
    components: {Alert, Badge},
    props: ['passwd']
}
</script>

<template>

    <Alert :type="passwd.match(/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[.,;:@#!]).{10,}$/) ? 'success' : 'danger'">
        Due to security reasons, every password should respect the following rules:

        <ul>
            <li>must be at least 10 characters <Badge type="success" v-if="passwd.length >= 10">OK</Badge></li>
            <li>must contains at least an upper case and a lower case letter <Badge type="success" v-if="passwd.match(/(?=.*[a-z])(?=.*[A-Z])/)">OK</Badge></li>
            <li>must contains at least a number <Badge v-if="passwd.match(/(?=.*[0-9])/)" type="success">OK</Badge></li>
            <li>must contains at least one of the following symbols: <b>. , ; : @ # !</b> <Badge v-if="passwd.match(/(?=.*[.,;:@#!])/)" type="success">OK</Badge></li>
        </ul>
    </Alert>

</template>
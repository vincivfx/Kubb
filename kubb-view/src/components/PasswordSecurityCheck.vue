<script lang="ts">
import Alert from './Alert.vue';
import Badge from './Badge.vue';
export default {
    components: {Alert, Badge},
    data: () => ({
        verified: false
    })
}
</script>

<template>

    <Alert :type="verified ? 'success' : 'danger'">
        Due to security reasons, every password should respect the following rules:

        <ul>
            <li>must be at least 10 characters <Badge type="success">OK</Badge></li>
            <li>must contains at least an upper case and a lower case letter <Badge type="success">OK</Badge></li>
            <li>must contains at least a number <Badge type="success">OK</Badge></li>
            <li>must contains at least one of the following symbols: <b>. , ; : @ # !</b> <Badge type="success">OK</Badge></li>
        </ul>
    </Alert>

</template>
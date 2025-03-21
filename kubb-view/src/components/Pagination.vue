<template>
    <div class="text-center" v-if="count > perPage">
        <button @click="move(-1)" class="btn" :disabled="page === 0">
            &lt;
        </button>
        <span style="font-size: 24px; padding: 12px;">
            {{ page + 1 }}
        </span>
        <button @click="move(1)" class="btn" :disabled="(page + 1) * perPage > count">
            &gt;
        </button>
    </div>
</template>

<script>
export default {
    props: ['count', 'page', 'perPage'],
    methods: {
        move(direction) {
            this.$emit('move', direction);
        }
    }
}
</script>
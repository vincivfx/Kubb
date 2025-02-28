<template>
    <span :class="['badge', type]">
        <slot />
    </span>
</template>

<script>
export default {
    props: ['type']
}
</script>
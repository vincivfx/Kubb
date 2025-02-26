<template>
    <div class="input-block">
        <label>
            <slot />
            <input class="text-input">
        </label>
        <span class="input-under-text">{{ underText }}</span>


    </div>
</template>

<script>
export default {
    props: ["underText"]
}
</script>
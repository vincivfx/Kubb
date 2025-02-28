<template>
    <div class="input-block">
        <label>
            <slot />
            <input
                :type="type"
                :disabled="disabled"
                :placeholder="placeholder"
                :readonly="readonly"
                class="text-input"
                :value="modelValue"
                @focus="$emit('focus')"
                @focusout="$emit('focusout')"
                @input="$emit('update:modelValue', $event.target.value)"
            />
        </label>
        <span class="input-under-text">{{ underText }}</span>


    </div>
</template>

<script>
export default {
    props: ['underText', 'placeholder', 'disabled', 'type', 'readonly', 'modelValue'],
}
</script>
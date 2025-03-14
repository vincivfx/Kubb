<template>
    <div class="input-block">
        <label>
            {{ label }}
            <input
                :pattern="pattern"
                :required="required"
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
            <span class="input-under-text">{{ underText }}</span>
        </label>
        <div class="input-block-group">
            <slot />
        </div>

    </div>
</template>

<script>
export default {
    props: ['underText', 'placeholder', 'disabled', 'type', 'readonly', 'modelValue', 'label', 'pattern', 'required'],
}
</script>
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
                :readonly="readonly !== false && readonly !== undefined"
                class="text-input"
                :value="modelValue"
                @focus="$emit('focus')"
                @focusout="$emit('focusout')"
                @change="$emit('change', formatValue($event.target.value))"
                @keyup="$emit('keyup', formatValue($event.target.value))"
                @input="$emit('update:modelValue', formatValue($event.target.value))"
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
    methods: {
        formatValue(val) {
            if (this.type === 'number') return Number.parseInt(val);
            return val;
        }
    }
}
</script>
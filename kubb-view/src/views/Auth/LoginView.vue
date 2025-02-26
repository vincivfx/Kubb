<script setup lang="ts">
import InputBlock from '@/components/InputBlock.vue';

</script>

<template>
    
    <div class="container">
        <h2>Kubb Contest Platform :: Login</h2>
        <p>

        </p>

        <InputBlock>Type your email address</InputBlock>
        <InputBlock>Type your password</InputBlock>
        <p>
            Have you forgotten your password?
            <a href="#">Recover your password</a>
        </p>


        <input type="submit" class="btn primary" value="Login">
    </div>

    <div class="container">
        <h3>Register into Kubb Contest Platform</h3>
        <p>
            Have you never tried KCP before? Register right now!
        </p>
        <div class="text-right">
            <RouterLink :to="{name: 'register'}" class="btn secondary">Register right now!</RouterLink>
        </div>
    </div>

</template>
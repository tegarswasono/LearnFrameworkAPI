<script setup>
import { onMounted, onBeforeUnmount } from 'vue'

onMounted(() => {
  document.body.style.backgroundColor = '#F0F0F0'
})
onBeforeUnmount(() => {
  document.body.style.backgroundColor = ''
})
</script>
<template>
  <div class="col-12 text-center q-pt-xl">
    <h2 class="text-blue-grey-6 text-weight-bold">404</h2>
    <h3 class="text-blue-grey-7 text-weight-medium">Page Not Found</h3>
    <p class="text-blue-grey-3 text-weight-regular">
      Sorry, the page you are looking for does not exist.
    </p>
    <router-link :to="{ name: 'bookingindex' }">Go to Bookings</router-link>
  </div>
</template>

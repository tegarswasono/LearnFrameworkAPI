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
    <h2 class="text-blue-grey-6 text-weight-bold">403</h2>
    <h2 class="text-blue-grey-6 text-weight-bold">:(</h2>
    <h5 class="text-blue-grey-7 text-weight-bold">Forbidden</h5>
    <p class="text-blue-grey-3 text-weight-regular">
      You don't have permission to access this page or resource.
    </p>
    <router-link :to="{ name: 'bookingindex' }">Go to Bookings</router-link>
  </div>
</template>

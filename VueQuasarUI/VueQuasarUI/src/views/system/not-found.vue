<script setup>
import { onMounted, onBeforeUnmount } from 'vue'

const goBack = () => {
  window.history.back()
}

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
    <h2 class="text-blue-grey-6 text-weight-bold">:(</h2>
    <h5 class="text-blue-grey-7 text-weight-bold">Page Not Found</h5>
    <p class="text-blue-grey-3 text-weight-regular">
      Sorry, the page you are looking for does not exist.
    </p>
    <a href="#" @click.prevent="goBack">Go Back</a>
  </div>
</template>

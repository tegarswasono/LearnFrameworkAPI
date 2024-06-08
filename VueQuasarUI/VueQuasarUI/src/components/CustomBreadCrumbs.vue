<script setup lang="ts">
import type { IBreadCrumbsModel } from '@/models/BreadCrumbsModel'
defineProps({
  breadcrumbs: {
    type: Array<IBreadCrumbsModel>,
    required: true
  }
})
</script>
<template>
  <q-breadcrumbs style="margin-bottom: 30px">
    <q-breadcrumbs-el
      v-for="breadcrumb in breadcrumbs"
      :label="breadcrumb.label"
      :icon="breadcrumb.icon"
      v-bind:key="breadcrumb.label"
    />
  </q-breadcrumbs>
</template>

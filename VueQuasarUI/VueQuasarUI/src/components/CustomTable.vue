<script setup lang="ts">
import { ref, watch } from 'vue'

var props = defineProps({
  title: {
    type: String,
    required: true
  },
  columns: {
    type: Array<any>,
    required: true
  },
  rows: {
    type: Array,
    required: true,
    default: () => []
  },
  loading: {
    type: Boolean,
    required: true,
    default: false
  },
  pagination: {
    type: Object,
    required: true
  },
  OnRequest: {
    type: Function,
    required: true
  },
  onAdd: {
    type: Function,
    required: true
  },
  onView: {
    type: Function,
    required: true
  },
  onEdit: {
    type: Function,
    required: true
  },
  onDelete: {
    type: Function,
    required: true
  },
  dialog: {
    type: Boolean,
    required: true,
    default: false
  },
  onSubmit: {
    type: Function,
    required: true
  }
})

const dialogTitle = ref('Add ' + props.title)
const visibleSubmit = ref(true)
// const formRef: Ref<QForm | null> = ref(null)
const onAddLocal = () => {
  dialogLocal.value = true
  dialogTitle.value = 'Add ' + props.title
  visibleSubmit.value = true
  props.onAdd()
}
const onViewLocal = (value: any) => {
  dialogLocal.value = true
  dialogTitle.value = 'View ' + props.title
  visibleSubmit.value = false
  props.onView(value)
}
const onEditLocal = (value: any) => {
  dialogLocal.value = true
  dialogTitle.value = 'Edit ' + props.title
  visibleSubmit.value = true
  props.onEdit(value)
}

// local
const paginationLocal = ref({ ...props.pagination })
const dialogLocal = ref(props.dialog)
watch(
  () => props.pagination,
  (newPagination) => {
    paginationLocal.value = { ...newPagination }
  },
  { deep: true }
)
const emit = defineEmits(['update:pagination', 'update:dialog'])
watch(
  paginationLocal,
  (newPagination) => {
    emit('update:pagination', newPagination)
  },
  { deep: true }
)
watch(
  () => props.dialog,
  (newDialog) => {
    dialogLocal.value = newDialog
  },
  { deep: true }
)
watch(
  dialogLocal,
  (newDialog) => {
    emit('update:dialog', newDialog)
  },
  { deep: true }
)
</script>

<template>
  <!-- Table -->
  <q-table
    title="Product"
    :columns="columns"
    :rows="rows"
    row-key="name"
    separator="cell"
    v-model:pagination="paginationLocal"
    dense
    :loading="loading"
    @request="(val) => OnRequest(val)"
  >
    <template v-slot:top>
      <q-btn icon="add" size="sm" label="Add" color="secondary" @click="onAddLocal()" />
    </template>
    <template v-slot:body-cell-actions="props">
      <q-td :props="props">
        <q-btn
          dense
          round
          flat
          color="grey"
          size="xs"
          icon="visibility"
          @click="onViewLocal(props.row)"
        ></q-btn>
        <q-btn
          dense
          round
          flat
          color="grey"
          size="xs"
          icon="edit"
          @click="onEditLocal(props.row)"
        ></q-btn>
        <q-btn
          dense
          round
          flat
          color="grey"
          size="xs"
          icon="delete"
          @click="onDelete(props.row.id)"
        ></q-btn>
      </q-td>
    </template>
  </q-table>
  <!-- DetailView -->
  <q-dialog v-model="dialogLocal" backdrop-filter="blur(4px) saturate(150%)">
    <q-card style="width: 700px; max-width: 80vw">
      <q-card-section>
        <div class="text-h6">{{ dialogTitle }}</div>
      </q-card-section>
      <q-card-section class="q-pt-none">
        <slot name="detailView"></slot>
      </q-card-section>
      <q-card-actions align="right" class="bg-white text-teal">
        <q-btn label="Close" color="negative" size="sm" icon="cancel" v-close-popup />
        <q-btn
          label="Save"
          color="primary"
          size="sm"
          icon="save"
          @click="onSubmit()"
          v-if="visibleSubmit"
        />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

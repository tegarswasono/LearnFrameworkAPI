<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type { IUserModelCreateOrUpdate } from '@/helpers/api/user/userModel'
import { UserApi } from '@/helpers/api/user/userApi'

const quasar = useQuasar()
const userApi = new UserApi()

//listview
const loading = ref(false)
const users = ref()
const pagination = ref({
  sortBy: 'email',
  descending: false,
  page: 1,
  rowsPerPage: 20,
  rowsNumber: 0
})
const columns: any = [
  { name: 'actions', label: '', align: 'left', style: 'width:50px;' },
  { name: 'email', label: 'Email', field: 'email', sortable: true, align: 'left' },
  { name: 'fullname', label: 'Fullname', field: 'fullName', sortable: true, align: 'left' },
  { name: 'active', label: 'Active', field: 'activeInString', sortable: true, align: 'left' }
]
const getData = async () => {
  loading.value = true
  try {
    var response = await userApi.getAll(
      pagination.value.sortBy,
      pagination.value.descending,
      pagination.value.page,
      pagination.value.rowsPerPage
    )
    users.value = response.result
    pagination.value.rowsNumber = response.rowsNumber
  } catch (error) {
    /* empty */
  }
  loading.value = false
}
const OnRequest = async (props: any) => {
  const { page, rowsPerPage, sortBy, descending } = props.pagination
  pagination.value.sortBy = sortBy
  pagination.value.descending = descending
  pagination.value.page = page
  pagination.value.rowsPerPage = rowsPerPage
  await getData()
}

//Add, Edit, Delete
const dialog = ref(false)
const dialogTitle = ref('Add User')
const emptyModel = {
  id: '',
  email: '',
  fullName: '',
  active: true
}
const model: Ref<IUserModelCreateOrUpdate> = ref(emptyModel)
const formRef: Ref<QForm | null> = ref(null)
const onAdd = () => {
  dialog.value = true
  dialogTitle.value = 'Add User'
  model.value = emptyModel
}
const onView = async (row: any) => {
  dialogTitle.value = 'View User'
  dialog.value = true
  model.value = row
}
const onEdit = async (row: any) => {
  dialogTitle.value = 'Edit User'
  dialog.value = true
  model.value = row
}
const onDelete = async (id: string) => {
  quasar
    .dialog({
      title: '',
      message: 'Are you sure want to delete this data?',
      cancel: true,
      color: 'primary'
    })
    .onOk(async () => {
      let output = await userApi.delete(id)
      if (output) {
        quasar.notify({
          type: 'positive',
          message: output.message,
          position: 'bottom-right'
        })
      }
      await getData()
    })
    .onCancel(() => {})
}
const onSubmit = async () => {
  let isValid = await formFieldValidationHelper(formRef)
  if (isValid) {
    if (model.value.id) {
      let output = await userApi.update(model.value)
      if (output) {
        quasar.notify({
          type: 'positive',
          message: output.message,
          position: 'bottom-right'
        })
        dialog.value = false
        await getData()
      }
    } else {
      let output = await userApi.create(model.value)
      if (output) {
        quasar.notify({
          type: 'positive',
          message: output.message,
          position: 'bottom-right'
        })
        dialog.value = false
        await getData()
      }
    }
  }
}

//onMounted
onMounted(async () => {
  await getData()
})
</script>

<template>
  <!-- breadcrumbs -->
  <q-breadcrumbs style="margin-bottom: 30px">
    <q-breadcrumbs-el label="Home" icon="home" />
    <q-breadcrumbs-el label="Configuration" icon="settings" />
    <q-breadcrumbs-el label="User" />
  </q-breadcrumbs>
  <!-- table -->
  <div>
    <q-table
      title="User"
      :rows="users"
      :columns="columns"
      row-key="email"
      separator="cell"
      v-model:pagination="pagination"
      dense
      :loading="loading"
      @request="OnRequest"
    >
      <template v-slot:top>
        <q-btn icon="add" size="sm" label="Add" color="primary" @click="onAdd" />
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
            @click="onView(props.row)"
          ></q-btn>
          <q-btn
            dense
            round
            flat
            color="grey"
            size="xs"
            icon="edit"
            @click="onEdit(props.row)"
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
  </div>
  <!-- dialog -->
  <q-dialog v-model="dialog" backdrop-filter="blur(4px) saturate(150%)">
    <q-card style="width: 700px; max-width: 80vw">
      <q-card-section>
        <div class="text-h6">{{ dialogTitle }}</div>
      </q-card-section>

      <q-form ref="formRef" class="q-gutter-md">
        <q-card-section class="q-pt-none">
          <q-input
            filled
            v-model="model.email"
            type="email"
            label="Email *"
            lazy-rules
            dense
            maxlength="512"
            :rules="[(val) => (val && val.length > 0) || 'Email is required']"
          />
          <q-input
            filled
            v-model="model.fullName"
            label="FullName *"
            lazy-rules
            dense
            maxlength="50"
            :rules="[(val) => (val && val.length > 0) || 'FullName is required']"
          />
          <q-toggle v-model="model.active" label="Is Active" />
        </q-card-section>
        <q-card-actions align="right" class="bg-white text-teal">
          <q-btn flat label="Cancel" color="primary" v-close-popup />
          <q-btn flat label="Save" color="primary" :onclick="onSubmit" />
        </q-card-actions>
      </q-form>
    </q-card>
  </q-dialog>
</template>

<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { useQuasar } from 'quasar'
import { useRouter } from 'vue-router'
import { UserApi } from '@/helpers/user/userApi'
//import { IPagination } from '@/helpers/apiModel'
import type { IUserModel, IUserModelCreateOrUpdate } from '@/helpers/user/userModel'

const $q = useQuasar()
const router = useRouter()
const userApi = new UserApi()

//listview
const users = ref()
const user = ref()
const columns = [
  { name: 'actions', label: '', align: 'left', style: 'width:50px;' },
  { name: 'email', label: 'Email', field: 'email', sortable: true, align: 'left' },
  { name: 'fullname', label: 'Fullname', field: 'fullName', sortable: true, align: 'left' },
  { name: 'isActive', label: 'Active', field: 'isActive', sortable: true, align: 'left' }
]
const loading = ref(false)
const pagination = ref({
  sortBy: 'email',
  descending: false,
  page: 1,
  rowsPerPage: 20,
  rowsNumber: 0
})
const getData = async () => {
  loading.value = true
  var response = await userApi.getAll(
    pagination.value.sortBy,
    pagination.value.descending,
    pagination.value.page,
    pagination.value.rowsPerPage
  )
  users.value = response.result
  pagination.value.rowsNumber = response.rowsNumber
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
  isActive: true
}
const model: Ref<IUserModelCreateOrUpdate> = ref(emptyModel)
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
  $q.dialog({
    title: '',
    message: 'Are you sure want to delete this data?',
    cancel: true,
    color: 'dark'
  })
    .onOk(async () => {
      let output = await userApi.delete(id)
      if (output) {
        $q.notify({
          type: 'positive',
          message: output.message,
          position: 'bottom-right'
        })
      }
    })
    .onCancel(() => {})
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
    <q-breadcrumbs-el label="Configuration" icon="widgets" />
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

      <q-card-section class="q-pt-none">
        <q-form @submit="onSubmit" class="q-gutter-md">
          <q-input
            filled
            v-model="model.email"
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
          <q-toggle v-model="model.isActive" label="Is Active" />
        </q-form>
      </q-card-section>

      <q-card-actions align="right" class="bg-white text-teal">
        <q-btn flat label="Cancel" color="primary" v-close-popup />
        <q-btn flat label="Submit" color="primary" v-close-popup />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

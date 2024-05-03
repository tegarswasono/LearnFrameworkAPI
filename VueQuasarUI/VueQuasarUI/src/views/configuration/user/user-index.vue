<script setup>
import { onMounted, ref } from 'vue'
import UsersApi from '@/helpers/user/userApi'
import { useQuasar } from 'quasar'
import { useRouter } from 'vue-router'

const $q = useQuasar()
const router = useRouter()
const usersApi = new UsersApi()

const users = ref([])
const user = ref()

onMounted(async () => {
  await getData()
})

//table
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
  var response = await usersApi.getAll(
    pagination.value.sortBy,
    pagination.value.descending,
    pagination.value.page,
    pagination.value.rowsPerPage
  )
  users.value = response.result
  pagination.value.rowsNumber = response.rowsNumber
  loading.value = false
}
const OnRequest = async (props) => {
  const { page, rowsPerPage, sortBy, descending } = props.pagination
  pagination.value.sortBy = sortBy
  pagination.value.descending = descending
  pagination.value.page = page
  pagination.value.rowsPerPage = rowsPerPage
  await getData()
}

//AddEdit
const dialog = ref(false)
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
        <q-btn icon="add" size="sm" label="Add" color="primary" @click="dialog = true" />
      </template>
      <template v-slot:body-cell-actions="props">
        <q-td :props="props">
          <q-btn dense round flat color="grey" size="xs" icon="visibility"></q-btn>
          <q-btn dense round flat color="grey" size="xs" icon="edit"></q-btn>
          <q-btn dense round flat color="grey" size="xs" icon="delete"></q-btn>
        </q-td>
      </template>
    </q-table>
  </div>
  <!-- dialog -->
  <q-dialog v-model="dialog" persistent backdrop-filter="blur(4px) saturate(150%)">
    <q-card style="width: 700px; max-width: 80vw">
      <q-card-section>
        <div class="text-h6">Add User</div>
      </q-card-section>

      <q-card-section class="q-pt-none">
        <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-md">
          <q-input
            filled
            v-model="name"
            label="Your name *"
            hint="Name and surname"
            lazy-rules
            :rules="[(val) => (val && val.length > 0) || 'Please type something']"
          />

          <q-input
            filled
            type="number"
            v-model="age"
            label="Your age *"
            lazy-rules
            :rules="[
              (val) => (val !== null && val !== '') || 'Please type your age',
              (val) => (val > 0 && val < 100) || 'Please type a real age'
            ]"
          />

          <q-toggle v-model="accept" label="I accept the license and terms" />

          <div>
            <q-btn label="Submit" type="submit" color="primary" />
            <q-btn label="Reset" type="reset" color="primary" flat class="q-ml-sm" />
          </div>
        </q-form>
      </q-card-section>

      <q-card-actions align="right" class="bg-white text-teal">
        <q-btn flat label="Cancel" color="primary" v-close-popup />
        <q-btn flat label="Submit" color="primary" v-close-popup />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

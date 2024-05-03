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
</script>

<template>
  <q-breadcrumbs style="margin-bottom: 30px">
    <q-breadcrumbs-el label="Home" icon="home" />
    <q-breadcrumbs-el label="Configuration" icon="widgets" />
    <q-breadcrumbs-el label="User" />
  </q-breadcrumbs>
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
        <q-btn color="primary" icon="add" size="sm" label="Add" @click="addRow" />
      </template>
    </q-table>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import UsersApi from '@/helpers/user/userApi'
import { useQuasar } from 'quasar'
import { useRouter } from 'vue-router'

const $q = useQuasar()
const router = useRouter()
const users = ref([])
const user = ref()
const usersApi = new UsersApi()

const getData = async () => {
  users.value = await usersApi.getAll()
}

onMounted(async () => {
  await getData()
})

//table
const columns = [
  { name: 'email', label: 'Email', field: 'email', sortable: true, align: 'left' },
  { name: 'fullname', label: 'Fullname', field: 'fullName', sortable: true, align: 'left' },
  { name: 'active', label: 'Active', field: 'isActive', sortable: true, align: 'left' }
]
const initialPagination = {
  rowsPerPage: 20
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
      row-key="name"
      separator="cell"
      :pagination="initialPagination"
      dense
    >
      <template v-slot:top>
        <q-btn color="primary" icon="add" size="sm" label="Add" @click="addRow" />
      </template>
    </q-table>
  </div>
</template>

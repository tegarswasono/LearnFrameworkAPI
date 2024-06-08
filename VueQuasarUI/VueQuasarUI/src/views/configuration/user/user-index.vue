<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type { IUserModelCreateOrUpdate } from '@/helpers/api/user/userModel'
import UserApi from '@/helpers/api/user/userApi'

const quasar = useQuasar()
const userApi = new UserApi()

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
  {
    name: 'phoneNumber',
    label: 'PhoneNumber',
    field: 'phoneNumber',
    sortable: true,
    align: 'left'
  },
  { name: 'rolesInString', label: 'Roles', field: 'rolesInString', sortable: false, align: 'left' },
  { name: 'active', label: 'Active', field: 'activeInString', sortable: true, align: 'left' }
]
const dialog = ref(false)
const dialogTitle = ref('Add User')
const formReadonly = ref(false)
const visibleSubmit = ref(true)
const isPwdPassword = ref(true)
const showPasswordField = ref(false)
const model: Ref<IUserModelCreateOrUpdate> = ref({
  id: '',
  email: '',
  fullName: '',
  phoneNumber: '',
  active: true,
  password: '',
  roles: null
})
const formRef: Ref<QForm | null> = ref(null)
const roles = ref()

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

const onAdd = () => {
  dialog.value = true
  dialogTitle.value = 'Add User'
  model.value = {
    id: '',
    email: '',
    fullName: '',
    phoneNumber: '',
    active: true,
    password: '',
    roles: null
  }
  showPasswordField.value = true
  formReadonly.value = false
  visibleSubmit.value = true
}
const onView = async (row: any) => {
  dialogTitle.value = 'View User'
  dialog.value = true
  model.value = row
  showPasswordField.value = false
  formReadonly.value = true
  visibleSubmit.value = false
}
const onEdit = async (row: any) => {
  dialogTitle.value = 'Edit User'
  dialog.value = true
  model.value = row
  showPasswordField.value = false
  formReadonly.value = false
  visibleSubmit.value = true
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

onMounted(async () => {
  await getData()
  var roles1 = await userApi.datasourceRoles()
  roles.value = roles1
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
        <q-btn icon="add" size="sm" label="Add" color="secondary" @click="onAdd" />
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
            :readonly="formReadonly"
          />
          <q-input
            filled
            v-model="model.fullName"
            label="FullName *"
            lazy-rules
            dense
            maxlength="50"
            :rules="[(val) => (val && val.length > 0) || 'FullName is required']"
            :readonly="formReadonly"
          />
          <q-input
            filled
            v-model="model.phoneNumber"
            label="PhoneNumber"
            dense
            maxlength="50"
            :readonly="formReadonly"
            style="padding-bottom: 20px"
          />
          <q-select
            v-model="model.roles"
            label="Roles"
            clearable
            :options="roles"
            option-label="name"
            option-value="id"
            :readonly="formReadonly"
            dense
            filled
            style="padding-bottom: 20px"
            multiple
          />
          <q-input
            v-if="showPasswordField"
            v-model="model.password"
            filled
            :type="isPwdPassword ? 'password' : 'text'"
            label="Password"
            lazy-rules
            :rules="[
              (val) => (val && val.length > 0) || 'Password is required',
              (val) => val.length >= 8 || 'Password must be a minimum of 8 characters',
              (val) => {
                let hasUppercase = false
                let hasLowercase = false
                let hasNumberOrSpecialChar = false
                for (let i = 0; i < val.length; i++) {
                  const char = val[i]
                  if (char >= 'A' && char <= 'Z') {
                    hasUppercase = true
                  } else if (char >= 'a' && char <= 'z') {
                    hasLowercase = true
                  } else if ((char >= '0' && char <= '9') || (char >= '!' && char <= '/')) {
                    hasNumberOrSpecialChar = true
                  }
                }
                if (!hasUppercase || !hasLowercase || !hasNumberOrSpecialChar) {
                  return 'Password must contain 1 uppercase letter, 1 lowercase letter, and a number or symbol'
                }
                return true
              }
            ]"
            dense
            maxlength="100"
          >
            <template v-slot:append>
              <q-icon
                :name="isPwdPassword ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="isPwdPassword = !isPwdPassword"
              />
            </template>
          </q-input>
          <q-toggle v-model="model.active" label="Is Active" :disable="formReadonly" />
        </q-card-section>
        <q-card-actions align="right" class="bg-white text-teal">
          <q-btn label="Close" color="negative" size="sm" icon="cancel" v-close-popup />
          <q-btn
            label="Save"
            color="primary"
            size="sm"
            icon="save"
            :onclick="onSubmit"
            v-if="visibleSubmit"
          />
        </q-card-actions>
      </q-form>
    </q-card>
  </q-dialog>
</template>

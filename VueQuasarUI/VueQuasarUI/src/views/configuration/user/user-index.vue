<script setup lang="ts">
import { onMounted, ref, type Ref, inject, computed } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type { IUserModelCreateOrUpdate } from '@/helpers/api/user/userModel'
import UserApi from '@/helpers/api/user/userApi'
import type { IBreadCrumbsModel } from '@/models/BreadCrumbsModel'
import {
  stringRequired,
  emailRequired,
  dropdownRequired,
  numberShouldbeBiggerThanOrEqualsTo0,
  numberShouldbeBiggerThan0,
  passwordRequired
} from '@/helpers/rulesHelper'
import type { IMyPermissionModel } from '@/helpers/api/myProfile/myProfileModel'
import { UsersAdd, UsersEdit, UsersDelete } from '@/helpers/constantString'
import CustomBreadCrumbs from '@/components/CustomBreadCrumbs.vue'
import CustomTable from '@/components/CustomTable.vue'

const quasar = useQuasar()
const userApi = new UserApi()

const myPermission = inject<Ref<IMyPermissionModel[]>>('myPermission')
const canAdd = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == UsersAdd)) {
      return true
    }
  }
  return false
})
const canEdit = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == UsersEdit)) {
      return true
    }
  }
  return false
})
const canDelete = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == UsersDelete)) {
      return true
    }
  }
  return false
})

const loading = ref(false)
const rows = ref()
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
const formReadonly = ref(false)
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
    rows.value = response.result
    pagination.value.rowsNumber = response.rowsNumber
  } catch (error) {
    /* empty */
  }
  loading.value = false
}
const onRequest = async (props: any) => {
  const { page, rowsPerPage, sortBy, descending } = props.pagination
  pagination.value.sortBy = sortBy
  pagination.value.descending = descending
  pagination.value.page = page
  pagination.value.rowsPerPage = rowsPerPage
  await getData()
}

const onAdd = () => {
  dialog.value = true
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
}
const onView = async (row: any) => {
  dialog.value = true
  model.value = row
  showPasswordField.value = false
  formReadonly.value = true
}
const onEdit = async (row: any) => {
  dialog.value = true
  model.value = row
  showPasswordField.value = false
  formReadonly.value = false
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

//breadcrumbs
const breadcrumbs = ref<IBreadCrumbsModel[]>([
  { label: 'Home', icon: 'home' },
  { label: 'Configuration', icon: 'settings' },
  { label: 'Users', icon: 'group' }
])

onMounted(async () => {
  await getData()
  var roles1 = await userApi.datasourceRoles()
  roles.value = roles1
})
</script>

<template>
  <!-- breadcrumbs -->
  <CustomBreadCrumbs :breadcrumbs="breadcrumbs" />
  <!-- table -->
  <CustomTable
    title="User"
    :columns="columns"
    :rows="rows"
    :loading="loading"
    :pagination="pagination"
    :onRequest="onRequest"
    :onView="onView"
    :onAdd="canAdd ? onAdd : undefined"
    :onEdit="canEdit ? onEdit : undefined"
    :onDelete="canDelete ? onDelete : undefined"
    :dialog="dialog"
    :onSubmit="onSubmit"
  >
    <template #detailView>
      <q-form ref="formRef" class="q-gutter-md">
        <q-input
          filled
          v-model="model.email"
          type="email"
          label="Email *"
          lazy-rules
          dense
          maxlength="512"
          :rules="emailRequired('Email')"
          :readonly="formReadonly"
        />
        <q-input
          filled
          v-model="model.fullName"
          label="FullName *"
          lazy-rules
          dense
          maxlength="50"
          :rules="stringRequired('Fullname')"
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
          :rules="passwordRequired('Password')"
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
      </q-form>
    </template>
  </CustomTable>
</template>

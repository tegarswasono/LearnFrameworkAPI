<script setup lang="ts">
import { onMounted, ref, type Ref, computed, inject } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type {
  IRoleModelCreateOrUpdate,
  IRoleFunctionModel,
  IRoleFunctionModelItem
} from '@/helpers/api/role/roleModel'
import RoleApi from '@/helpers/api/role/roleApi'
import type { IBreadCrumbsModel } from '@/models/BreadCrumbsModel'
import {
  stringRequired,
  dropdownRequired,
  numberShouldbeBiggerThanOrEqualsTo0,
  numberShouldbeBiggerThan0
} from '@/helpers/rulesHelper'
import type { IMyPermissionModel } from '@/helpers/api/myProfile/myProfileModel'
import { RolesAdd, RolesEdit, RolesDelete } from '@/helpers/constantString'
import CustomBreadCrumbs from '@/components/CustomBreadCrumbs.vue'
import CustomTable from '@/components/CustomTable.vue'

const quasar = useQuasar()
const roleApi = new RoleApi()

const myPermission = inject<Ref<IMyPermissionModel[]>>('myPermission')
const canAdd = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == RolesAdd)) {
      return true
    }
  }
  return false
})
const canEdit = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == RolesEdit)) {
      return true
    }
  }
  return false
})
const canDelete = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == RolesDelete)) {
      return true
    }
  }
  return false
})

const rows = ref()
const loading = ref(false)
const columns: any = [
  { name: 'actions', label: '', align: 'left', style: 'width:50px;' },
  { name: 'name', label: 'Name', field: 'name', sortable: true, align: 'left' }
]
const pagination = ref({
  sortBy: 'name',
  descending: false,
  page: 1,
  rowsPerPage: 20,
  rowsNumber: 0
})
const dialog = ref(false)
const formReadonly = ref(false)
const model: Ref<IRoleModelCreateOrUpdate> = ref({
  id: '',
  name: '',
  RoleFunctions: []
})
const formRef: Ref<QForm | null> = ref(null)

const columnsRoleFunction: any = [
  { name: 'actions', label: '', align: 'left', style: 'width:10px' },
  { name: 'module', label: 'Module', align: 'left', field: 'moduleId', style: 'width:100px' },
  { name: 'function1', label: 'Function', align: 'left' }
]
const roleFunctionsPagination = ref({
  rowsPerPage: 0
})
//const roleFunctions = ref()
const checkbox1 = ref(false)

const getData = async () => {
  loading.value = true
  try {
    var response = await roleApi.getAll(
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
const onAdd = async () => {
  dialog.value = true
  formReadonly.value = false
  model.value = {
    id: '',
    name: '',
    RoleFunctions: []
  }
  await refreshRoleFunction('')
}
const onView = async (row: any) => {
  dialog.value = true
  formReadonly.value = true
  model.value = row
  await refreshRoleFunction(row.id)
}
const onEdit = async (row: any) => {
  dialog.value = true
  formReadonly.value = false
  model.value = JSON.parse(JSON.stringify(row))
  await refreshRoleFunction(row.id)
}
const refreshRoleFunction = async (id: string) => {
  var output = await roleApi.roleFunctions(id)
  model.value.RoleFunctions = output
  var isCheckedFalse = output.find(({ isChecked }) => isChecked == false)
  if (isCheckedFalse == null) {
    checkbox1.value = true
  } else {
    checkbox1.value = false
  }
}
const onCheckbox1Change = async (value: boolean) => {
  model.value.RoleFunctions.forEach((item: IRoleFunctionModel, index: any) => {
    item.isChecked = value
    item.items.forEach((item1: IRoleFunctionModelItem, index1: number) => {
      item1.isChecked = value
    })
  })
}
const onCheckbox2Change = async (value: boolean, param1: IRoleFunctionModel) => {
  param1.items.forEach((item: IRoleFunctionModelItem, index: any) => {
    item.isChecked = value
  })
}
const onCheckbox3Change = async (value: boolean, param1: IRoleFunctionModel) => {
  if (value) {
    var isCheckedFalse = param1.items.find(({ isChecked }) => isChecked == false)
    if (isCheckedFalse == null) {
      param1.isChecked = true
    }
  } else {
    param1.isChecked = false
  }
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
      let output = await roleApi.delete(id)
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
  //role
  let isValid = await formFieldValidationHelper(formRef)
  if (isValid) {
    if (model.value.id) {
      let output = await roleApi.update(model.value)
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
      let output = await roleApi.create(model.value)
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

  //
}

//breadcrumbs
const breadcrumbs = ref<IBreadCrumbsModel[]>([
  { label: 'Home', icon: 'home' },
  { label: 'Configuration', icon: 'settings' },
  { label: 'Role', icon: 'key' }
])

onMounted(async () => {
  await getData()
})
</script>

<template>
  <!-- breadcrumbs -->
  <CustomBreadCrumbs :breadcrumbs="breadcrumbs" />
  <!-- table -->
  <CustomTable
    title="Role"
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
          v-model="model.name"
          label="Name *"
          lazy-rules
          dense
          maxlength="512"
          :rules="stringRequired('Name')"
          :readonly="formReadonly"
        />
        <q-table
          :columns="columnsRoleFunction"
          :rows="model.RoleFunctions"
          :pagination="roleFunctionsPagination"
          dense
          hide-bottom
        >
          <template v-slot:header-cell-actions="props">
            <q-th :props="props">
              <q-checkbox
                v-model="checkbox1"
                size="xs"
                :disable="formReadonly"
                @update:model-value="onCheckbox1Change"
              />
            </q-th>
          </template>
          <template v-slot:body-cell-actions="props">
            <q-td :props="props">
              <q-checkbox
                v-model="props.row.isChecked"
                size="xs"
                :disable="formReadonly"
                @update:model-value="(val: boolean) => onCheckbox2Change(val, props.row)"
              />
            </q-td>
          </template>
          <template v-slot:body-cell-function1="props">
            <q-td :props="props">
              <template v-for="item in props.row.items" v-bind:key="item.id">
                <q-checkbox
                  v-model="item.isChecked"
                  size="xs"
                  :label="item.name"
                  :disable="formReadonly"
                  @update:model-value="(val: boolean) => onCheckbox3Change(val, props.row)"
                />
                &nbsp;&nbsp;
              </template>
            </q-td>
          </template>
        </q-table>
      </q-form>
    </template>
  </CustomTable>
</template>

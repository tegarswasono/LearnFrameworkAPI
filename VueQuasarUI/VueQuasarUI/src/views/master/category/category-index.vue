<script setup lang="ts">
import { onMounted, ref, type Ref, computed, inject } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type { ICategoryModelCreateOrUpdate } from '@/helpers/api/category/categoryModel'
import CategoryApi from '@/helpers/api/category/categoryApi'
import type { IBreadCrumbsModel } from '@/models/BreadCrumbsModel'
import {
  stringRequired,
  dropdownRequired,
  numberShouldbeBiggerThanOrEqualsTo0,
  numberShouldbeBiggerThan0
} from '@/helpers/rulesHelper'
import type { IMyPermissionModel } from '@/helpers/api/myProfile/myProfileModel'
import { CategoryAdd, CategoryEdit, CategoryDelete } from '@/helpers/constantString'
import CustomBreadCrumbs from '@/components/CustomBreadCrumbs.vue'
import CustomTable from '@/components/CustomTable.vue'

const quasar = useQuasar()
const categoryApi = new CategoryApi()

const myPermission = inject<Ref<IMyPermissionModel[]>>('myPermission')
const canAdd = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == CategoryAdd)) {
      return true
    }
  }
  return false
})
const canEdit = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == CategoryEdit)) {
      return true
    }
  }
  return false
})
const canDelete = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == CategoryDelete)) {
      return true
    }
  }
  return false
})

const loading = ref(false)
const rows = ref()
const pagination = ref({
  sortBy: 'name',
  descending: false,
  page: 1,
  rowsPerPage: 20,
  rowsNumber: 0
})
const columns: any = [
  { name: 'actions', label: '', align: 'left', style: 'width:50px;' },
  { name: 'name', label: 'Name', field: 'name', sortable: true, align: 'left' }
]
const getData = async () => {
  loading.value = true
  try {
    var response = await categoryApi.getAll(
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

const dialog = ref(false)
const formReadonly = ref(false)
const model: Ref<ICategoryModelCreateOrUpdate> = ref({
  id: '',
  name: '',
  description: ''
})
const formRef: Ref<QForm | null> = ref(null)
const onAdd = () => {
  dialog.value = true
  formReadonly.value = false
  model.value = {
    id: '',
    name: '',
    description: ''
  }
}
const onView = async (row: any) => {
  dialog.value = true
  formReadonly.value = true
  model.value = row
}
const onEdit = async (row: any) => {
  dialog.value = true
  formReadonly.value = false
  model.value = JSON.parse(JSON.stringify(row))
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
      let output = await categoryApi.delete(id)
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
      let output = await categoryApi.update(model.value)
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
      let output = await categoryApi.create(model.value)
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
  { label: 'Master', icon: 'assignment' },
  { label: 'Category', icon: 'category' }
])

//onMounted
onMounted(async () => {
  await getData()
})
</script>

<template>
  <!-- breadcrumbs -->
  <CustomBreadCrumbs :breadcrumbs="breadcrumbs" />
  <!-- table -->
  <CustomTable
    title="Category"
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
          maxlength="100"
          :rules="stringRequired('Name')"
          :readonly="formReadonly"
        />
      </q-form>
    </template>
  </CustomTable>
</template>

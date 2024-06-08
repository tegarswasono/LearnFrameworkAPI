<script setup lang="ts">
import { onMounted, ref, type Ref, inject, computed } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type { IProductModel, IProductModelCreateOrUpdate } from '@/helpers/api/product/productModel'
import ProductApi from '@/helpers/api/product/productApi'
import DatasourceApi from '@/helpers/api/datasource/datasourceApi'
import CustomTable from '@/components/CustomTable.vue'
import {
  stringRequired,
  dropdownRequired,
  numberShouldbeBiggerThanOrEqualsTo0,
  numberShouldbeBiggerThan0
} from '@/helpers/rulesHelper'
import type { IMyPermissionModel } from '@/helpers/api/myProfile/myProfileModel'
import { ProductAdd, ProductEdit, ProductDelete } from '@/helpers/constantString'
import type { IBreadCrumbsModel } from '@/models/BreadCrumbsModel'
import CustomBreadCrumbs from '@/components/CustomBreadCrumbs.vue'

const quasar = useQuasar()
const productApi = new ProductApi()
const datasourceApi = new DatasourceApi()

const myPermission = inject<Ref<IMyPermissionModel[]>>('myPermission')
const canAdd = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == ProductAdd)) {
      return true
    }
  }
  return false
})
const canEdit = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == ProductEdit)) {
      return true
    }
  }
  return false
})
const canDelete = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == ProductDelete)) {
      return true
    }
  }
  return false
})

const breadcrumbs = ref<IBreadCrumbsModel[]>([
  { label: 'Home', icon: 'home' },
  { label: 'Master', icon: 'assignment' },
  { label: 'Product', icon: 'inventory' }
])
const loading = ref(false)
const columns: any = [
  { name: 'actions', label: '', align: 'left', style: 'width:50px;' },
  { name: 'name', label: 'Name', field: 'name', sortable: true, align: 'left' },
  { name: 'stok', label: 'Stok', field: 'stok', sortable: true, align: 'left' },
  { name: 'price', label: 'Price', field: 'price', sortable: true, align: 'left' },
  {
    name: 'category.name',
    label: 'Category',
    field: (row: IProductModel) => row.category.name,
    sortable: true,
    align: 'left'
  }
]
const rows = ref()
const pagination = ref({
  sortBy: 'name',
  descending: false,
  page: 1,
  rowsPerPage: 20,
  rowsNumber: 0
})
const dialog = ref(false)
const formReadonly = ref(false)
const model: Ref<IProductModelCreateOrUpdate> = ref({
  id: '',
  name: '',
  description: '',
  stok: 0,
  price: 0,
  categoryId: ''
})
const formRef: Ref<QForm | null> = ref(null)

const getData = async () => {
  loading.value = true
  try {
    var response = await productApi.getAll(
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
  formReadonly.value = false
  model.value = {
    id: '',
    name: '',
    description: '',
    stok: 0,
    price: 0,
    categoryId: ''
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
      let output = await productApi.delete(id)
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
      let output = await productApi.update(model.value)
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
      let output = await productApi.create(model.value)
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

//datasource
const categories = ref()
const GetCategories = async () => {
  categories.value = await datasourceApi.category()
}

//onMounted
onMounted(async () => {
  await getData()
  await GetCategories()
})
</script>

<template>
  <!-- breadcrumbs -->
  <CustomBreadCrumbs :breadcrumbs="breadcrumbs" />
  <!-- table -->
  <CustomTable
    title="Product"
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
      <q-form class="q-gutter-md" ref="formRef">
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
        <q-input
          type="number"
          filled
          v-model="model.stok"
          label="Stok *"
          lazy-rules
          dense
          :rules="numberShouldbeBiggerThanOrEqualsTo0('Stok')"
          :readonly="formReadonly"
        />
        <q-input
          type="number"
          filled
          v-model="model.price"
          label="Price *"
          lazy-rules
          dense
          :rules="numberShouldbeBiggerThan0('Price')"
          :readonly="formReadonly"
        />
        <q-select
          filled
          dense
          v-model="model.categoryId"
          label="Category"
          :options="categories"
          clearable
          emit-value
          map-options
          :readonly="formReadonly"
          lazy-rules
          :rules="dropdownRequired('Category')"
        />
        <q-input
          type="textarea"
          filled
          v-model="model.description"
          label="Description"
          lazy-rules
          dense
          maxlength="300"
          :readonly="formReadonly"
          class="textarea-no-resize"
        />
      </q-form>
    </template>
  </CustomTable>
</template>

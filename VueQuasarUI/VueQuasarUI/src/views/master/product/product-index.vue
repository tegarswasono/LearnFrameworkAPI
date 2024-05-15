<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type { IProductModel, IProductModelCreateOrUpdate } from '@/helpers/api/product/productModel'
import { ProductApi } from '@/helpers/api/product/productApi'
import type { IGeneralDatasourceModel } from '@/helpers/api/datasource/datasourceModel'
import { DatasourceApi } from '@/helpers/api/datasource/datasourceApi'

const quasar = useQuasar()
const productApi = new ProductApi()
const datasourceApi = new DatasourceApi()

//listview
const loading = ref(false)
const products = ref()
const pagination = ref({
  sortBy: 'name',
  descending: false,
  page: 1,
  rowsPerPage: 20,
  rowsNumber: 0
})
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
const getData = async () => {
  loading.value = true
  try {
    var response = await productApi.getAll(
      pagination.value.sortBy,
      pagination.value.descending,
      pagination.value.page,
      pagination.value.rowsPerPage
    )
    products.value = response.result
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
const dialogTitle = ref('Add Product')
const visibleSubmit = ref(true)
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
const categories = ref()

const onAdd = () => {
  dialog.value = true
  dialogTitle.value = 'Add Product'
  visibleSubmit.value = true
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
  dialogTitle.value = 'View Product'
  visibleSubmit.value = false
  formReadonly.value = true

  model.value = row
}
const onEdit = async (row: any) => {
  dialog.value = true
  dialogTitle.value = 'Edit Product'
  visibleSubmit.value = true
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
  <q-breadcrumbs style="margin-bottom: 30px">
    <q-breadcrumbs-el label="Home" icon="home" />
    <q-breadcrumbs-el label="Master" icon="assignment" />
    <q-breadcrumbs-el label="Product" />
  </q-breadcrumbs>
  <!-- table -->
  <div>
    <q-table
      title="Product"
      :rows="products"
      :columns="columns"
      row-key="name"
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
            v-model="model.name"
            label="Name *"
            lazy-rules
            dense
            maxlength="100"
            :rules="[(val) => (val && val.length > 0) || 'Name is required']"
            :readonly="formReadonly"
          />
          <q-input
            type="number"
            filled
            v-model="model.stok"
            label="Stok *"
            lazy-rules
            dense
            :rules="[
              (val) => val !== '' || 'Stok is required',
              (val) => val >= 0 || 'Stok Should be bigger than or equals to zero'
            ]"
            :readonly="formReadonly"
          />
          <q-input
            type="number"
            filled
            v-model="model.price"
            label="Price *"
            lazy-rules
            dense
            :rules="[
              (val) => val !== '' || 'Price is required',
              (val) => val > 0 || 'Price Should be bigger than zero'
            ]"
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
            :rules="[(val) => (val && val.length > 0) || 'Category is required']"
          />
          <br />
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
        </q-card-section>
        <q-card-actions align="right" class="bg-white text-teal">
          <q-btn flat label="Close" color="primary" v-close-popup />
          <q-btn flat label="Save" color="primary" :onclick="onSubmit" v-if="visibleSubmit" />
        </q-card-actions>
      </q-form>
    </q-card>
  </q-dialog>
</template>

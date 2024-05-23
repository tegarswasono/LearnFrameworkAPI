<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type { IRoleModelCreateOrUpdate } from '@/helpers/api/role/roleModel'
import type {
  IModuleFunctionModel,
  IModuleFunctionModelItem
} from '@/helpers/api/moduleFunction/moduleFunctionModel'
import { RoleApi } from '@/helpers/api/role/roleApi'

const quasar = useQuasar()
const roleApi = new RoleApi()

const roles = ref()
const loadingTable = ref(false)
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
const dialogTitle = ref('Add Role')
const visibleSubmit = ref(true)
const formReadonly = ref(false)
const model: Ref<IRoleModelCreateOrUpdate> = ref({
  id: '',
  name: ''
})
const formRef: Ref<QForm | null> = ref(null)

const columnsModuleFunction: any = [
  { name: 'actions', label: '', align: 'left', style: 'width:10px' },
  { name: 'module', label: 'Module', align: 'left', field: 'moduleId', style: 'width:100px' },
  { name: 'function1', label: 'Function', align: 'left' }
]
const moduleFunctionsPagination = ref({
  rowsPerPage: 0
})
const moduleFunctions = ref()
const checkbox1 = ref(false)

const getData = async () => {
  loadingTable.value = true
  try {
    var response = await roleApi.getAll(
      pagination.value.sortBy,
      pagination.value.descending,
      pagination.value.page,
      pagination.value.rowsPerPage
    )
    roles.value = response.result
    pagination.value.rowsNumber = response.rowsNumber
  } catch (error) {
    /* empty */
  }
  loadingTable.value = false
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
  dialogTitle.value = 'Add Role'
  visibleSubmit.value = true
  formReadonly.value = false

  model.value = {
    id: '',
    name: ''
  }
}
const onView = async (row: any) => {
  dialog.value = true
  dialogTitle.value = 'View Role'
  visibleSubmit.value = false
  formReadonly.value = true

  model.value = row
  await refreshAvailableModule()
}
const onEdit = async (row: any) => {
  dialog.value = true
  dialogTitle.value = 'Edit Role'
  visibleSubmit.value = true
  formReadonly.value = false

  model.value = JSON.parse(JSON.stringify(row))
  await refreshAvailableModule()
}
const refreshAvailableModule = async () => {
  var output = await roleApi.getAllModules()
  moduleFunctions.value = output
}
const onCheckbox1Change = async (value: boolean) => {
  moduleFunctions.value.forEach((item: IModuleFunctionModel, index: any) => {
    item.isChecked = value
    item.items.forEach((item1: IModuleFunctionModelItem, index1: number) => {
      item1.isChecked = value
    })
  })
}
const onCheckbox2Change = async (value: boolean, param1: IModuleFunctionModel) => {
  param1.items.forEach((item: IModuleFunctionModelItem, index: any) => {
    item.isChecked = value
  })
}
const onCheckbox3Change = async (value: boolean, param1: IModuleFunctionModel) => {
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
}

onMounted(async () => {
  await getData()
})
</script>

<template>
  <!-- breadcrumbs -->
  <q-breadcrumbs style="margin-bottom: 30px">
    <q-breadcrumbs-el label="Home" icon="home" />
    <q-breadcrumbs-el label="Configuration" icon="settings" />
    <q-breadcrumbs-el label="Role" />
  </q-breadcrumbs>
  <!-- table -->
  <div>
    <q-table
      title="Role"
      :rows="roles"
      :columns="columns"
      row-key="email"
      separator="cell"
      v-model:pagination="pagination"
      dense
      :loading="loadingTable"
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
            v-model="model.name"
            label="Name *"
            lazy-rules
            dense
            maxlength="512"
            :rules="[(val) => (val && val.length > 0) || 'Name is required']"
            :readonly="formReadonly"
          />
          <q-table
            :columns="columnsModuleFunction"
            :rows="moduleFunctions"
            :pagination="moduleFunctionsPagination"
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

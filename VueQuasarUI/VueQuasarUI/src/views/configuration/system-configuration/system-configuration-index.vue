<script setup lang="ts">
import { onMounted, ref, type Ref, computed, inject } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type { ISystemConfigurationModelCreateOrUpdate } from '@/helpers/api/systemConfiguration/systemConfigurationModel'
import SystemConfigurationApi from '@/helpers/api/systemConfiguration/systemConfigurationApi'
import type { IBreadCrumbsModel } from '@/models/BreadCrumbsModel'
import {
  stringRequired,
  dropdownRequired,
  numberShouldbeBiggerThanOrEqualsTo0,
  numberShouldbeBiggerThan0
} from '@/helpers/rulesHelper'
import type { IMyPermissionModel } from '@/helpers/api/myProfile/myProfileModel'
import { SystemConfigurationCreateOrUpdate } from '@/helpers/constantString'
import CustomBreadCrumbs from '@/components/CustomBreadCrumbs.vue'

const quasar = useQuasar()
const scApi = new SystemConfigurationApi()

const myPermission = inject<Ref<IMyPermissionModel[]>>('myPermission')
const canCreateOrUpdate = computed(() => {
  if (myPermission && myPermission.value) {
    if (
      myPermission.value.some(({ functionId }) => functionId == SystemConfigurationCreateOrUpdate)
    ) {
      return true
    }
  }
  return false
})

const model: Ref<ISystemConfigurationModelCreateOrUpdate> = ref({
  id: '',
  appBaseUrl: '',
  defaultRoleId: '',
  exampleSetting: ''
})
const formRef: Ref<QForm | null> = ref(null)
const getData = async () => {
  var response = await scApi.get()
  model.value = response
}
const onSubmit = async () => {
  let isValid = await formFieldValidationHelper(formRef)
  if (isValid) {
    var response = await scApi.createOrUpdate(model.value)
    if (response) {
      quasar.notify({
        type: 'positive',
        message: response.message,
        position: 'bottom-right'
      })
    }
  }
}

//breadcrumbs
const breadcrumbs = ref<IBreadCrumbsModel[]>([
  { label: 'Home', icon: 'home' },
  { label: 'Configuration', icon: 'settings' },
  { label: 'System Configuration', icon: 'manufacturing' }
])

//dropdown
const roles = ref()
const getRoles = async () => {
  roles.value = await scApi.datasourceRoles()
}
onMounted(async () => {
  await getData()
  await getRoles()
})
</script>
<template>
  <!-- breadcrumbs -->
  <CustomBreadCrumbs :breadcrumbs="breadcrumbs" />

  <!-- systemconfiguration -->
  <div class="q-pa-md" style="border: 1px solid #eeeeee">
    <q-form class="q-gutter-md" ref="formRef">
      <q-input
        type="text"
        v-model="model.appBaseUrl"
        label="App Base Url"
        filled
        dense
        maxlength="50"
        lazy-rules
        :rules="stringRequired('App Base Url')"
        :readonly="!canCreateOrUpdate"
      />
      <q-select
        v-model="model.defaultRoleId"
        :options="roles"
        label="Default Role"
        filled
        dense
        clearable
        emit-value
        map-options
        lazy-rules
        :rules="dropdownRequired('Default Role')"
      />
      <q-input
        type="text"
        filled
        v-model="model.exampleSetting"
        label="Example Setting"
        lazy-rules
        :rules="stringRequired('Example Setting')"
        dense
        maxlength="50"
        :readonly="!canCreateOrUpdate"
      />
      <div class="row q-ml-auto">
        <div class="col-12 q-px-sm flex justify-end">
          <q-btn
            label="Save"
            color="primary"
            icon="save"
            style="padding-left: 10px; padding-right: 10px"
            size="sm"
            fixed-right
            @click="onSubmit"
            v-if="canCreateOrUpdate"
          />
        </div>
      </div>
    </q-form>
  </div>
</template>

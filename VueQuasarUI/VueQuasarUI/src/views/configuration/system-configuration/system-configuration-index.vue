<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type {
  ISystemConfigurationModel,
  ISystemConfigurationModelCreateOrUpdate
} from '@/helpers/api/systemConfiguration/systemConfigurationModel'
import { SystemConfigurationApi } from '@/helpers/api/systemConfiguration/systemConfigurationApi'

const quasar = useQuasar()
const scApi = new SystemConfigurationApi()

//createOrEdit
const model: Ref<ISystemConfigurationModelCreateOrUpdate> = ref({
  id: '',
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

onMounted(async () => {
  await getData()
})
</script>
<template>
  <!-- breadcrumbs -->
  <q-breadcrumbs style="margin-bottom: 30px">
    <q-breadcrumbs-el label="Home" icon="home" />
    <q-breadcrumbs-el label="Configuration" icon="settings" />
    <q-breadcrumbs-el label="System Configuration" />
  </q-breadcrumbs>

  <!-- systemconfiguration -->
  <div class="q-pa-md" style="border: 1px solid #eeeeee">
    <q-form class="q-gutter-md" ref="formRef">
      <q-input
        filled
        v-model="model.exampleSetting"
        label="Example Setting"
        lazy-rules
        :rules="[(val) => (val && val.length > 0) || 'Example Setting is Required']"
        dense
        maxlength="100"
      />
      <div class="row q-ml-auto">
        <div class="col-12 q-px-sm flex justify-end">
          <q-btn
            label="Submit"
            color="primary"
            icon="save"
            style="padding-left: 10px; padding-right: 10px"
            dense
            fixed-right
            @click="onSubmit"
          />
        </div>
      </div>
    </q-form>
  </div>
</template>

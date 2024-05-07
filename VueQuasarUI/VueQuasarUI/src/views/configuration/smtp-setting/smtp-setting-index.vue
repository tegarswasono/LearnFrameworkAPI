<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type { ISmtpSettingModelCreateOrUpdate } from '@/helpers/api/smtpSetting/smtpSettingModel'
import { SmtpSettingApi } from '@/helpers/api/smtpSetting/smtpSettingApi'

const quasar = useQuasar()
const ssApi = new SmtpSettingApi()

//createOrEdit
const model: Ref<ISmtpSettingModelCreateOrUpdate> = ref({
  id: '',
  smtpServer: '',
  smtpPort: null,
  smtpUser: '',
  smtpPassword: '',
  smtpIsUseSsl: true
})
const formRef: Ref<QForm | null> = ref(null)
const isPwd = ref(true)

const getData = async () => {
  var response = await ssApi.get()
  model.value = response
}
const onSubmit = async () => {
  let isValid = await formFieldValidationHelper(formRef)
  if (isValid) {
    var response = await ssApi.createOrUpdate(model.value)
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
    <q-breadcrumbs-el label="SMTP Setting" />
  </q-breadcrumbs>

  <!-- systemconfiguration -->
  <div class="q-pa-md" style="border: 1px solid #eeeeee">
    <q-form class="q-gutter-md" ref="formRef">
      <q-input
        filled
        v-model="model.smtpServer"
        label="Smtp Server"
        lazy-rules
        :rules="[(val) => (val && val.length > 0) || 'Smtp Server is Required']"
        dense
        maxlength="100"
      />
      <q-input
        type="number"
        filled
        v-model="model.smtpPort"
        label="Smtp Port"
        lazy-rules
        :rules="[(val) => (val !== null && val !== '') || 'Smtp Port is Required']"
        dense
      />
      <q-input
        filled
        v-model="model.smtpUser"
        label="Smtp User"
        lazy-rules
        :rules="[(val) => (val && val.length > 0) || 'Smtp User is Required']"
        dense
        maxlength="100"
      />
      <q-input
        v-model="model.smtpPassword"
        filled
        :type="isPwd ? 'password' : 'text'"
        label="Smtp Password"
        lazy-rules
        :rules="[(val) => (val && val.length > 0) || 'Smtp Password is Required']"
        dense
        maxlength="100"
      >
        <template v-slot:append>
          <q-icon
            :name="isPwd ? 'visibility_off' : 'visibility'"
            class="cursor-pointer"
            @click="isPwd = !isPwd"
          />
        </template>
      </q-input>
      <div class="row q-ml-auto">
        <div class="col-12 q-px-sm flex justify-end">
          <q-btn
            label="Save"
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

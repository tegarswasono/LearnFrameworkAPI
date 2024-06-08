<script setup lang="ts">
import { onMounted, ref, type Ref, computed, inject } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type { ISmtpSettingModelCreateOrUpdate } from '@/helpers/api/smtpSetting/smtpSettingModel'
import SmtpSettingApi from '@/helpers/api/smtpSetting/smtpSettingApi'
import type { IBreadCrumbsModel } from '@/models/BreadCrumbsModel'
import {
  stringRequired,
  dropdownRequired,
  numberShouldbeBiggerThanOrEqualsTo0,
  numberShouldbeBiggerThan0,
  numberRequired
} from '@/helpers/rulesHelper'
import type { IMyPermissionModel } from '@/helpers/api/myProfile/myProfileModel'
import { SMTPSettingCreateOrUpdate } from '@/helpers/constantString'
import CustomBreadCrumbs from '@/components/CustomBreadCrumbs.vue'

const quasar = useQuasar()
const ssApi = new SmtpSettingApi()

const myPermission = inject<Ref<IMyPermissionModel[]>>('myPermission')
const canCreateOrUpdate = computed(() => {
  if (myPermission && myPermission.value) {
    if (myPermission.value.some(({ functionId }) => functionId == SMTPSettingCreateOrUpdate)) {
      return true
    }
  }
  return false
})

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

//breadcrumbs
const breadcrumbs = ref<IBreadCrumbsModel[]>([
  { label: 'Home', icon: 'home' },
  { label: 'Configuration', icon: 'settings' },
  { label: 'SMTP Setting', icon: 'mail_lock' }
])

onMounted(async () => {
  await getData()
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
        filled
        v-model="model.smtpServer"
        label="Smtp Server"
        lazy-rules
        :rules="stringRequired('Smtp Server')"
        dense
        maxlength="100"
        :readonly="!canCreateOrUpdate"
      />
      <q-input
        type="number"
        filled
        v-model="model.smtpPort"
        label="Smtp Port"
        lazy-rules
        :rules="numberRequired('Smtp Port')"
        dense
        :readonly="!canCreateOrUpdate"
      />
      <q-input
        type="text"
        filled
        v-model="model.smtpUser"
        label="Smtp User"
        lazy-rules
        :rules="numberRequired('Smtp User')"
        dense
        maxlength="100"
        :readonly="!canCreateOrUpdate"
      />
      <q-input
        v-model="model.smtpPassword"
        filled
        :type="isPwd ? 'password' : 'text'"
        label="Smtp Password"
        lazy-rules
        :rules="stringRequired('Smtp Password')"
        dense
        maxlength="100"
        :readonly="!canCreateOrUpdate"
      >
        <template v-slot:append>
          <q-icon
            :name="isPwd ? 'visibility_off' : 'visibility'"
            class="cursor-pointer"
            @click="isPwd = !isPwd"
          />
        </template>
      </q-input>
      <q-toggle
        v-model="model.smtpIsUseSsl"
        filled
        dense
        label="Smtp Is Use SSL"
        :disable="!canCreateOrUpdate"
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

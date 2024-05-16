<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type { IMyProfileModelUpdate } from '@/helpers/api/myProfile/myProfileModel'
import { MyProfileApi } from '@/helpers/api/myProfile/myProfileApi'

const quasar = useQuasar()
const myProfileApi = new MyProfileApi()

//createOrEdit
const model: Ref<IMyProfileModelUpdate> = ref({
  id: '',
  fullName: '',
  phoneNumber: ''
})
const formRef: Ref<QForm | null> = ref(null)
const getData = async () => {
  var response = await myProfileApi.get()
  model.value = response
}
const onSubmit = async () => {
  let isValid = await formFieldValidationHelper(formRef)
  if (isValid) {
    var response = await myProfileApi.update(model.value)
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
    <q-breadcrumbs-el label="My Profile" />
  </q-breadcrumbs>

  <!-- systemconfiguration -->
  <div class="q-pa-md" style="border: 1px solid #eeeeee">
    <q-form class="q-gutter-md" ref="formRef">
      <q-input
        type="text"
        v-model="model.fullName"
        label="FullName"
        maxlength="512"
        lazy-rules
        :rules="[(val) => (val && val.length > 0) || 'FullName is Required']"
        dense
        filled
      />
      <q-input
        type="text"
        v-model="model.phoneNumber"
        label="PhoneNumber"
        maxlength="512"
        lazy-rules
        :rules="[(val) => (val && val.length > 0) || 'PhoneNumber is Required']"
        dense
        filled
      />
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

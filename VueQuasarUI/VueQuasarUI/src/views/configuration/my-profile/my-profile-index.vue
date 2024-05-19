<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import type {
  IMyProfileModelUpdate,
  IMyProfileChangePassword
} from '@/helpers/api/myProfile/myProfileModel'
import { MyProfileApi } from '@/helpers/api/myProfile/myProfileApi'

const quasar = useQuasar()
const myProfileApi = new MyProfileApi()

const model: Ref<IMyProfileModelUpdate> = ref({
  id: '',
  fullName: '',
  phoneNumber: ''
})
const formRef: Ref<QForm | null> = ref(null)
const dialogChangePassword = ref()
const modelChangePassword: Ref<IMyProfileChangePassword> = ref({
  currentPassword: '',
  newPassword: '',
  confirmPassword: ''
})
const currentPasswordIsPwd = ref(true),
  newPasswordIsPwd = ref(true),
  confirmPasswordIsPwd = ref(true)
const modelChangePasswordRef: Ref<QForm | null> = ref(null)

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

const onChangePasswordClicked = async () => {
  dialogChangePassword.value = true
  modelChangePassword.value = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  }
}
const onSubmitChangePassword = async () => {
  let isValid = await formFieldValidationHelper(modelChangePasswordRef)
  if (isValid) {
    var response = await myProfileApi.changePassword(modelChangePassword.value)
    if (response) {
      quasar.notify({
        type: 'positive',
        message: response.message,
        position: 'bottom-right'
      })
      dialogChangePassword.value = false
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
  <div style="border: 1px solid rgb(238, 238, 238)" class="q-pa-md">
    <q-btn
      color="primary"
      icon="lock"
      label="Change Password"
      size="sm"
      @click="onChangePasswordClicked"
    />
  </div>
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
            size="sm"
            fixed-right
            @click="onSubmit"
          />
        </div>
      </div>
    </q-form>
  </div>

  <!-- dialog change password -->
  <q-dialog v-model="dialogChangePassword" backdrop-filter="blur(4px) saturate(150%)">
    <q-card style="width: 700px; max-width: 80vw">
      <q-card-section>
        <div class="text-h6">Change Password</div>
      </q-card-section>
      <q-form ref="modelChangePasswordRef" class="q-gutter-md">
        <q-card-section class="q-pt-none">
          <q-input
            v-model="modelChangePassword.currentPassword"
            filled
            :type="currentPasswordIsPwd ? 'password' : 'text'"
            label="Current Password"
            lazy-rules
            :rules="[(val) => (val && val.length > 0) || 'Current Password is Required']"
            dense
            maxlength="100"
          >
            <template v-slot:append>
              <q-icon
                :name="currentPasswordIsPwd ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="currentPasswordIsPwd = !currentPasswordIsPwd"
              />
            </template>
          </q-input>
          <q-input
            v-model="modelChangePassword.newPassword"
            :type="newPasswordIsPwd ? 'password' : 'text'"
            label="New Password"
            maxlength="100"
            lazy-rules
            :rules="[
              (val) => (val && val.length > 0) || 'New Password is required',
              (val) => val.length >= 8 || 'New Password must be a minimum of 8 characters',
              (val) => {
                let hasUppercase = false
                let hasLowercase = false
                let hasNumberOrSpecialChar = false
                for (let i = 0; i < val.length; i++) {
                  const char = val[i]
                  if (char >= 'A' && char <= 'Z') {
                    hasUppercase = true
                  } else if (char >= 'a' && char <= 'z') {
                    hasLowercase = true
                  } else if ((char >= '0' && char <= '9') || (char >= '!' && char <= '/')) {
                    hasNumberOrSpecialChar = true
                  }
                }
                if (!hasUppercase || !hasLowercase || !hasNumberOrSpecialChar) {
                  return 'New Password must contain 1 uppercase letter, 1 lowercase letter, and a number or symbol'
                }
                return true
              }
            ]"
            filled
            dense
          >
            <template v-slot:append>
              <q-icon
                :name="newPasswordIsPwd ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="newPasswordIsPwd = !newPasswordIsPwd"
              />
            </template>
          </q-input>
          <q-input
            v-model="modelChangePassword.confirmPassword"
            :type="confirmPasswordIsPwd ? 'password' : 'text'"
            label="Confirm Password"
            maxlength="100"
            lazy-rules
            :rules="[
              (val) => (val && val.length > 0) || 'Confirm Password is required',
              (val) => val.length >= 8 || 'Confirm Password must be a minimum of 8 characters',
              (val) => {
                let hasUppercase = false
                let hasLowercase = false
                let hasNumberOrSpecialChar = false
                for (let i = 0; i < val.length; i++) {
                  const char = val[i]
                  if (char >= 'A' && char <= 'Z') {
                    hasUppercase = true
                  } else if (char >= 'a' && char <= 'z') {
                    hasLowercase = true
                  } else if ((char >= '0' && char <= '9') || (char >= '!' && char <= '/')) {
                    hasNumberOrSpecialChar = true
                  }
                }
                if (!hasUppercase || !hasLowercase || !hasNumberOrSpecialChar) {
                  return 'Confirm Password must contain 1 uppercase letter, 1 lowercase letter, and a number or symbol'
                }
                return true
              },
              (val) =>
                val == modelChangePassword.newPassword ||
                'Confirm Password and New Password Should be same'
            ]"
            filled
            dense
          >
            <template v-slot:append>
              <q-icon
                :name="confirmPasswordIsPwd ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="confirmPasswordIsPwd = !confirmPasswordIsPwd"
              />
            </template>
          </q-input>
        </q-card-section>
        <q-card-actions align="right" class="bg-white text-teal">
          <q-btn flat label="Cancel" color="primary" v-close-popup />
          <q-btn flat label="Save" color="primary" :onclick="onSubmitChangePassword" />
        </q-card-actions>
      </q-form>
    </q-card>
  </q-dialog>
</template>

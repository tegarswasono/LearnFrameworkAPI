<script setup lang="ts">
import { ref, onBeforeMount, type Ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import axios, { Axios } from 'axios'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import { stringRequired, emailRequired, passwordRequired } from '@/helpers/rulesHelper'

const router = useRouter()
const route = useRoute()
const $q = useQuasar()
const isLoadingBtnResetPassword = ref(false)

const newPassword = ref()
const confirmPassword = ref()
const newPasswordIsPwd = ref(false)
const confirmPasswordIsPwd = ref(false)
const formRef: Ref<QForm | null> = ref(null)
const baseUrlApi = (<any>window).appSettings.api.base_url
const resetToken = route.query.resetToken

const checkIsValidToken = async () => {
  var model = {
    resetToken: resetToken
  }
  let headers = {
    headers: {
      'Content-Type': 'application/json'
    }
  }
  await axios
    .post(`${baseUrlApi}/Api/Common/Guest/IsValidResetToken`, model, headers)
    .then((res) => {})
    .catch((err) => {
      $q.notify({
        type: 'negative',
        message: err.response.data.message,
        position: 'bottom-right'
      })
      router.push({
        name: 'loginindex'
      })
    })
}

const onResetPasswordClicked = async () => {
  isLoadingBtnResetPassword.value = true
  let isValid = await formFieldValidationHelper(formRef)
  if (isValid) {
    let model = {
      resetToken: resetToken,
      newPassword: newPassword.value,
      confirmPassword: confirmPassword.value
    }
    let headers = {
      headers: {
        'Content-Type': 'application/json'
      }
    }
    let baseUrlApi = (<any>window).appSettings.api.base_url
    await axios
      .post(`${baseUrlApi}/Api/Common/Guest/ResetPassword`, model, headers)
      .then((res) => {
        $q.notify({
          type: 'positive',
          message: res.data.message,
          position: 'bottom-right'
        })
        router.push({
          name: 'loginindex'
        })
      })
      .catch((err) => {
        $q.notify({
          type: 'negative',
          message: err.response.data.message,
          position: 'bottom-right'
        })
      })
  }
  isLoadingBtnResetPassword.value = false
}

onBeforeMount(async () => {
  const token = localStorage.getItem('access_token')
  if (token != null) {
    router.push({
      name: 'bookingindex'
    })
  }
  //await checkIsValidToken()
})
</script>
<template>
  <q-layout>
    <q-page-container>
      <q-page class="flex flex-center bg-grey-2">
        <q-card class="q-pa-md shadow-24" bordered style="min-width: 400px; max-width: 70%">
          <q-card-section class="text-center">
            <div class="text-grey-9 text-h5 text-weight-bold">Registration Form</div>
            <div class="text-grey-8">
              Please fill out the form below to register. Make sure to provide accurate and complete
              information. Fields marked with an asterisk (*) are mandatory. Once you have completed
              the form, click the "Submit" button to finalize your registration. If you have any
              questions, please contact our support team.
            </div>
          </q-card-section>
          <q-card-section>
            <q-form ref="formRef">
              <q-input
                v-model="newPassword"
                :type="newPasswordIsPwd ? 'password' : 'text'"
                label="New Password"
                maxlength="100"
                lazy-rules
                :rules="passwordRequired('New Password')"
                filled
                dense
                class="q-mb-lg"
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
                v-model="confirmPassword"
                :type="confirmPasswordIsPwd ? 'password' : 'text'"
                label="Confirm Password"
                maxlength="100"
                lazy-rules
                :rules="[
                  ...passwordRequired('Confirm Password'),
                  (val) => val == newPassword || 'Confirm Password and New Password Should be same'
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
            </q-form>
          </q-card-section>
          <q-card-section>
            <q-btn
              style="border-radius: 8px"
              color="primary"
              rounded
              size="md"
              label="Reset Password"
              no-caps
              class="full-width"
              @click="onResetPasswordClicked"
              :loading="isLoadingBtnResetPassword"
            ></q-btn>
          </q-card-section>
          <q-card-section class="q-py-none">
            <q-btn
              flat
              size="md"
              label="Back to Login"
              no-caps
              class="full-width"
              icon="arrow_back"
              @click="router.push({ name: 'loginindex' })"
            ></q-btn>
          </q-card-section>
        </q-card>
      </q-page>
    </q-page-container>
  </q-layout>
</template>

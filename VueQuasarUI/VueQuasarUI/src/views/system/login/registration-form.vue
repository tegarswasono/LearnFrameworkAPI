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

const loading = ref(false)
const registrationToken = route.query.registrationToken
const email = ref()
const fullName = ref()
const phoneNumber = ref()
const password = ref()
const confirmPassword = ref()
const passwordIsPwd = ref(false)
const confirmPasswordIsPwd = ref(false)

const formRef: Ref<QForm | null> = ref(null)
const baseUrlApi = (<any>window).appSettings.api.base_url

const checkIsValidToken = async () => {
  var model = {
    registrationToken: registrationToken
  }
  let headers = {
    headers: {
      'Content-Type': 'application/json'
    }
  }
  await axios
    .post(`${baseUrlApi}/Api/Common/Guest/IsValidRegistrationForm`, model, headers)
    .then((res) => {
      email.value = res.data.email
    })
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

const onSubmitClicked = async () => {
  loading.value = true
  let isValid = await formFieldValidationHelper(formRef)
  if (isValid) {
    let model = {
      registrationToken: registrationToken,
      fullName: fullName.value,
      phoneNumber: phoneNumber.value,
      password: password.value,
      confirmPassword: confirmPassword.value
    }
    let headers = {
      headers: {
        'Content-Type': 'application/json'
      }
    }
    let baseUrlApi = (<any>window).appSettings.api.base_url
    await axios
      .post(`${baseUrlApi}/Api/Common/Guest/SubmitRegistrationForm`, model, headers)
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
  loading.value = false
}

onBeforeMount(async () => {
  const token = localStorage.getItem('access_token')
  if (token != null) {
    router.push({
      name: 'bookingindex'
    })
  }
  await checkIsValidToken()
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
            <q-form ref="formRef" class="q-gutter-md">
              <q-input
                type="email"
                v-model="email"
                label="Email"
                readonly
                filled
                dense
                style="padding-bottom: 10px"
              />
              <q-input
                type="text"
                v-model="fullName"
                label="FullName"
                filled
                dense
                maxlength="100"
                lazy-rules
                :rules="stringRequired('FullName')"
              />
              <q-input
                type="text"
                v-model="phoneNumber"
                label="PhoneNumber"
                filled
                dense
                maxlength="100"
                style="padding-bottom: 10px"
              />
              <q-input
                v-model="password"
                :type="passwordIsPwd ? 'password' : 'text'"
                label="Password"
                maxlength="100"
                lazy-rules
                :rules="passwordRequired('Password')"
                filled
                dense
              >
                <template v-slot:append>
                  <q-icon
                    :name="passwordIsPwd ? 'visibility_off' : 'visibility'"
                    class="cursor-pointer"
                    @click="passwordIsPwd = !passwordIsPwd"
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
                  (val) => val == password || 'Confirm Password and New Password Should be same'
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
              label="Submit"
              no-caps
              class="full-width"
              @click="onSubmitClicked"
              :loading="loading"
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

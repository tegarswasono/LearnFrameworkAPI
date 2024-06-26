<script setup lang="ts">
import { ref, onBeforeMount, type Ref } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import AuthService from '@/helpers/authService'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import { stringRequired, emailRequired } from '@/helpers/rulesHelper'

const router = useRouter()
const $q = useQuasar()
const auth = AuthService.getInstance()
const idsUrl = auth.getIdsUrl()

const username = ref()
const password = ref()
const isPwd = ref(true)
const formRef: Ref<QForm | null> = ref(null)
const isLoadingBtnLogin = ref(false)

const onLoginClicked = async () => {
  isLoadingBtnLogin.value = true
  let isValid = await formFieldValidationHelper(formRef)
  if (isValid) {
    let model = {
      grant_type: 'password',
      scope: 'offline_access',
      username: username.value,
      password: password.value
    }
    let headers = {
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      }
    }

    await axios
      .post(`${idsUrl}/connect/token`, model, headers)
      .then((res) => {
        localStorage.setItem('access_token', res.data.access_token)
        localStorage.setItem('refresh_token', res.data.refresh_token)

        router.push({
          name: 'bookingindex'
        })
      })
      .catch(() => {
        $q.notify({
          type: 'negative',
          message: 'Email Or Password is invalid',
          position: 'bottom-right'
        })
      })
  }
  isLoadingBtnLogin.value = false
}

const onForgotPasswordClicked = () => {
  router.push({
    name: 'forgotpassword'
  })
}

const onSignUpClicked = () => {
  router.push({ name: 'signUp' })
}

onBeforeMount(async () => {
  const token = localStorage.getItem('access_token')
  if (token != null) {
    router.push({
      name: 'bookingindex'
    })
  }
})
</script>
<template>
  <q-layout>
    <q-page-container>
      <q-page class="flex flex-center bg-grey-2">
        <q-card class="q-pa-md shadow-24" bordered style="min-width: 400px">
          <q-form ref="formRef">
            <q-card-section class="text-center">
              <div class="text-grey-9 text-h5 text-weight-bold">New Century - Back Office</div>
              <div class="text-grey-8">Sign in below to access your account</div>
            </q-card-section>
            <q-card-section>
              <q-input
                dense
                outlined
                v-model="username"
                label="Email"
                :rules="emailRequired('Email')"
              ></q-input>

              <q-input
                dense
                outlined
                class="q-mt-md"
                v-model="password"
                :type="isPwd ? 'password' : 'text'"
                label="Password"
                :rules="stringRequired('Password')"
              >
                <template v-slot:append>
                  <q-icon
                    :name="isPwd ? 'visibility_off' : 'visibility'"
                    class="cursor-pointer"
                    @click="isPwd = !isPwd"
                  />
                </template>
              </q-input>
            </q-card-section>
            <q-card-section class="q-py-none">
              <q-btn
                flat
                borderless
                size="md"
                label="Forgot Password?"
                no-caps
                class="full-width"
                @click="onForgotPasswordClicked"
              />
            </q-card-section>
            <q-card-section class="q-py-none">
              <q-btn
                flat
                borderless
                size="md"
                label="Don't have an account? Sign up"
                no-caps
                class="full-width"
                @click="onSignUpClicked"
              />
            </q-card-section>
            <q-card-section>
              <q-btn
                style="border-radius: 8px"
                color="primary"
                rounded
                size="md"
                label="Sign in"
                no-caps
                class="full-width"
                @click="onLoginClicked"
                :loading="isLoadingBtnLogin"
              />
            </q-card-section>
          </q-form>
        </q-card>
      </q-page>
    </q-page-container>
  </q-layout>
</template>

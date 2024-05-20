<script setup lang="ts">
import { ref, onBeforeMount, type Ref } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import AuthService from '@/helpers/authService'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'

const router = useRouter()
const $q = useQuasar()
const auth = AuthService.getInstance()
const idsUrl = auth.getIdsUrl()

const email = ref()
const formRef: Ref<QForm | null> = ref(null)

const onResetPasswordClicked = async () => {
  let isValid = await formFieldValidationHelper(formRef)
  if (isValid) {
    let model = {
      email: email.value
    }
    let headers = {
      headers: {
        'Content-Type': 'application/json'
      }
    }

    let tmp = (<any>window).appSettings.api.base_url
    //console.log(tmp);
    // axios
    //   .post(`${idsUrl}/connect/token`, model, headers)
    //   .then((res) => {
    //     localStorage.setItem('access_token', res.data.access_token)
    //     localStorage.setItem('refresh_token', res.data.refresh_token)
    //     router.push({
    //       name: 'bookingindex'
    //     })
    //   })
    //   .catch(() => {
    //     $q.notify({
    //       type: 'negative',
    //       message: 'Email Or Password is invalid',
    //       position: 'bottom-right'
    //     })
    //   })
  }
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
              <div class="text-grey-9 text-h5 text-weight-bold">Forgot Password</div>
              <div class="text-grey-8">
                Enter your email and we'll send you a link to reset your password
              </div>
            </q-card-section>
            <q-card-section>
              <q-input
                type="email"
                dense
                outlined
                v-model="email"
                label="Email"
                :rules="[
                  (val) => (val && val.length > 0) || 'Email is required',
                  (val, rules) => rules.email(val) || 'Please enter a valid email address'
                ]"
              ></q-input>
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
              ></q-btn>
            </q-card-section>
            <q-card-section class="q-py-none">
              <q-btn
                flat
                size="md"
                label="Forgot Password"
                no-caps
                class="full-width"
                icon="arrow_back"
                @click="router.push({ name: 'loginindex' })"
              ></q-btn>
            </q-card-section>
          </q-form>
        </q-card>
      </q-page>
    </q-page-container>
  </q-layout>
</template>

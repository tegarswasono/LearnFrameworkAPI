<script setup lang="ts">
import { ref, onBeforeMount, type Ref } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import { useQuasar, QForm } from 'quasar'
import formFieldValidationHelper from '@/helpers/formFieldValidationHelper'
import { emailRequired } from '@/helpers/rulesHelper'

const router = useRouter()
const $q = useQuasar()
const loading = ref(false)

const email = ref()
const formRef: Ref<QForm | null> = ref(null)

const onResetPasswordClicked = async () => {
  loading.value = true
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

    let baseUrlApi = (<any>window).appSettings.api.base_url
    await axios
      .post(`${baseUrlApi}/Api/Common/Guest/SendLinkSignUp`, model, headers)
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
})
</script>
<template>
  <q-layout>
    <q-page-container>
      <q-page class="flex flex-center bg-grey-2">
        <q-card class="q-pa-md shadow-24" bordered style="min-width: 400px">
          <q-form ref="formRef">
            <q-card-section class="text-center">
              <div class="text-grey-9 text-h5 text-weight-bold">Sign Up</div>
              <div class="text-grey-8">
                Enter your email and we'll send you a link to continue the registration
              </div>
            </q-card-section>
            <q-card-section>
              <q-input
                type="email"
                dense
                outlined
                v-model="email"
                label="Email"
                :rules="emailRequired('Email')"
              ></q-input>
            </q-card-section>
            <q-card-section>
              <q-btn
                style="border-radius: 8px"
                color="primary"
                rounded
                size="md"
                label="Sign Up"
                no-caps
                class="full-width"
                @click="onResetPasswordClicked"
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
          </q-form>
        </q-card>
      </q-page>
    </q-page-container>
  </q-layout>
</template>

<script setup>
import { ref, onBeforeMount } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import AuthService from '@/helpers/authService'

const router = useRouter()
const auth = AuthService.getInstance()
const idsUrl = auth.getIdsUrl()

const username = ref()
const password = ref()
const isPwd = ref(true)

onBeforeMount(async () => {
  const token = localStorage.getItem('access_token')
  if (token != null) {
    router.push({
      name: 'bookingindex'
    })
  }
})

function login() {
  let model = {
    grant_type: 'password',
    scope: 'offline_access',
    app: 'Administrator',
    username: username.value,
    password: password.value
  }

  let headers = {
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded'
    }
  }

  axios
    .post(`${idsUrl}/connect/token`, model, headers)
    .then((res) => {
      localStorage.setItem('access_token', res.data.access_token)
      localStorage.setItem('refresh_token', res.data.refresh_token)

      router.push({
        name: 'bookingindex'
      })
    })
    .catch((err) => {
      alert.value = true
    })
}
</script>
<template>
  <q-layout view="lHh Lpr lFf">
    <q-dialog v-model="alert">
      <q-card style="width: 400px">
        <q-card-section>
          <div class="text-h6">Login failed</div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          Please enter a correct username and password.
        </q-card-section>

        <q-card-actions align="right">
          <q-btn flat label="OK" color="primary" v-close-popup />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <q-page-container>
      <q-page class="flex flex-center bg-grey-2">
        <!-- <h2>Sign</h2> -->
        <q-card class="q-pa-md shadow-2" bordered>
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
              :rules="[
                (val) => (val && val.length > 0) || 'Email is required',
                (val, rules) => rules.email(val) || 'Please enter a valid email address'
              ]"
            ></q-input>

            <q-input
              dense
              outlined
              class="q-mt-md"
              v-model="password"
              :type="isPwd ? 'password' : 'text'"
              label="Password"
              :rules="[(val) => (val && val.length > 0) || 'Password is required']"
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
            ></q-btn>
          </q-card-section>

          <q-card-section>
            <q-btn
              :loading="isLoading"
              style="border-radius: 8px"
              color="primary"
              rounded
              size="md"
              label="Sign in"
              no-caps
              class="full-width"
              @click="login"
            ></q-btn>
          </q-card-section>
        </q-card>
      </q-page>
    </q-page-container>
  </q-layout>
</template>

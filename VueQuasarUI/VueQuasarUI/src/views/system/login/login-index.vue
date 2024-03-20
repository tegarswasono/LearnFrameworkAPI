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
            <div class="text-grey-9 text-h5 text-weight-bold">New Century - Back Office 99</div>
            <div class="text-grey-8">Sign in below to access your account</div>
          </q-card-section>
          <q-card-section>
            <q-input dense outlined v-model="username" label="Email"></q-input>
            <q-input
              dense
              outlined
              class="q-mt-md"
              v-model="password"
              type="password"
              label="Password"
            ></q-input>
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
              color="dark"
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

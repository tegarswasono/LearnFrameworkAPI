<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import { stringRequired, emailRequired, passwordRequired } from '@/helpers/rulesHelper'

const name = ref('')
const email = ref('')

const password = ref('')
const isPwdPassword = ref(true)
const confirmPassword = ref('')
const isPwdConfirmPassword = ref(true)

const number1 = ref()
const number2 = ref()

//date
</script>
<template>
  <!-- breadcrumbs -->
  <q-breadcrumbs style="margin-bottom: 30px">
    <q-breadcrumbs-el label="Home" icon="home" />
    <q-breadcrumbs-el label="Master" icon="assignment" />
    <q-breadcrumbs-el label="Example 1" />
  </q-breadcrumbs>
  <!--Content-->
  <div class="q-pa-md" style="border: 1px solid #eeeeee">
    <form class="q-gutter-md" ref="">
      <q-input
        type="text"
        v-model="name"
        label="Name"
        filled
        dense
        lazy-rules
        :rules="stringRequired('Name')"
      />
      <q-input
        type="email"
        v-model="email"
        label="Email"
        filled
        dense
        lazy-rules
        :rules="emailRequired('Email')"
      />
      <q-input
        v-model="password"
        label="Password"
        filled
        dense
        lazy-rules
        :rules="passwordRequired('Password')"
        :type="isPwdPassword ? 'password' : 'text'"
      >
        <template v-slot:append>
          <q-icon
            :name="isPwdPassword ? 'visibility_off' : 'visibility'"
            class="cursor-pointer"
            @click="isPwdPassword = !isPwdPassword"
          />
        </template>
      </q-input>
      <q-input
        v-model="confirmPassword"
        label="Confirm Password"
        filled
        dense
        lazy-rules
        :rules="[
          ...passwordRequired('Confirm Password'),
          (val) => val == password || 'Confirm Password and Password should be same'
        ]"
      >
        <template v-slot:append>
          <q-icon
            :name="isPwdConfirmPassword ? 'visibility_off' : 'visibility'"
            class="cursor-pointer"
            @click="isPwdConfirmPassword = !isPwdConfirmPassword"
          />
        </template>
      </q-input>
    </form>
  </div>
</template>

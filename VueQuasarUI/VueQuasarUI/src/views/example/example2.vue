<script setup lang="ts">
import { onMounted, ref, type Ref } from 'vue'
import {
  stringRequired,
  emailRequired,
  passwordRequired,
  numberRequired,
  numberShouldbeBiggerThanOrEqualsTo0,
  numberShouldbeBiggerThan0,
  selectRequired
} from '@/helpers/rulesHelper'

const name = ref('')
const email = ref('')

const password = ref('')
const isPwdPassword = ref(true)
const confirmPassword = ref('')
const isPwdConfirmPassword = ref(true)

const number1 = ref()
const number2 = ref()
const number3 = ref()

const food = ref()
const foodOptions = ref([
  { value: '6F45F3E5-19F4-4A91-95D2-98B0EEFF4840', label: 'Sate', description: 'Sate enak' },
  { value: 'D878A796-7527-49C3-9DF8-0722CC7520AC', label: 'Soto', description: 'Soto enak' },
  { value: '022407C4-2575-4C24-9E93-9DD61D527352', label: 'Bakso', description: 'Bakso enak' },
  {
    value: '1BCB7406-0885-47BF-872A-19287B0CF5D5',
    label: 'Nasi Goreng',
    description: 'Nasi Goreng enak'
  }
])
const select1 = ref()
const select2 = ref()

//date
</script>
<template>
  <!-- breadcrumbs -->
  <q-breadcrumbs style="margin-bottom: 30px">
    <q-breadcrumbs-el label="Home" icon="home" />
    <q-breadcrumbs-el label="Master" icon="assignment" />
    <q-breadcrumbs-el label="Example 1 - Rule Validation Example" />
  </q-breadcrumbs>
  <!--Content-->
  <div class="q-pa-md" style="border: 1px solid #eeeeee">
    <form class="q-gutter-md" ref="">
      <p class="text-weight-medium text-blue-grey-8">String input</p>
      <q-input
        type="text"
        v-model="name"
        label="Name"
        filled
        dense
        lazy-rules
        :rules="stringRequired('Name')"
        maxlength="15"
      />
      <q-input
        type="email"
        v-model="email"
        label="Email"
        filled
        dense
        lazy-rules
        :rules="emailRequired('Email')"
        maxlength="15"
      />
      <q-input
        v-model="password"
        label="Password"
        filled
        dense
        lazy-rules
        :rules="passwordRequired('Password')"
        :type="isPwdPassword ? 'password' : 'text'"
        maxlength="15"
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
        maxlength="15"
      >
        <template v-slot:append>
          <q-icon
            :name="isPwdConfirmPassword ? 'visibility_off' : 'visibility'"
            class="cursor-pointer"
            @click="isPwdConfirmPassword = !isPwdConfirmPassword"
          />
        </template>
      </q-input>
      <p class="text-weight-medium text-blue-grey-8">Number input</p>
      <q-input
        v-model="number1"
        label="Number 1"
        type="number"
        filled
        dense
        lazy-rules
        :rules="numberRequired('Number 1')"
      />
      <q-input
        v-model="number2"
        label="Number 2"
        type="number"
        filled
        dense
        lazy-rules
        :rules="numberShouldbeBiggerThanOrEqualsTo0('Number 2')"
      />
      <q-input
        v-model="number3"
        label="Number 3"
        type="number"
        filled
        dense
        lazy-rules
        :rules="numberShouldbeBiggerThan0('Number 3')"
      />
      <p class="text-weight-medium text-blue-grey-8">Select</p>
      <q-select
        v-model="food"
        :options="foodOptions"
        label="Food"
        filled
        dense
        emit-value
        map-options
        lazy-rules
        :rules="selectRequired('Food')"
        clearable
      />
      Food: {{ food }}
      <q-select v-model="select1" :options="foodOptions" label="Select 1" filled dense clearable />
      Select1: {{ select1 }}
      <q-select
        v-model="select2"
        :options="foodOptions"
        option-value="label"
        option-label="description"
        emit-value
        map-options
        label="Select 2"
        filled
        dense
        clearable
      />
      Select2: {{ select2 }}
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeMount } from 'vue'
import { useRouter } from 'vue-router'
import { AppFullscreen, useQuasar } from 'quasar'
import moment from 'moment'
import { MyProfileApi } from '@/helpers/api/myProfile/myProfileApi'

import backgroundProfilePicture from '@/assets/material.png'
import logo from '@/assets/New_Century_Tour_Logo.svg'
import defaultProfilePicture from '@/assets/avatar.webp'

const $q = useQuasar()
const router = useRouter()
const myProfileApi = new MyProfileApi()

const currentTime = ref()
const fullscreen = ref(false)
const drawer = ref(false)
const fullName = ref()
const profilePicture = ref()

const menus = ref()

const setCurrentTime = () => {
  currentTime.value = new Date()
}

const onFullScreenToggle = () => {
  if (fullscreen.value) {
    AppFullscreen.exit()
      .then(() => {
        // success!
      })
      .catch(() => {
        // oh, no!!!
      })
  } else {
    AppFullscreen.request()
      .then(() => {
        // success!
      })
      .catch(() => {
        // oh, no!!!
      })
  }
  fullscreen.value = !fullscreen.value
}

const onLogout = async () => {
  $q.dialog({
    title: '',
    message: 'Are you sure want to Logout?',
    cancel: true,
    color: 'primary'
  })
    .onOk(async () => {
      localStorage.clear()
      router.push({
        name: 'loginindex'
      })
    })
    .onCancel(() => {})
}

onBeforeMount(async () => {
  const token = localStorage.getItem('access_token')
  if (token == null) {
    router.push({
      name: 'loginindex'
    })
  }
})

onMounted(async () => {
  //myProfile
  let myProfile = await myProfileApi.get()
  fullName.value = myProfile.fullName
  if (myProfile.profilePicture) {
    let baseUrl = (<any>window).appSettings.api.base_url
    profilePicture.value = baseUrl + '/Upload/ProfilePicture/' + myProfile.profilePicture
  }

  //current time
  setInterval(setCurrentTime, 1000)
  setCurrentTime()

  menus.value = [
    {
      section: 'Transaction',
      child: [
        {
          title: 'Booking',
          icon: 'shopping_cart',
          url: 'bookingindex',
          child: []
        }
      ]
    },
    {
      section: 'Configuration',
      child: [
        {
          title: 'Master',
          icon: 'assignment',
          url: '',
          child: [
            {
              title: 'Product',
              icon: 'inventory',
              url: 'productindex',
              child: []
            },
            {
              title: 'Category',
              icon: 'category',
              url: 'categoryindex',
              child: []
            }
          ]
        },
        {
          title: 'My Profile',
          icon: 'person',
          url: 'myprofileindex',
          child: []
        }
      ]
    }
  ]
})
</script>
<template>
  <q-layout view="lHh Lpr lff" class="shadow-2 rounded-borders">
    <q-header elevated class="bg-cyan-8">
      <q-toolbar>
        <q-btn flat @click="drawer = !drawer" round dense icon="menu" />
        <q-toolbar-title style="text-align: center">
          <img :src="logo" style="margin-top: 10px" />
        </q-toolbar-title>
        <q-btn flat round dense icon="fullscreen" @click="onFullScreenToggle()" />
      </q-toolbar>
    </q-header>
    <q-footer class="bg-grey-2" style="color: grey">
      <div style="margin: 15px; font-size: small">
        <div class="left">
          Made with <i class="fa fa-heart" style="color: red"></i> in Indonesia. By
          <a
            style="text-decoration: none; font-weight: bold; color: grey"
            target="_blank"
            href="https://www.linkedin.com/in/tegar-swasono/"
            title="Tegar Swasono"
            >Tegar Swasono</a
          >
          <div class="float-right"><b>Version</b> 1.0</div>
        </div>
      </div>
    </q-footer>

    <q-drawer v-model="drawer" show-if-above :width="250" :breakpoint="400">
      <!-- <q-scroll-area
        style="height: calc(100% - 150px); margin-top: 150px; border-right: 1px solid #ddd"
      >
        <q-list padding style="font-size:">
          <q-item-label header>Transaction</q-item-label>
          <q-item clickable v-ripple :to="{ name: 'bookingindex' }">
            <q-item-section avatar>
              <q-icon name="shopping_cart" />
            </q-item-section>
            <q-item-section style="font-size:"> Booking</q-item-section>
          </q-item>
          <q-item-label header>Configuration</q-item-label>
          <q-expansion-item icon="assignment" label="Master" :content-inset-level="0.3">
            <q-item clickable v-ripple :to="{ name: 'productindex' }">
              <q-item-section avatar>
                <q-icon name="inventory" />
              </q-item-section>
              <q-item-section> Product </q-item-section>
            </q-item>
            <q-item clickable v-ripple :to="{ name: 'categoryindex' }">
              <q-item-section avatar>
                <q-icon name="category" />
              </q-item-section>
              <q-item-section> Category </q-item-section>
            </q-item>
          </q-expansion-item>
          <q-expansion-item icon="settings" label="Configuration" :content-inset-level="0.3">
            <q-item clickable v-ripple :to="{ name: 'userindex' }">
              <q-item-section avatar>
                <q-icon name="group" />
              </q-item-section>
              <q-item-section> User </q-item-section>
            </q-item>
            <q-item clickable v-ripple :to="{ name: 'roleindex' }">
              <q-item-section avatar>
                <q-icon name="key" />
              </q-item-section>
              <q-item-section> Role </q-item-section>
            </q-item>
            <q-item clickable v-ripple :to="{ name: 'systemconfigurationindex' }">
              <q-item-section avatar>
                <q-icon name="manufacturing" />
              </q-item-section>
              <q-item-section> System Configuration </q-item-section>
            </q-item>
            <q-item clickable v-ripple :to="{ name: 'smtpsettingindex' }">
              <q-item-section avatar>
                <q-icon name="mail_lock" />
              </q-item-section>
              <q-item-section> SMTP Setting </q-item-section>
            </q-item>
          </q-expansion-item>
          <q-item clickable v-ripple :to="{ name: 'myprofileindex' }">
            <q-item-section avatar>
              <q-icon name="person" />
            </q-item-section>
            <q-item-section style="font-size:"> My Profile</q-item-section>
          </q-item>
        </q-list>
      </q-scroll-area> -->
      <q-scroll-area
        style="height: calc(100% - 150px); margin-top: 150px; border-right: 1px solid #ddd"
      >
        <q-list padding style="font-size:">
          <template v-for="menu in menus">
            <q-item-label header>{{ menu.section }}</q-item-label>
            <template v-for="child1 in menu.child">
              <q-item clickable v-ripple :to="{ name: child1.url }" v-if="child1.child.length == 0">
                <q-item-section avatar>
                  <q-icon :name="child1.icon" />
                </q-item-section>
                <q-item-section style="font-size:"> {{ child1.title }}</q-item-section>
              </q-item>
              <q-expansion-item
                icon="assignment"
                label="Master"
                :content-inset-level="0.3"
                v-if="child1.child.length > 0"
              >
                <!--child rendering-->
                <template v-for="child2 in child1.child">
                  <q-item
                    clickable
                    v-ripple
                    :to="{ name: child2.url }"
                    v-if="child2.child.length == 0"
                  >
                    <q-item-section avatar>
                      <q-icon :name="child2.icon" />
                    </q-item-section>
                    <q-item-section style="font-size:"> {{ child2.title }}</q-item-section>
                    <!-- If you need subchild render here -->
                  </q-item>
                </template>
              </q-expansion-item>
            </template>
          </template>
          <q-item-label header>Transaction Batas</q-item-label>
          <q-item-label header>Transaction</q-item-label>
          <q-item clickable v-ripple :to="{ name: 'bookingindex' }">
            <q-item-section avatar>
              <q-icon name="shopping_cart" />
            </q-item-section>
            <q-item-section style="font-size:"> Booking</q-item-section>
          </q-item>

          <q-item-label header>Configuration</q-item-label>
          <q-expansion-item icon="assignment" label="Master" :content-inset-level="0.3">
            <q-item clickable v-ripple :to="{ name: 'productindex' }">
              <q-item-section avatar>
                <q-icon name="inventory" />
              </q-item-section>
              <q-item-section> Product </q-item-section>
            </q-item>
            <q-item clickable v-ripple :to="{ name: 'categoryindex' }">
              <q-item-section avatar>
                <q-icon name="category" />
              </q-item-section>
              <q-item-section> Category </q-item-section>
            </q-item>
          </q-expansion-item>
          <q-expansion-item icon="settings" label="Configuration" :content-inset-level="0.3">
            <q-item clickable v-ripple :to="{ name: 'userindex' }">
              <q-item-section avatar>
                <q-icon name="group" />
              </q-item-section>
              <q-item-section> User </q-item-section>
            </q-item>
            <q-item clickable v-ripple :to="{ name: 'roleindex' }">
              <q-item-section avatar>
                <q-icon name="key" />
              </q-item-section>
              <q-item-section> Role </q-item-section>
            </q-item>
            <q-item clickable v-ripple :to="{ name: 'systemconfigurationindex' }">
              <q-item-section avatar>
                <q-icon name="manufacturing" />
              </q-item-section>
              <q-item-section> System Configuration </q-item-section>
            </q-item>
            <q-item clickable v-ripple :to="{ name: 'smtpsettingindex' }">
              <q-item-section avatar>
                <q-icon name="mail_lock" />
              </q-item-section>
              <q-item-section> SMTP Setting </q-item-section>
            </q-item>
          </q-expansion-item>
          <q-item clickable v-ripple :to="{ name: 'myprofileindex' }">
            <q-item-section avatar>
              <q-icon name="person" />
            </q-item-section>
            <q-item-section style="font-size:"> My Profile</q-item-section>
          </q-item>
        </q-list>
      </q-scroll-area>

      <q-img class="absolute-top" :src="backgroundProfilePicture" style="height: 150px">
        <div class="absolute-bottom bg-transparent">
          <q-avatar size="56px" class="q-mb-sm">
            <img v-if="profilePicture" :src="profilePicture" />
            <img v-else :src="defaultProfilePicture" />
          </q-avatar>
          <div class="text-weight-bold" style="font-size: larger">{{ fullName }}</div>
          <div style="font-size: x-small">
            {{ moment(currentTime).format('DD-MM-YYYY HH:mm:ss') }}
          </div>
          <div style="font-size: smaller; margin-top: 5px">
            <div style="color: white; cursor: pointer" @click="onLogout">
              <q-icon name="logout" /> Logout
            </div>
          </div>
        </div>
      </q-img>
    </q-drawer>

    <q-page-container>
      <q-page padding>
        <router-view></router-view>
      </q-page>
    </q-page-container>
  </q-layout>
</template>
<style>
.textarea-no-resize textarea {
  resize: none !important; /* Menonaktifkan resize */
}
</style>

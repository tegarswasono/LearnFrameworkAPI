<template>
  <q-layout view="lHh Lpr lff" class="shadow-2 rounded-borders">
    <q-header elevated class="bg-cyan-8">
      <q-toolbar>
        <q-btn flat @click="drawer = !drawer" round dense icon="menu" />
        <q-toolbar-title style="text-align: center">
          <!-- Learn Framework API -->
          <img src="../assets/New_Century_Tour_Logo.svg" />
        </q-toolbar-title>
        <q-btn flat round dense icon="fullscreen" @click="fullScreenToggle()" />
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
      <q-scroll-area
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
      </q-scroll-area>

      <q-img
        class="absolute-top"
        src="https://cdn.quasar.dev/img/material.png"
        style="height: 150px"
      >
        <div class="absolute-bottom bg-transparent">
          <q-avatar size="56px" class="q-mb-sm">
            <!-- <img src="https://cdn.quasar.dev/img/boy-avatar.png" /> -->
            <img src="../assets/avatar.webp" />
          </q-avatar>
          <div class="text-weight-bold" style="font-size: larger">Razvan Stoenescu</div>
          <div style="font-size: x-small">
            {{ moment(currentTime).format('DD-MM-YYYY HH:mm:ss') }}
          </div>
          <!-- <div style="font-size: x-small">17-03-2024 12:19</div> -->
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
        <!-- <p v-for="n in 15" :key="n">
          Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit nihil praesentium molestias
          a adipisci, dolore vitae odit, quidem consequatur optio voluptates asperiores pariatur eos
          numquam rerum delectus commodi perferendis voluptate?
        </p> -->
      </q-page>
    </q-page-container>
  </q-layout>
</template>

<script setup>
import { computed, ref, onMounted, onBeforeMount } from 'vue'
import { useRouter } from 'vue-router'
import { AppFullscreen, useQuasar } from 'quasar'
import moment from 'moment'

const $q = useQuasar()
const router = useRouter()

const fullscreen = ref(false)
const drawer = ref(false)
const currentTime = ref('')

const updateTime = () => {
  currentTime.value = new Date()
}

onMounted(() => {
  setInterval(updateTime, 1000)
  updateTime()
})

function fullScreenToggle() {
  if (this.fullscreen) {
    AppFullscreen.exit()
      .then(() => {
        // success!
      })
      .catch((err) => {
        // oh, no!!!
      })
  } else {
    AppFullscreen.request()
      .then(() => {
        // success!
      })
      .catch((err) => {
        // oh, no!!!
      })
  }
  this.fullscreen = !this.fullscreen
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
</script>
<style>
.textarea-no-resize textarea {
  resize: none !important; /* Menonaktifkan resize */
}
</style>

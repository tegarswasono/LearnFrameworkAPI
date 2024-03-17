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
          <q-item clickable v-ripple>
            <q-item-section avatar>
              <q-icon name="inbox" />
            </q-item-section>

            <q-item-section> Inbox </q-item-section>
          </q-item>

          <q-item active clickable v-ripple>
            <q-item-section avatar>
              <q-icon name="star" />
            </q-item-section>

            <q-item-section style="font-size:"> Start</q-item-section>
          </q-item>

          <q-item clickable v-ripple>
            <q-item-section avatar>
              <q-icon name="send" />
            </q-item-section>

            <q-item-section> Send </q-item-section>
          </q-item>

          <q-item clickable v-ripple>
            <q-item-section avatar>
              <q-icon name="drafts" />
            </q-item-section>

            <q-item-section> Drafts </q-item-section>
          </q-item>

          <q-item-label header>Configuration</q-item-label>
          <q-item clickable v-ripple>
            <q-item-section avatar>
              <q-icon name="assignment" />
            </q-item-section>

            <q-item-section> Master </q-item-section>
          </q-item>
          <q-item clickable v-ripple>
            <q-item-section avatar>
              <q-icon name="settings" />
            </q-item-section>

            <q-item-section> Configuration </q-item-section>
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
          <div style="font-size: x-small">17-03-2024 12:19</div>
          <div style="font-size: smaller; margin-top: 5px">
            <a href="#" style="color: white; text-decoration: none">MyProfile</a> |
            <a href="#" style="color: white; text-decoration: none">Logout</a>
          </div>
        </div>
      </q-img>
    </q-drawer>

    <q-page-container>
      <q-page padding>
        <router-view></router-view>
        <p v-for="n in 15" :key="n">
          Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit nihil praesentium molestias
          a adipisci, dolore vitae odit, quidem consequatur optio voluptates asperiores pariatur eos
          numquam rerum delectus commodi perferendis voluptate?
        </p>
      </q-page>
    </q-page-container>
  </q-layout>
</template>

<script setup>
import { ref } from 'vue'
import { AppFullscreen } from 'quasar'

const drawer = ref(false)
const fullscreen = ref(false)

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
</script>

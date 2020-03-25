<template>
  <v-container fluid>
    <v-toolbar color="indigo" dark>
      <v-app-bar-nav-icon></v-app-bar-nav-icon>

      <v-toolbar-title>Discover</v-toolbar-title>

      <v-spacer></v-spacer>

      <v-btn icon>
        <v-icon>mdi-magnify</v-icon>
      </v-btn>
    </v-toolbar>

    <v-container fluid>
      <v-row dense>
        <v-col>
          <v-card color="#ededed">
            <v-img :src="cultures.src" class="white--text align-end" height="200px">
              <v-card-title v-text="cultures.title"></v-card-title>
              <div
                class="question"
                v-for="(item, index) in cultures.data"
                :key="item.id"
                v-on:click="updateQuestions(item.name)"
              >
                {{index + 1}}.
                {{item.name}}
              </div>
            </v-img>
          </v-card>
          <v-card color="#ededed">
            <v-img :src="questions.src" class="white--text align-end" height="200px">
              <v-card-title v-text="questions.title"></v-card-title>
              <div
                class="question"
                v-for="(item, index) in questions.data"
                :key="item.id"
                v-on:click="updateResponse(item.answer)"
              >
                {{index + 1}}.
                {{item.value}}
              </div>
            </v-img>
          </v-card>
          <v-card color="#ededed" height="500px">
            <v-img :src="responses.src" class="white--text align-end">
              <v-card-title v-text="responses.title"></v-card-title>
              <div class="answer">{{responses.data}}
              <youtube :video-id="O7dq_wo5eto" :player-vars="{autoplay: 1}"></youtube>

              </div>
            </v-img>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </v-container>
</template>

<script>
import axios from "axios";
export default {
  el: "#app",
  data: () => ({
    cultures: {
      title: "Supported Culture",
      src: "https://cdn.vuetifyjs.com/",
      flex: 4,
      data: []
    },
    questions: {
      title: "List of Questions",
      src: "https://cdn.vuetifyjs.com/",
      flex: 8,
      data: []
    },
    responses: {
      title: "Video Responses",
      src: "https://cdn.vuetifyjs.com/",
      flex: 12,
      data: ""
    },
    test: "test"
  }),
  methods: {
    updateQuestions(culture) {
      axios
        .get("http://192.168.0.117:3000/questions?culture=" + culture)
        .then(response => {
          this.questions.data = response.data;
          if (response.data[0]) {
            this.updateResponse(response.data[0].answer)
          }
          else {
            this.responses.data = ""
          }
        });
    },
    updateResponse(answer) {
      console.log(answer);
      this.responses.data = answer;
    }
  },
  beforeMount() {
    axios.get("http://192.168.0.117:3000/cultures").then(response => {
      this.cultures.data = response.data;
      this.questions.data = this.updateQuestions(response.data[0].name)
    });
  }
};
</script>

<style lang="scss" scoped>
title {
  margin: 0;
  color: red;
}

.question {
  cursor: pointer;
  margin: 10px;
}

.answer {
  margin: 10px;
}
</style>
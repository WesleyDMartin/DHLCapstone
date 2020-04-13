<template>
  <v-container fluid class="pa-n4">
    <v-toolbar color="#333333" dark height="41px">
      <v-toolbar-title>Admin Dashboard</v-toolbar-title>

      <v-spacer></v-spacer>
      <v-toolbar-side-icon>
        <v-img class="mr-3" src="../assets/dhl-logo.png" height="100%" width="50px"></v-img>
      </v-toolbar-side-icon>
    </v-toolbar>

    <v-container fluid>
      <v-row>
        <v-col>
          <v-card :color="color">
            <v-card-title v-text="cultures.title"></v-card-title>
            <v-card-text>
              <v-row
                class="mb-4 question"
                justify="center"
                no-gutters
                v-for="(item) in paginatedDataCulture"
                :key="item.id"
              >
                <p>
                  <img
                    width="100"
                    height="140"
                    class="delete"
                    v-on:click="deleteCulture(item.id)"
                    :src="require('../assets/delete.png')"
                  />
                  <span class="title"
                    v-on:click="updateQuestions(item.name)"
                    v-on:hover="hoverCulture(item.name)"
                  >{{item.name}}</span>
                </p>
              </v-row>
              <v-row>
                <v-col align="center">
                  <button v-on:click="prevPageCulture" :disabled="pageNumberCulture==0">Previous</button>
                  <span>- {{pageNumberCulture+1}} -</span>
                  <button
                    v-on:click="nextPageCulture"
                    :disabled="pageNumberCulture >= pageCountCulture - 1"
                  >Next</button>
                </v-col>
              </v-row>
              <v-form v-on:submit.prevent="onSubmitCulture">
                    <v-text-field ref="culturename" width="25%" placeholder="Add New Culture"></v-text-field>
                    <button>Add Culture</button>
              </v-form>
            </v-card-text>
          </v-card>
        </v-col>
        <v-col>
          <v-card :color="color">
            <v-card-title v-text="questions.title"></v-card-title>
            <v-card-text>
              <v-row class="mb-4 question" v-for="(item) in paginatedDataQuestion" :key="item.id">
                <p>
                  <img
                    class="delete"
                    v-on:click="deleteQuestion(item.id)"
                    :src="require('../assets/delete.png')"
                  />
                  <span v-on:click="updateResponse(item)">{{item.value}}</span>
                </p>
              </v-row>
              <v-row>
                <v-col align="center">
                  <button v-on:click="prevPageQuestion" :disabled="pageNumberQuestion==0">Previous</button>
                  <span>- {{pageNumberQuestion+1}} -</span>
                  <button
                    v-on:click="nextPageQuestion"
                    :disabled="pageNumberQuestion >= pageCountQuestion - 1"
                  >Next</button>
                </v-col>
              </v-row>
              <v-form v-on:submit.prevent="onSubmitQuestion">
                <v-text-field ref="question" width="25%" placeholder="Question"></v-text-field>
                <v-text-field ref="text_answer" width="25%" placeholder="Text Answer"></v-text-field>
                <v-text-field ref="answerurl" width="25%" placeholder="Video URL"></v-text-field>
                <v-card-text>
                  What type of youtube video is this?
                  <br />
                  <v-radio-group v-model="videotype" :mandatory="true">
                    <v-radio value="standard" label="Standard" />
                    <br />
                    <v-radio value="threesixty" label="360" />
                  </v-radio-group>
                </v-card-text>
                <button>Add Question</button>
              </v-form>
            </v-card-text>
          </v-card>
        </v-col>
        <v-col>
          <v-card :color="color">
            <v-card-title v-text="responses.title + responses.data.question"></v-card-title>
              <v-card-text>Response: {{responses.data.text}}</v-card-text>
              <v-card-text>Type: {{responses.data.videotype}}</v-card-text>
              <v-card-text align="center">
                <iframe
                  width="490"
                  height="275"
                  :src="responses.data.url"
                  frameborder="0"
                  allowfullscreen
                ></iframe>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </v-container>
</template>

<script>
import axios from "axios";
export default {
  props: {
    size: {
      type: Number,
      required: false,
      default: 5
    }
  },
  data: () => ({
    cultures: {
      title: "Supported Culture",
      src: "https://cdn.vuetifyjs.com/",
      flex: 4,
      data: []
    },
    pageNumberCulture: 0,
    pageNumberQuestion: 0,
    questions: {
      title: "List of Questions",
      src: "https://cdn.vuetifyjs.com/",
      flex: 8,
      data: []
    },
    responses: {
      title: "Answer For: ",
      src: "https://cdn.vuetifyjs.com/",
      flex: 12,
      data: {}
    },
    currentCulture: "",
    host: "http://138.197.146.4:3000/",
    videotype: "",
    color: "#FFFFFF"
  }),
  methods: {
    hoverCulture(culture) {
      console.log(culture);
    },
    updateCultures(setCulture) {
      axios.get(this.host + "cultures").then(response => {
        this.cultures.data = response.data;
        if (this.cultures.data[0]) {
          if (setCulture == 0) {
            this.updateQuestions(this.cultures.data[0].name);
          } else {
            this.updateQuestions(this.cultures.data.slice(-1)[0].name);
            this.pageNumberCulture = Math.max(
              Math.ceil(this.cultures.data.length / 5) - 1,
              0
            );
          }
        } else {
          this.questions.data = [];
          this.questions.title = "There Are Currently No Supported Cultures";
        }
      });
    },
    updateQuestions(culture, questionId) {
      axios.get(this.host + "questions?culture=" + culture).then(response => {
        this.questions.data = response.data;
        this.questions.title =
          "Supported Questions for the " + culture + " Culture";
        this.currentCulture = culture;
        if (response.data[0]) {
          if (questionId == 0) {
            this.updateResponse(response.data[0]);
          } else {
            this.updateResponse(response.data.slice(-1)[0]);
            this.pageNumberQuestion = Math.max(
              Math.ceil(this.questions.data.length / 5) - 1,
              0
            );
          }
        } else {
          this.responses.data = {};
        }
      });
    },
    deleteCulture(culture) {
      axios.delete(this.host + "cultures/" + culture).then(() => {
        if (
          this.cultures.data.length % 5 == 1 &&
          this.pageNumberCulture ==
            Math.max(Math.ceil(this.cultures.data.length / 5) - 1, 0)
        ) {
          this.prevPageCulture();
        }
        this.updateCultures(0);
      });
    },
    deleteQuestion(question) {
      axios.delete(this.host + "questions/" + question).then(() => {
        if (
          this.questions.data.length % 5 == 1 &&
          this.pageNumberQuestion ==
            Math.max(Math.ceil(this.questions.data.length / 5) - 1, 0)
        ) {
          this.prevPageQuestion();
        }
        this.updateQuestions(this.currentCulture, 0);
      });
    },
    updateResponse(answer) {
      this.responses.data = {
        question: answer.value,
        url: answer.answer.replace("watch?v=", "embed/"),
        text: answer.text_answer,
        videotype: answer.videotype == "standard" ? "2-D Movie" : "360 Movie"
      };
    },
    onSubmitCulture() {
      if (this.$refs.culturename.internalValue != "") {
        axios
          .post(this.host + "cultures", {
            culture: {
              name: this.$refs.culturename.internalValue
            }
          })
          .then(() => {
            this.updateCultures();
          });
      }
      this.$refs.culturename.internalValue = "";
    },
    onSubmitQuestion() {
      if (this.currentCulture != "") {
        axios
          .post(this.host + "questions", {
            question: {
              value: this.$refs.question.internalValue,
              answer: this.$refs.answerurl.internalValue,
              text_answer: this.$refs.text_answer.internalValue,
              videotype: this.videotype,
              culture: this.currentCulture
            }
          })
          .then(() => {
            this.updateQuestions(this.currentCulture);
          });
      }
      this.$refs.question.internalValue = "";
      this.$refs.text_answer.internalValue = "";
      this.$refs.answerurl.internalValue = "";
    },
    nextPageCulture() {
      this.pageNumberCulture++;
    },
    prevPageCulture() {
      this.pageNumberCulture--;
    },
    nextPageQuestion() {
      this.pageNumberQuestion++;
    },
    prevPageQuestion() {
      this.pageNumberQuestion--;
    }
  },
  computed: {
    pageCountCulture() {
      let l = this.cultures.data.length,
        s = this.size;
      return Math.ceil(l / s);
    },
    paginatedDataCulture() {
      const start = this.pageNumberCulture * this.size,
        end = start + this.size;

      return this.cultures.data.slice(start, end);
    },
    pageCountQuestion() {
      let l = 0;
      if (this.questions.data != undefined) {
        l = this.questions.data.length;
      }
      let s = this.size;
      return Math.ceil(l / s);
    },
    paginatedDataQuestion() {
      const start = this.pageNumberQuestion * this.size,
        end = start + this.size;

      // I don't know how this gets set to undefined sometimes.
      if (this.questions.data == undefined) {
        return [];
      }

      return this.questions.data.slice(start, end);
    }
  },
  beforeMount() {
    this.updateCultures(0);
  }
};
</script>

<style lang="scss" scoped>
img {
  float: left;
  margin: 0px 10px 20px 0px;
}
title {
  color: red;
}
.v-card__text{
    font-size: 1.1em;
}

.question {
  cursor: pointer;
  margin: 10px;
}

.answer {
  margin: 10px;
}

.delete {
  height: 20px;
  width: 20px;
}
html,
body {
  background-color: rgb(250, 28, 65);
}

button {
  width: 8em;
  border-radius: 6px;
  margin: 10px;
  background-color: rgb(241, 152, 51);
  color: #fff;
  padding: 10px 10px;
  cursor: pointer;
  transition: transform 0.1s ease-in;

  &:active {
    transform: scale(0.9);
  }

  &:focus {
    outline: none;
  }
}
</style>
<template>
  <v-container fluid>
    <v-toolbar color="indigo" dark>
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
            <v-card-title v-text="cultures.title"></v-card-title>
            <div class="question" v-for="(item, index) in paginatedDataCulture" :key="item.id">
                      <div v-on:click="updateQuestions(item.name)">{{index+1}}. {{item.name}}</div>
                      <img class="delete" v-on:click="deleteCulture(item.id)" :src="require('../assets/delete.png')" />
            </div>
            <button v-on:click="prevPageCulture" :disabled="pageNumberCulture==0">Previous</button>
              &nbsp;&nbsp;
            <button v-on:click="nextPageCulture" :disabled="pageNumberCulture >= pageCountCulture - 1">Next</button>
              
            <form v-on:submit.prevent="onSubmitCulture">
              <v-text-field ref="culturename" width="25%" placeholder="Add New Culture"></v-text-field>
              <button>Add Culture</button>
            </form>
          </v-card>
          <v-card color="#ededed">
            <v-card-title v-text="questions.title"></v-card-title>
            
            <!-- <div class="question" v-for="(item, index) in questions.data" :key="item.id">
              <div v-on:click="updateResponse(item)">
                {{index + 1}}.
                {{item.value}}
              </div>
              <img
                class="delete"
                v-on:click="deleteQuestion(item.id)"
                :src="require('../assets/delete.png')"
              />
            </div> -->
            <div class="question" v-for="(item, index) in paginatedDataQuestion" :key="item.id">
                      <div v-on:click="updateResponse(item)">{{index+1}}. {{item.value}}</div>
                      <img class="delete" v-on:click="deleteQuestion(item.id)" :src="require('../assets/delete.png')" />
            </div>
            <button v-on:click="prevPageQuestion" :disabled="pageNumberQuestion==0">Previous</button>
              &nbsp;&nbsp;
            <button v-on:click="nextPageQuestion" :disabled="pageNumberQuestion >= pageCountQuestion - 1">Next</button>

            <form v-on:submit.prevent="onSubmitQuestion">
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
                  <br />
                </v-radio-group>
              </v-card-text>
              <button>Add Question</button>
            </form>
          </v-card>
          <v-card color="#ededed">
            <v-card-title v-text="responses.title"></v-card-title>
            <v-card-text>Response: {{responses.data.text}}</v-card-text>
            <iframe
              width="490"
              height="275"
              :src="responses.data.url"
              frameborder="0"
              allowfullscreen
            ></iframe>
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
    size:{
      type: Number,
      required:false,
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
      title: "Answer",
      src: "https://cdn.vuetifyjs.com/",
      flex: 12,
      data: {}
    },
    currentCulture: "",
    host: "http://138.197.146.4:3000/",
    videotype: ""
  }),
  methods: {
    updateCultures() {
      axios.get(this.host + "cultures").then(response => {
        this.cultures.data = response.data;
        if (this.cultures.data[0]) {
          this.updateQuestions(this.cultures.data[0].name);
        } else {
          this.questions.data = [];
        }
      });
    },
    deleteCulture(culture) {
      axios.delete(this.host + "cultures/" + culture).then(() => {
        this.updateCultures();
      });
    },
    deleteQuestion(question) {
      axios.delete(this.host + "questions/" + question).then(() => {
        this.updateQuestions(this.currentCulture);
      });
    },
    updateQuestions(culture) {
      axios.get(this.host + "questions?culture=" + culture).then(response => {
        this.questions.data = response.data;
        this.questions.title =
          "Supported Questions for the " + culture + " Culture";
        this.currentCulture = culture;
        if (response.data[0]) {
          this.updateResponse(response.data[0]);
        } else {
          this.responses.data = {};
        }
      });
    },
    updateResponse(answer) {
      this.responses.data = {
        url: answer.answer.replace("watch?v=", "embed/"),
        text: answer.text_answer
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
      if (this.$refs.culturename.internalValue != "") {
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
      this.$refs.question.internalValue="";
      this.$refs.text_answer.internalValue="";
      this.$refs.answerurl.internalValue="";
    },
    nextPageCulture(){
    this.pageNumberCulture++;
    },
    prevPageCulture(){
      this.pageNumberCulture--;
    },
    nextPageQuestion(){
    this.pageNumberQuestion++;
    },
    prevPageQuestion(){
      this.pageNumberQuestion--;
    },
  },
  computed: {
    pageCountCulture(){
      let l = this.cultures.data.length,
      s= this.size;
      return Math.ceil(l/s);
    },
    paginatedDataCulture(){
      const start = this.pageNumberCulture * this.size,
      end = start + this.size;

      return this.cultures.data.slice(start, end);
    },
    pageCountQuestion(){
      let l = 0;
      if (this.questions.data != undefined)
      {
        l = this.questions.data.length;
      }
      let s = this.size;
      return Math.ceil(l/s);
    },
    paginatedDataQuestion(){
      const start = this.pageNumberQuestion * this.size,
      end = start + this.size;

      // I don't know how this gets set to undefined sometimes.
      if (this.questions.data == undefined)
      {
        return [];
      }

      return this.questions.data.slice(start, end);
    },
  },
  beforeMount() {
    axios.get(this.host + "cultures").then(response => {
      this.cultures.data = response.data;
      this.questions.data = this.updateQuestions(response.data[0].name);
      this.currentCulture = response.data[0].name;
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

.delete {
  height: 40px;
  width: 40px;
}


button {
  border-radius: 6px;
  background-color: rgb(241, 143, 51);
  color: #fff;
  padding: 10px 40px;
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
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
            <v-img :src="cultures.src" class="white--text align-end" width="25%">
              <v-card-title v-text="cultures.title"></v-card-title>
            </v-img>
              <div
                class="question"
                v-for="(item, index) in cultures.data"
                :key="item.id"
                v-on:click="updateQuestions(item.name)"
              >{{index + 1}}. {{item.name}}</div>
            <form v-on:submit.prevent="onSubmit">
              <v-text-field ref="culturename" width="25%"></v-text-field>
              <button>Submit!</button>
            </form>

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
          <v-card color="#ededed">
            <v-img :src="responses.src" class="white--text align-end">
              <v-card-title v-text="responses.title"></v-card-title>
              <div class="answer">{{responses.data}}</div>
            </v-img>
            <youtube :video-id="O7dq_wo5eto" :player-vars="{autoplay: 1}"></youtube>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </v-container>
</template>

<script>
import axios from "axios";
export default {
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
    }
  }),
  methods: {
    updateCultures() {
      axios.get("http://192.168.0.117:3000/cultures").then(response => {
        this.cultures.data = response.data;
        this.updateQuestions(this.cultures.data[0].name);
      });
    },
    updateQuestions(culture) {
      axios
        .get("http://192.168.0.117:3000/questions?culture=" + culture)
        .then(response => {
          this.questions.data = response.data;
          if (response.data[0]) {
            this.updateResponse(response.data[0].answer);
          } else {
            this.responses.data = "";
          }
        });
    },
    updateResponse(answer) {
      console.log(answer);
      this.responses.data = answer;
    },
    onSubmit() {
      axios
        .post("http://192.168.0.117:3000/cultures", {
          culture: {
            name: this.$refs.culturename.internalValue
          }
        })
        .then((res) => {
          console.log(res)
          if (!res.data.result){
            alert("sdfsfsf")
          }
          this.updateCultures();
        });
    }
  },
  beforeMount() {
    axios.get("http://192.168.0.117:3000/cultures").then(response => {
      this.cultures.data = response.data;
      this.questions.data = this.updateQuestions(response.data[0].name);
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

// form {
//   position: absolute;
//   top: 0;
//   display: flex;
//   align-items: left;
//   justify-content: space-around;
//   flex-direction: column;
//   padding: 90px 60px;
//   height: calc(100% - 180px);
//   text-align: left;
//   transition: all 0.5s ease-in-out;

//   div {
//     font-size: 1rem;
//   }

//   input {
//     background-color: #fff;
//     border: none;
//     padding: 10px 15px;
//     margin: 6px 0;
//     width: 100px;
//     border-radius: 6px;
//     overflow: hidden;

//     &:focus {
//       outline: none;
//     }
//   }
// }

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
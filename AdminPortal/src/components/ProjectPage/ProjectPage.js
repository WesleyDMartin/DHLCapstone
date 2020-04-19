export default {
  name: 'project-page',
  components: {},
  props: {
    source: String
  },
  data() {
    return {
      direction: "right",
      row_height: "50em",
      drawer: false,
      showBanner: true,
      currentTechnology: {},
      currentPerson: {},
      galleryImage: false,
      currentGalleryImage: {},
      items: [
        {
          src: require('../../assets/shots/Controls.png'),
          text: "Use the Oculus controller to navigate your way"
        },
        {
          src: require('../../assets/shots/UnityBTS.png'),
          text: "Built in Unity to support a fully immersive experience"
        },
        {
          src: require('../../assets/shots/CanadianCulture.png'),
          text: "Customise the supported content for any culture you wish"
        },
      ],
      rows: [
        {
          text: "Row one",
          src: 'https://cdn.vuetifyjs.com/images/carousel/squirrel.jpg',
          direction: "right"
        },
        {
          text: "Row two",
          src: 'https://cdn.vuetifyjs.com/images/carousel/sky.jpg',
          direction: "left"
        }
      ],
      technologies: [
        {
          tech: "Unity",
          description: "A cross platform game engine and development platform",
          use: "Unity is how we built the code base and combined all aspects of our platform. We used it to create the 3D world where the guide can answer questions. Unity integrates well with the Oculus platform, and as it is C# based, we were easily able to integrate our main application with the Rails API, Google's APIs, and the TensorFlow service.",
          links: [{
            value: "Out Unity Code",
            url: "https://github.com/WesleyDMartin/DHLCapstone/tree/master/VR/Practice"
          }],
          image: require("../../assets/shots/UnityBTS.png")
        },
        {
          tech: "Oculus",
          description: "Used to create Virtual Reality worlds",
          use: "Using the Oculus platform, we were able to create a truly immersive experience for our platforms users. The Oculus VR headset allows users to stand in front of the guide and ask them questions directly, and get both spoken and written responses back. The Oculus is the ideal way for a user to experience the 360 videos which our platform supports, allowing the user to feel like they are IN the place where the video was recorded.",
          links: [],
          image: require("../../assets/shots/Controls.png")
        },
        {
          tech: "Vue",
          description: "Used to build snappy user interfaces and single page web apps",
          use: "We used Vue to build the Administrative Panel for our application. We structured our front end around this framework, as well as all of the applications promotional web pages.",
          links: [{
            value: "Vue Application Code",
            url: "https://github.com/WesleyDMartin/DHLCapstone/tree/master/AdminPortal"
          }],
          image: require("../../assets/shots/AdminPage.png")
        },
        {
          tech: "TensorFlow",
          description: "An end to end Machine Learning platform",
          use: "TensorFlow is the platform we used to train the sentence encoder, which in turn is how we matched user questions to the most likely question and answer. E.g. A user asks 'What is a wedding like where you are form', and TensorFlow could match on 'What are marriage traditions like in your culture', or 'Do people like to get married where you live' (Assuming either of those questions had been added to the dashboard). Pictured to the right, you can see that the question 'how do you say hello' was asked, and that text is compared to each supported question. The best match is then returned.",
          links: [{
            value: "Our TensorFlow Script",
            url: "https://github.com/WesleyDMartin/DHLCapstone/blob/master/PythonTest/service.py"
          }],
          image: require("../../assets/shots/tensorQuestionMatching.png")
        },
        {
          tech: "Google Speech to Text API",
          description: "Google's Speech to Text and Text to Speech APIs are what we used to convert between spoken word and text",
          use: "User's questions are submitted to the API, and the text response is then sent to the Tensor Flow service. We also use it to generate the audio clips spoken by the guide in the Virtual Reality world. Pictured to the right you can see the service in action, recieving requests from the VR application, and passing those along to either the google API or the TensorFlow service.",
          links: [{
            value: "Our Google API Interfaces",
            url: "https://github.com/WesleyDMartin/DHLCapstone/tree/master/SpeechRecognizer"
          }],
          image: require("../../assets/shots/speechAPIService.png")
        },
        {
          tech: "Ruby/Rails",
          description: "Used to build RESTful APIs and single page web apps",
          use: "We used Ruby on Rails to build the API which provide the supporte cultures, questions, and answers to the Virtual Reality environment. The content is submitted to the API Via the Administrative Portal web app",
          links: [{
            value: "Our Rails Implementation",
            url: "https://github.com/WesleyDMartin/DHLCapstone/tree/master/VirtualLearnerApi"
          }],
          image: require("../../assets/shots/cultureApi.png")
        }
      ],
      people: [
        {
          name: "Wes",
          role: "Wes was the Project Lead, and was primarily responsible for the Unity and Oculus development, which included ingegration with Google APIs and the TensorFlow project complete by Ricardo. Wes was also responsible for the backend of the Administrative Portal API.",
          links: [{
            value: "LinkedIn",
            url: "https://www.linkedin.com/in/wesley-martin/"
          }, {
            value: "GitHub",
            url: "https://github.com/WesleyDMartin"
          }],
          image: "https://media-exp1.licdn.com/dms/image/C5603AQFics_xolPzfg/profile-displayphoto-shrink_200_200/0?e=1592438400&v=beta&t=vyC36VhylZgk7V_6Gsb2Dvq1aRE9SiwWb5MzgwjV03Y"
        },
        {
          name: "Humaira",
          role: "Humaira was the Team Manager. She helped keep us on track and on schedule. Her main role was as a cultural researcher to complete a sample supported culture for our application. She create the set of questions and answers for the Bangladeshi culture. She also was responsible for getting the admin portal and sign in page set up.",
          links: [{
            value: "LinkedIn",
            url: "https://www.linkedin.com/in/hsiddiqa/"
          }, {
            value: "GitHub",
            url: "https://github.com/hsiddiqa"
          }],
          image: "https://media-exp1.licdn.com/dms/image/C4E03AQEo1xOagBQ8ag/profile-displayphoto-shrink_800_800/0?e=1592438400&v=beta&t=Jf5dUJ2SY0n1Ga8lJl9WhWMa32eis_T4gDq3_pq8z2c"
        },
        {
          name: "Ricardo",
          role: "Ricardo was primarily responsible for setting up and training the models used in Tensor Flow. He set it up to properly match incoming questions to the bank of exisiting questions",
          links: [{
            value: "LinkedIn",
            url: "https://www.linkedin.com/in/ricardo-mohammed/"
          }, {
            value: "GitHub",
            url: "https://github.com/Fractalbroccoli"
          }],
          image: "https://media-exp1.licdn.com/dms/image/C4D03AQGnXoMXPOqLnA/profile-displayphoto-shrink_200_200/0?e=1592438400&v=beta&t=MnB9kbFvy89kXYHKl6TT05PcrsP-UDz3qaDibJuezDQ"
        }
      ],
      screenshots: [
        { title: "The Guide Welcome Shot", path: require("../../assets/shots/WelcomeShot.png") },
        { title: "A Sample 2D Video Response", path: require("../../assets/shots/2DVideoResponse.png") },
        { title: "A Sample 3D Video Response", path: require("../../assets/shots/3DVideoResponse.png") },
        { title: "A Sample 3D Video Response", path: require("../../assets/shots/3DVideoResponse2.png") },
        { title: "The Administrative Panel", path: require("../../assets/shots/AdminPage.png") },
        { title: "Questions for the Canadian Culture", path: require("../../assets/shots/CanadianCulture.png") },
        { title: "In World Oculus Controls", path: require("../../assets/shots/Controls.png") },
        { title: "A Sample List of Supported Cultures", path: require("../../assets/shots/CultureList.png") },
        { title: "The Guide Doesn't Have An Answer", path: require("../../assets/shots/DontKnowHowToAnswer.png") },
        { title: "The Guide Did Not Hear Your Question", path: require("../../assets/shots/HeardNothing.png") },
        { title: "In Video Playback Oculus Controls", path: require("../../assets/shots/InVideoControls.png") },
        { title: "Menu Selection", path: require("../../assets/shots/MenuSelect.png") },
        { title: "A List of Supported Questions", path: require("../../assets/shots/QuestionsList.png") },
        { title: "The Response for a Particular Question", path: require("../../assets/shots/ResponseView.png") },
        { title: "Selecting the Bangladeshi Culture", path: require("../../assets/shots/SelectBangladeshi.png") },
        { title: "Selecting the Canadian Culture", path: require("../../assets/shots/SelectCanadian.png") },
        { title: "A Text Only Response", path: require("../../assets/shots/TextOnlyResponse.png") },
        { title: "A Text Only Response", path: require("../../assets/shots/TextOnlyResponse2.png") },
        { title: "Text With 2D Video", path: require("../../assets/shots/TextWith2D.png") },
        { title: "Unity Behind the Scenes", path: require("../../assets/shots/UnityBTS.png") },
        { title: "Video Player Behind the Scenes", path: require("../../assets/shots/VideoPlayersBTS.png") },
        { title: "The Culture API in Action", path: require("../../assets/shots/cultureApi.png") },
        { title: "The Speech Synthesizer Service in Action", path: require("../../assets/shots/speechAPIService.png") },
        { title: "TensorFlow in Action", path: require("../../assets/shots/tensorQuestionMatching.png") },
      ]
    }
  },
  computed: {

  },
  created() {
    window.addEventListener('scroll', this.onScroll);
  },
  destroyed() {
    window.removeEventListener('scroll', this.onScroll);
  },
  mounted() {
    this.currentPerson = this.people[0]
    this.currentTechnology = this.technologies[0]
  },
  methods: {
    flipRows() {
      console.log(this.direction)
      if (this.direction === "right") {
        this.direction = "left"
      }
      else {
        this.direction = "right"
      }
      return this.direction
    },
    updateTech(i) {
      this.currentTechnology = this.technologies[i]
    },
    updatePerson(i) {
      this.currentPerson = this.people[i]
    },
    updateGalleryImage(image) {
      this.galleryImage = true
      this.currentGalleryImage = image
    }

  }
}



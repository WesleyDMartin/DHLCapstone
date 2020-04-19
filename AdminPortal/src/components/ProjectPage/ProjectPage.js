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
          tech: "Oculus",
          description: "Used for Virtual Reality",
          use: "It is how we provide a fully immersive experience to the users",
          links: [{
            value: "More Info",
            url: "https://www.google.com"
          }],
          image: require("../../assets/shots/Controls.png")
        },
        {
          tech: "Unity",
          description: "Used to build all sorts of applications",
          use: "Unity is how we built the code base and combined all aspects of our platform",
          links: [{
            value: "More Info",
            url: "https://www.google.com"
          }],
          image: require("../../assets/shots/UnityBTS.png")
        },
        {
          tech: "TensorFlow",
          description: "Used to build all sorts of applications",
          use: "Unity is how we built the code base and combined all aspects of our platform",
          links: [{
            value: "More Info",
            url: "https://www.google.com"
          }],
          image: require("../../assets/shots/Controls.png")
        },
        {
          tech: "Google Speech to Text and Text to Speech",
          description: "Used to build all sorts of applications",
          use: "Unity is how we built the code base and combined all aspects of our platform",
          links: [{
            value: "More Info",
            url: "https://www.google.com"
          }],
          image: require("../../assets/shots/Controls.png")
        },
        {
          tech: "Rails",
          description: "Used to build all sorts of applications",
          use: "Unity is how we built the code base and combined all aspects of our platform",
          links: [{
            value: "More Info",
            url: "https://www.google.com"
          }],
          image: require("../../assets/shots/UnityBTS.png")
        },
        {
          tech: "Vue",
          description: "Used to build all sorts of applications",
          use: "Unity is how we built the code base and combined all aspects of our platform",
          links: [{
            value: "More Info",
            url: "https://www.google.com"
          }],
          image: require("../../assets/shots/UnityBTS.png")
        }
      ],
      people: [
        {
          tech: "Wes",
          description: "Used for Virtual Reality",
          use: "It is how we provide a fully immersive experience to the users",
          links: [{
            value: "LinkedIn",
            url: "https://www.linkedin.com/in/wesley-martin/"
          }],
          image: "https://media-exp1.licdn.com/dms/image/C5603AQFics_xolPzfg/profile-displayphoto-shrink_200_200/0?e=1592438400&v=beta&t=vyC36VhylZgk7V_6Gsb2Dvq1aRE9SiwWb5MzgwjV03Y"
        },
        {
          tech: "Humaira",
          description: "Used to build all sorts of applications",
          use: "Unity is how we built the code base and combined all aspects of our platform",
          links: [{
            value: "LinkedIn",
            url: "https://www.linkedin.com/in/hsiddiqa/"
          }],
          image: "https://media-exp1.licdn.com/dms/image/C4E03AQEo1xOagBQ8ag/profile-displayphoto-shrink_800_800/0?e=1592438400&v=beta&t=Jf5dUJ2SY0n1Ga8lJl9WhWMa32eis_T4gDq3_pq8z2c"
        },
        {
          tech: "Ricardo",
          description: "Used to build all sorts of applications",
          use: "Unity is how we built the code base and combined all aspects of our platform",
          links: [{
            value: "LinkedIn",
            url: "https://www.linkedin.com/in/ricardo-mohammed/"
          }],
          image: "https://media-exp1.licdn.com/dms/image/C4D03AQGnXoMXPOqLnA/profile-displayphoto-shrink_200_200/0?e=1592438400&v=beta&t=MnB9kbFvy89kXYHKl6TT05PcrsP-UDz3qaDibJuezDQ"
        }
      ],
      screenshots: [
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
        { title: "The Guide Welcome Shot", path: require("../../assets/shots/WelcomeShot.png") }
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



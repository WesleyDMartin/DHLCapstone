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
      items: [
        { 
          src: require('../../assets/shots/Controls.png') ,
          text: "Use the Oculus controller to navigate your way"
        },
        { 
          src: require('../../assets/shots/UnityBTS.png') ,
          text: "Built in Unity to support a fully immersive experience"
        },
        { 
          src: require('../../assets/shots/CanadianCulture.png') ,
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
          tech: "Unity",
          description: "Used to build all sorts of applications",
          use: "Unity is how we built the code base and combined all aspects of our platform",
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
        require("../../assets/shots/2DVideoResponse.png"),
        require("../../assets/shots/3DVideoResponse.png"),
        require("../../assets/shots/3DVideoResponse2.png"),
        require("../../assets/shots/AdminPage.png"),
        require("../../assets/shots/CanadianCulture.png"),
        require("../../assets/shots/Controls.png"),
        require("../../assets/shots/CultureList.png"),
        require("../../assets/shots/DontKnowHowToAnswer.png"),
        require("../../assets/shots/HeardNothing.png"),
        require("../../assets/shots/InVideoControls.png"),
        require("../../assets/shots/MenuSelect.png"),
        require("../../assets/shots/QuestionsList.png"),
        require("../../assets/shots/ResponseView.png"),
        require("../../assets/shots/SelectBangladeshi.png"),
        require("../../assets/shots/SelectCanadian.png"),
        require("../../assets/shots/TextOnlyResponse.png"),
        require("../../assets/shots/TextOnlyResponse2.png"),
        require("../../assets/shots/TextWith2D.png"),
        require("../../assets/shots/UnityBTS.png"),
        require("../../assets/shots/VideoPlayersBTS.png"),
        require("../../assets/shots/WelcomeShot.png")
      ]
    }
  },
  computed: {

  },
  created () {
    window.addEventListener('scroll', this.onScroll);
  },
  destroyed () {
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
    }

  }
}



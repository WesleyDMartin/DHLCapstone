<template lang="html">

  <section class= "splash-page">
    <div class="square" v-for="index in rows" :key="index" :style="styles">
        <div class="content" :style="{ backgroundImage: 'url(' + require('@/assets/Backgrounds/' + (index % 29 + 1) + '.jpg') + ')' }">
        </div>
    </div>
  <div class="center">
    <img class="logo" src="../assets/dhl-people.png">
  </div>
  <div class="button" style="top: 0px">
              <v-btn class="btn" color="#1c76b8" to="/ProjectPage" x-large>Learn More About Our Project</v-btn>
  </div>
  <div class="button" style="top: 200px">
              <v-btn class="btn" color="#1c76b8" to="/SignIn" x-large>Go to the Administrator Dashbaord</v-btn>
  </div>
  </section>

</template>

<script lang="js">

  export default  {
    name: 'splash-page',
    props: [],
    mounted () {
      
    },
    created() {
      window.addEventListener("resize", this.updateScreenSize);
    },
    destroyed() {
      window.removeEventListener("resize", this.updateScreenSize);
    },
    data () {
      return {
        percentage: this.getPercentage(200/window.innerWidth*100),
        styles: {
          'background-color': 'blue',
          width: this.getPercentage(200/window.innerWidth*100)+'%',
          'padding-bottom': this.getPercentage(200/ window.innerWidth*100)+'%'
        },
        picIndex: (Math.floor(Math.random() * 29) + 1),
        width: window.innerWidth,
        height: window.innerHeight
      }
    },
    methods: {
      getColour(){
        var x = Math.floor(Math.random()*16777215).toString(16);
        return x
      },
      getPercentage(goal){
        console.log(goal)
        var counts = [20,25,33.33,50]

        var closest = counts.reduce(function(prev, curr) {
          return (Math.abs(curr - goal) < Math.abs(prev - goal) ? curr : prev);
        });
        return closest
      }
    },
    computed: {
      rows() {
        var area = (this.height * this.width)
        var imageSize = (this.width*(this.percentage/100)*this.width*(this.percentage/100))
        var extra = Math.max(this.width/this.height, this.height/this.width)
        var x = (area / imageSize) * extra
        return Math.ceil(x)
      }
    }
}


</script>

<style scoped lang="scss">
.splash-page {
  overflow: hidden;
  height: 100vh;
  width: 100vw;
  min-width: 500px;
  position: absolute;
  top: 0;
  left: 0;
}

.logo {
  max-width: 500px;
  min-width: 420px;
  width: 50%;
  outline: 5px solid;
  outline-color: grey; 
  background-color: white;
  padding: 5px;
}

.center {
  position: absolute;
  margin: auto;
  top: -300px;
  right: 0;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 9%;
  border-radius: 3px;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 200%;
  background-color: grey;
}

.btn {
  color: #ffffff;
  max-width: 500px;
  min-width: 220px;
}

.button {
  position: absolute;
  margin: auto;
  right: 0;
  bottom: 0;
  left: 0;
  height: 10%;
  border-radius: 3px;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 200%;
}

.inner {
  position: absolute;
}
.square {
  float: left;
  position: relative;
  background-color: #1e1e1e;
  overflow: hidden;
  filter: grayscale(100%);
}

.content {
  position: absolute;
  height: 100%; /* = 100% - 2*5% padding */
  width: 100%; /* = 100% - 2*5% padding */
  background-size: 100% 100%;
}
</style>

<template lang="html">

  <section v-if="direction === 'right'">
        <v-row
        class="main-body-row"
          no-gutters
          :style="{ height: rowheight}"
        >
        
          <v-col>
            <div class="main-text pt-1 pb-1"  :style="{ height: rowheight }">
            {{text}}
            </div>
          </v-col>
          <v-col>
            <div id="container">
              <img class="pt-1 pb-1" id="image" ref="image" :style="{ height: rowheight }" :src="assetpath" />
              <div class="pt-1 pb-1" id="fader" ref="fader" :style="{ height: rowheight }" />
            </div>
          </v-col>
        </v-row>
  </section>

  
  <section v-else>
        <v-row
        class="main-body-row"
          no-gutters

          :style="{ height: rowheight}"
        >
          <v-col cols="6">
            <div id="container">
              <img class="pt-1 pb-1" id="image" ref="image" :style="{ height: rowheight }" :src="assetpath" />
              <div class="pt-1 pb-1" id="fader" ref="fader" :style="{ height: rowheight }" />
            </div>
          </v-col>
          <v-col cols="6">
            <div class="main-text pt-1 pb-1"  :style="{ height: rowheight }">
            {{text}}
            </div>
          </v-col>
        </v-row>
  </section>

</template>

<script lang="js">

  export default  {
    name: 'main-body-row',
    props: [
      'text',
      'rowheight',
      'assetpath',
      'direction'
    ],
    mounted () {
      console.log(this.direction)
      this.$refs["fader"].style.width = this.$refs["image"].getBoundingClientRect().width + "px"
    },
    data () {
      return {

      }
    },
    methods: {
    },
    computed: {
    }
}


</script>

<style scoped lang="scss">
.main-body-row {
  background: -moz-linear-gradient(
    top,
    rgba(255, 255, 255, 0),
    rgba(255, 255, 255, 1)
  );
}

.main-text {
  display: table-cell;
  vertical-align: middle;
}

.row-fade {
  background: linear-gradient(
    to right,
    rgba(255, 255, 255, 1),
    rgba(255, 255, 255, 1),
    rgba(255, 255, 255, 0),
    rgba(255, 255, 255, 0),
    rgba(255, 255, 255, 0)
  );
}

img {
}

#container {
    position: relative;    
}

#fader {
   position: absolute;
   top: 0px;
   left: 0px; 
    
   padding: 5px;
  background: linear-gradient(
    to right,
    rgba(255, 255, 255, 1),
    rgba(255, 255, 255, 0),
    rgba(255, 255, 255, 0),
    rgba(255, 255, 255, 0),
    rgba(255, 255, 255, 0)
  );
}
</style>

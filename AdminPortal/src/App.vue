
// //   <div id="app">
// //     <div id="nav">
// //       <router-link to="/">Home</router-link> |
// //       <router-link to="/about">About</router-link>
// //     </div>
// //     <router-view/>
// //   </div>



<template>
<router-view>
  <div id="app">
    <!-- <SignIn/>
    <AdminPage/> -->
    <router-link to="/">SignIn</router-link>
    <router-link to="/">AdminPage</router-link>
  </div>
  </router-view>
</template>

<script>
// import SignIn from './components/SignIn.vue'
// import AdminPage from './components/AdminPage.vue'


export default {
  name: 'app',
  components: {
    // SignIn,
    // AdminPage
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}
</style>

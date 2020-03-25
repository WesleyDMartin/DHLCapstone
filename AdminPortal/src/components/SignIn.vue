<template>
  <article>
    <form class="sign-in" action="#">
      <img :src="require('../assets/dhl-people.png')" />
      <div>Please enter your User ID and Password to sign in</div>
      <input type="email" placeholder="Email" />
      <input type="password" placeholder="Password" />
      <a href="#">Forgot your password?</a>
      <button v-on:click="onSubmit">Sign In</button>
    </form>
  </article>
</template>

<script>
export default {
  methods: {
    onSubmit() {
      // this.$router.push("./AdminPage");
      try {
        console.log(this.$router);
        const path = `./AdminPage`;
        this.$router.push(path);
      } catch (err) {
        console.log(err);
      }
    }
  },
  data: () => {
    return {
      signUp: false
    };
  }
};
</script>

<style lang="scss" scoped>
@import url('https://fonts.googleapis.com/css?family=Open+Sans&display=swap');
.container {
  position: relative;
  width: 768px;
  height: 480px;
  border-radius: 10px;
  overflow: hidden;
  box-shadow: 0 15px 30px rgba(0, 0, 0, 0.2), 0 10px 10px rgba(0, 0, 0, 0.2);
  background: linear-gradient(to bottom, #efefef, #ccc);

  .overlay-container {
    position: absolute;
    top: 0;
    left: 50%;
    width: 50%;
    height: 100%;
    overflow: hidden;
    transition: transform 0.5s ease-in-out;
    z-index: 100;
  }
}

html * {
  font-family: 'Open Sans', sans-serif;
}

h1 {
  margin: 0;
}
p {
  margin: 20px 0 30px;
}
a {
  color: #222;
  text-decoration: none;
  margin: 15px 0;
  font-size: 1rem;
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

button.invert {
  background-color: transparent;
  border-color: #fff;
}
.sign-in {
  transform: translateX(0%);
  //   100% will make it go right
}

.sign-in {
  left: 0;
  z-index: 2;
}

form {
  position: absolute;
  top: 0;
  display: flex;
  align-items: center;
  justify-content: space-around;
  flex-direction: column;
  padding: 90px 60px;
  width: 100%;
  height: calc(100% - 180px);
  text-align: center;
  transition: all 0.5s ease-in-out;

  div {
    font-size: 1rem;
  }

  input {
    background-color: #eee;
    border: none;
    padding: 10px 15px;
    margin: 6px 0;
    width: calc(50% - 30px);
    min-width: 100px;
    border-radius: 6px;
    overflow: hidden;

    &:focus {
      outline: none;
    }
  }
}
</style>
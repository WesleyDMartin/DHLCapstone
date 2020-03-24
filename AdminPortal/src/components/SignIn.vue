<template>
<article>
    <form class="sign-in" action="#" >
        <h1>Sign In</h1>
              <img
        :src="require('../assets/dhl-people.png')"
      >
        <div>
            Please enter your User ID and Password to sign in
        </div>
        <input type="email" placeholder="Email"/>
        <input type="password" placeholder="Password"/>
        <a href ="#">Forgot your password?</a>
        <button v-on:click="onSubmit">Sign In</button>

    </form>


</article>
</template>

<script>
export default {
    methods:{
        onSubmit(){
            // this.$router.push("./AdminPage");
            try{
                console.log(this.$router)
            const path = `./AdminPage`
            this.$router.push(path)
            }
            catch(err){
                console.log(err);
            }

        }

    },
    data: ()=> {
        return {
            signUp: false
        }
    }
}

</script>

<style lang="scss" scoped>
  .container {
    position: relative;
    width: 768px;
    height: 480px;
    border-radius: 10px;
    overflow: hidden;
    box-shadow: 0 15px 30px rgba(0, 0, 0, .2),
                0 10px 10px rgba(0, 0, 0, .2);
    background: linear-gradient(to bottom, #efefef, #ccc);

    .overlay-container {
      position: absolute;
      top: 0;
      left: 50%;
      width: 50%;
      height: 100%;
      overflow: hidden;
      transition: transform .5s ease-in-out;
      z-index: 100;
    }

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
    border-radius: 20px;
    border: 1px solid black;
    background-color: orangered;
    color: #fff;
    font-size: 1rem;
    font-weight: bold;
    padding: 10px 40px;
    letter-spacing: 1px;
    text-transform: uppercase;
    cursor: pointer;
    transition: transform .1s ease-in;

    &:active {
      transform: scale(.9);
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
    width: calc(50% - 120px);
    height: calc(100% - 180px);
    text-align: center;
    background: linear-gradient(to bottom, #efefef, #ccc);
    transition: all .5s ease-in-out;

    div {
      font-size: 1rem;
    }

    input {
      background-color: #eee;
      border: none;
      padding: 8px 15px;
      margin: 6px 0;
      width: calc(100% - 30px);
      border-radius: 15px;
      border-bottom: 1px solid #ddd;
      box-shadow: inset 0 1px 2px rgba(0, 0, 0, .4), 
                        0 -1px 1px #fff, 
                        0 1px 0 #fff;
      overflow: hidden;

      &:focus {
        outline: none;
        background-color: #fff;
      }
    }
  }
</style>
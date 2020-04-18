import Vue from 'vue'
import App from './App.vue'
import router from './router'
import vuetify from './plugins/vuetify'
import VueAxios from './plugins/axios'
import VueYouTubeEmbed from 'vue-youtube-embed'
import MainBodyRow from './components/MainBodyRow.vue'
var cors = require('cors')
Vue.use(cors)
Vue.use(VueAxios)
Vue.use(VueYouTubeEmbed)
Vue.component('main-body-row', MainBodyRow)

Vue.config.productionTip = false

new Vue({
  router,
  vuetify,
  render: h => h(App)
}).$mount('#app')

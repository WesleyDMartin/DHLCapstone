import Vue from 'vue';
import Vuetify from 'vuetify/lib';

Vue.use(Vuetify);

const MY_ICONS = {
    dhl: 'AdminPortal/src/assets/dhl-logo.png'
    
}

export default new Vuetify({
    icons: {
        iconfont: 'mdi', // default
        values: MY_ICONS,
    },
});

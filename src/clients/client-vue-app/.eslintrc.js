module.exports = {
  "root": true,
  "env": {
    "node": true
  },
  "extends": [
    "plugin:vue/vue3-essential",
    "eslint:recommended"
  ],
  "parserOptions": {
    "parser": "babel-eslint"
  },
  rules: {
    "no-unused-vars": "off",
    "vue/no-unused-components": "off"
  },
  // extends: [
  //   'eslint:recommended',
  //   'plugin:vue/vue3-recommended',
  // ],
  // "env": {
  //   "node": true,
  //   "es6": true,
  // },
  // "parserOptions": {
  //   "ecmaVersion": 8,
  //   "sourceType": "module"
  // }
}

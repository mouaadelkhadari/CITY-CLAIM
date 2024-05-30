/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Areas/Identity/Pages/Account/Register.cshtml',
        './Areas/Identity/Pages/Account/Login.cshtml',
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml',
        './node_modules/flowbite/**/*.js'
    ],
  theme: {
    extend: {},
  },
    plugins: [
        require('flowbite/plugin')
    ],
}




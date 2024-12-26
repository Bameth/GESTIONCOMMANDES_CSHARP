/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Views/**/*.cshtml", // Pour inclure vos fichiers Razor
    "./wwwroot/js/**/*.js", // Vos fichiers JavaScript
    "./wwwroot/css/**/*.css", // Vos fichiers CSS si nécessaire
    "./node_modules/flowbite/**/*.js" // Inclut les composants Flowbite
  ],
  theme: {
    extend: {
      animation: {
        gradientBG: 'gradientBG 6s ease infinite',
      },
      keyframes: {
        gradientBG: {
          '0%': { backgroundPosition: '0% 50%' },
          '50%': { backgroundPosition: '100% 50%' },
          '100%': { backgroundPosition: '0% 50%' },
        },
      },
    },
  },
  plugins: [
    require('flowbite/plugin') // Ajoute le plugin Flowbite
  ],
};

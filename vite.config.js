import { defineConfig } from 'vite'
import tailwindcss from 'tailwindcss'
import autoprefixer from 'autoprefixer'

// https://vitejs.dev/config/
export default defineConfig({
  css: {
    postcss: {
      plugins: [tailwindcss, autoprefixer],
    },
    server: {
      port: 5198, // Port du serveur de développement
    },
  },
  build: {
    outDir: './wwwroot/dist', // Répertoire de sortie pour les fichiers compilés
  },
  server: {
    // Optionnel : si vous voulez activer un serveur de développement
    watch: {
      usePolling: true,
    },
  },
})

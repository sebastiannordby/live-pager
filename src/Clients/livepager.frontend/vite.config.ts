import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import mkcert from "vite-plugin-mkcert";

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    https: true,
    port: parseInt(process.env.PORT) || 3000,
  },
  plugins: [mkcert(), react()],
});

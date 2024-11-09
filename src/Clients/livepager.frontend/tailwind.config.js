/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        beige: "#e8e0c7",
        "brown-1": "#c3a97b",
        "brown-2": "#bda478",
        "green-1": "#99b091",
        "green-2": "#5d7f66",
        "green-3": "#2c4c3f",
        "green-4": "#484a22",
      },
      backgroundImage: {
        "gradient-radial": "radial-gradient(var(--tw-gradient-stops))",
        "gradient-conic":
          "conic-gradient(from 180deg at 50% 50%, var(--tw-gradient-stops))",
      },
    },
  },
  plugins: [],
};

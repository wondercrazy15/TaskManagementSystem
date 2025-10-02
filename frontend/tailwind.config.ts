import { type Config } from "tailwindcss";
import tailwindAnimate from "tailwindcss-animate";

const config: Config = {
  darkMode: ["class"],
  content: ["./index.html", "./src/**/*.{ts,tsx}"],
  theme: {
    extend: {
      colors: {
        primary: {
          200: "#D9F1FF",
          500: "#4EB5F4",
          800: "#3C81F3",
        },
        gray: {
          400: "#ECECEC",
          500: "#C8C8C8",
          600: "#979797",
          700: "#676767",
        },
        black: {
          500: "#1A202D",
        },
      },
    },
  },
  plugins: [tailwindAnimate],
};

export default config;

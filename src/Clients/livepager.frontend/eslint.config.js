import js from "@eslint/js";
import globals from "globals";
import reactHooks from "eslint-plugin-react-hooks";
import reactRefresh from "eslint-plugin-react-refresh";
import tseslint from "@typescript-eslint/eslint-plugin";
import tsParser from "@typescript-eslint/parser";

export default {
  ignores: ["dist"],
  extends: [
    js.configs.recommended,
    "plugin:@typescript-eslint/recommended",
    "plugin:@typescript-eslint/eslint-recommended",
  ],
  files: ["**/*.{ts,tsx}"],
  languageOptions: {
    ecmaVersion: 2020,
    globals: globals.browser,
    parser: tsParser,
    parserOptions: {
      project: "./tsconfig.json",
    },
  },
  plugins: {
    "@typescript-eslint": tseslint,
    "react-hooks": reactHooks,
    "react-refresh": reactRefresh,
  },
  rules: {
    ...reactHooks.configs.recommended.rules,
    ...tseslint.configs.recommended.rules,

    // Enforce no unused variables and imports
    "@typescript-eslint/no-unused-vars": ["warn", { argsIgnorePattern: "^_" }],
    "@typescript-eslint/no-unused-imports": "warn",

    // Specific React rules
    "react-refresh/only-export-components": [
      "warn",
      { allowConstantExport: true },
    ],
  },
};

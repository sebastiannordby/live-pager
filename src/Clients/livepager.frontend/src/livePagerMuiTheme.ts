import { createTheme } from "@mui/material/styles";

const livePagerTheme = createTheme({
  palette: {
    primary: {
      main: "#484a22", // Deep green for primary actions
      light: "#99b091", // Soft green for accents
      dark: "#2c4c3f", // Darker green for contrast
      contrastText: "#ffffff", // White for readability on green backgrounds
    },
    secondary: {
      main: "#c3a97b", // Light brown for secondary actions
      light: "#e8e0c7", // Beige for lighter accents
      dark: "#bda478", // Darker brown for strong emphasis
      contrastText: "#111827", // Dark gray text for legibility
    },
    background: {
      default: "#e8e0c7", // Beige as the background for a warm feel
      paper: "#ffffff", // White background for cards and dialogs
    },
    text: {
      primary: "#2c4c3f", // Dark green for primary text
      secondary: "#5d7f66", // Softer green for secondary text
    },
    error: {
      main: "#dc2626", // Strong red for error messages
    },
    warning: {
      main: "#f59e0b", // Bright orange for warnings
    },
    info: {
      main: "#99b091", // Soft green for informational messages
    },
    success: {
      main: "#5d7f66", // Calming green for success messages
    },
  },
  typography: {
    fontFamily: "Inter, Arial, sans-serif", // Fall back to Arial/sans-serif
    h1: {
      fontSize: "2.25rem",
      fontWeight: 700,
      color: "#1e3a8a", // Primary color for headings
    },
    h2: {
      fontSize: "1.875rem",
      fontWeight: 600,
      color: "#1e3a8a", // Primary color for subheadings
    },
    body1: {
      fontSize: "1rem",
      color: "#111827", // Primary text color
    },
    body2: {
      fontSize: "0.875rem",
      color: "#6b7280", // Secondary text color
    },
  },
  components: {
    MuiTextField: {
      defaultProps: {
        slotProps: {
          inputLabel: {
            shrink: true,
          },
        },
      },
    },
  },
});

export default livePagerTheme;

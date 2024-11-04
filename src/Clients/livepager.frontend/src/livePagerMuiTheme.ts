import { createTheme } from "@mui/material/styles";

const livePagerTheme = createTheme({
  palette: {
    primary: {
      main: "#1e3a8a", // Deep blue for reliability and trust
      light: "#3b82f6", // Lighter blue for accents
      dark: "#1e40af", // Dark blue for contrast
      contrastText: "#ffffff", // White text for readability
    },
    secondary: {
      main: "#ef4444", // Red for urgency and action (used sparingly)
      light: "#f87171", // Lighter red for accents
      dark: "#b91c1c", // Dark red for strong emphasis
      contrastText: "#ffffff", // White text for contrast
    },
    background: {
      default: "#f3f4f6", // Soft, neutral gray for a modern feel
      paper: "#ffffff", // White background for paper elements
    },
    text: {
      primary: "#111827", // Dark gray for primary text for better readability
      secondary: "#6b7280", // Medium gray for secondary text
    },
    error: {
      main: "#dc2626", // Strong red for error states
    },
    warning: {
      main: "#f59e0b", // Orange for warnings
    },
    info: {
      main: "#3b82f6", // Blue for informational messages
    },
    success: {
      main: "#10b981", // Green for success messages
    },
  },
  typography: {
    fontFamily: "'Roboto', sans-serif", // Clean and modern font
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
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: "8px", // Rounded corners for buttons
          textTransform: "none", // No uppercase text
          fontWeight: 600,
        },
        containedPrimary: {
          backgroundColor: "#1e3a8a",
          "&:hover": {
            backgroundColor: "#1e40af", // Darker on hover
          },
        },
        containedSecondary: {
          backgroundColor: "#ef4444",
          "&:hover": {
            backgroundColor: "#b91c1c", // Darker on hover
          },
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          borderRadius: "10px", // Slightly rounded corners for paper elements
          padding: "16px", // Default padding
        },
      },
    },
    MuiAppBar: {
      styleOverrides: {
        root: {
          backgroundColor: "#1e3a8a", // Primary color for app bar
          color: "#ffffff", // White text for the app bar
        },
      },
    },
  },
});

export default livePagerTheme;

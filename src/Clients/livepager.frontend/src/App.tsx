import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import TrackingPage from "./features/location/TrackingPage";
import Home from "./Home";
import ProtectedRoute from "./common/auth/protected-route";
import CreateMission from "./features/mission/CreateMissionPage";
import Footer from "./Footer";
import EnterMissionPage from "./features/mission/EnterMissionPage";
import { ActiveMissionPage } from "./features/mission/ActiveMissionPage";
import {
  RegisterUserPage,
  LoginUserPage,
  AuthShell,
} from "./features/authentication";
import { ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline"; // To apply global styles
import livePagerTheme from "./common/theme/livePagerMuiTheme";

function App() {
  return (
    <ThemeProvider theme={livePagerTheme}>
      <CssBaseline />
      <BrowserRouter>
        <Routes>
          {/* Authentication Routes (No Header or Footer) */}
          <Route path="/authentication" element={<AuthShell />}>
            <Route path="login" element={<LoginUserPage />} />
            <Route path="register" element={<RegisterUserPage />} />
          </Route>

          {/* Main Application Routes (With Header and Footer) */}
          <Route
            path="/"
            element={
              <>
                <header className="flex justify-between items-center p-4 bg-beige text-white">
                  <div className="flex items-center">
                    <h1 className="ml-4 text-2xl text-green-4 font-bold">
                      Live Pager
                    </h1>
                  </div>
                </header>
                <main className="flex flex-col h-full">
                  <Routes>
                    <Route
                      path="/mission/create"
                      element={
                        <ProtectedRoute>
                          <CreateMission />
                        </ProtectedRoute>
                      }
                    />
                    <Route
                      path="/mission/active"
                      element={
                        <ProtectedRoute>
                          <ActiveMissionPage />
                        </ProtectedRoute>
                      }
                    />
                    <Route
                      path="/mission/enter/:id"
                      element={
                        <ProtectedRoute>
                          <EnterMissionPage />
                        </ProtectedRoute>
                      }
                    />
                    <Route
                      path="/"
                      element={
                        <ProtectedRoute>
                          <Home />
                        </ProtectedRoute>
                      }
                    />
                    <Route
                      path="/tracking"
                      element={
                        <ProtectedRoute>
                          <TrackingPage />
                        </ProtectedRoute>
                      }
                    />
                  </Routes>
                </main>
                <Footer />
              </>
            }
          />
        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  );
}

export default App;

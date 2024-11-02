import { BrowserRouter, Link, Route, Routes } from "react-router-dom";
import "./App.css";
import TrackingPage from "./features/location/TrackingPage";
import Home from "./Home";
import LoginPage from "./LoginPage";
import ProtectedRoute from "./common/auth/protected-route";
import CreateMission from "./features/mission/CreateMissionPage";
import Footer from "./Footer";
import EnterMissionPage from "./features/mission/EnterMissionPage";
import { ActiveMissionPage } from "./features/mission/ActiveMissionPage";

function App() {
  return (
    <BrowserRouter>
      <header className="flex justify-between items-center p-4 bg-gray-800 text-white">
        <div className="flex items-center">
          <Link to="/" className="text-white">
            <h1 className="ml-4 text-2xl">Live Pager</h1>
          </Link>
        </div>
        <nav>
          <ul className="flex space-x-4">
            <li>
              <Link to="/login" className="text-white hover:underline">
                Login
              </Link>
            </li>
          </ul>
        </nav>
      </header>
      <main className="flex flex-col h-full">
        <Routes>
          <Route path="/login" element={<LoginPage />} />
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
    </BrowserRouter>
  );
}

export default App;

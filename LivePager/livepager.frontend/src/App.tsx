import { BrowserRouter, Link, Route, Routes } from "react-router-dom";
import "./App.css";
import TrackingPage from "./features/location/TrackingPage";
import Home from "./Home";

function App() {
  return (
    <BrowserRouter>
      <header className="flex justify-between items-center p-4 bg-gray-800 text-white">
        <div className="flex items-center">
          <h1 className="ml-4 text-2xl">Live Pager</h1>
        </div>
        <nav>
          <ul className="flex space-x-4">
            <li>
              <Link to="/" className="text-white hover:underline">
                Home
              </Link>
            </li>
            <li>
              <Link to="/tracking" className="text-white hover:underline">
                Tracking
              </Link>
            </li>
          </ul>
        </nav>
      </header>

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/tracking" element={<TrackingPage />} />
      </Routes>

      <footer className="flex justify-center items-center p-4 bg-gray-800 text-white">
        <p>© 2024 Live Tracker App</p>
      </footer>
    </BrowserRouter>
  );
}

export default App;

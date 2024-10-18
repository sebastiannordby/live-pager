// Home.js
import { Link } from "react-router-dom";

export default function Home() {
  return (
    <main className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
      <h2 className="text-3xl font-bold mb-4">
        Welcome to the Live Tracker App
      </h2>
      <p className="mb-4">Use this app to track user locations in real-time.</p>
      <Link to="/tracking">
        <button className="mt-4 px-4 py-2 text-white bg-blue-500 rounded hover:bg-blue-600">
          Start Tracking
        </button>
      </Link>
    </main>
  );
}

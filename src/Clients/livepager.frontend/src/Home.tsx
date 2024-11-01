// Home.js
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { GetMissionsResponseMissionDto } from "./data/models/mission/get-missions-response";
import { API } from "./data/api";

export default function Home() {
  const [missions, setMissions] = useState<GetMissionsResponseMissionDto[]>([]);

  useEffect(() => {
    (async () => {
      setMissions((await API.mission.getMissions()).missions);
    })();
  }, []);

  return (
    <main className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
      <h2 className="text-3xl font-bold mb-4">
        Welcome to the Live Tracker App
      </h2>
      <div>
        <p className="mb-4">
          Use this app to track user locations in real-time.
        </p>
        <Link to="/tracking">
          <button className="mt-4 px-4 py-2 text-white bg-blue-500 rounded hover:bg-blue-600">
            Start Tracking
          </button>
        </Link>
      </div>
      <div>
        <h3>Open Missions</h3>

        {missions.length == 0 ? (
          <p>No available missions</p>
        ) : (
          <>
            <ul>
              {missions.map((x) => (
                <li>{x.name}</li>
              ))}
            </ul>
          </>
        )}
      </div>
    </main>
  );
}

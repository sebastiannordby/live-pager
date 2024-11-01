// Home.js
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { GetMissionsResponseMissionDto } from "./data/models/mission/get-missions-response";
import { API } from "./data/api";
import { List, ListItem, ListItemText, Typography } from "@mui/material";

export default function Home() {
  const [missions, setMissions] = useState<GetMissionsResponseMissionDto[]>([]);

  useEffect(() => {
    (async () => {
      setMissions((await API.mission.getMissions()).missions);
    })();
  }, []);

  return (
    <main className="flex flex-col items-center min-h-screen pt-2 bg-gray-100">
      <h3 className="text-lg font-bold mb-4 text-center">
        Thanks for helping other people, you are a good person.
      </h3>
      <div>
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
          <List>
            {missions.map((mission) => (
              <ListItem key={mission.name}>
                <ListItemText
                  primary={
                    <div>
                      <Typography variant="h6">{mission.name}</Typography>
                      <Typography variant="subtitle1">
                        {mission.organization}
                      </Typography>

                      {!mission.updated ? (
                        ""
                      ) : (
                        <Typography variant="body2" color="textSecondary">
                          Updated:{" "}
                          {new Date(mission.updated).toLocaleDateString()}{" "}
                          {/* Format the date as needed */}
                        </Typography>
                      )}

                      {!mission.created ? (
                        ""
                      ) : (
                        <Typography variant="body2" color="textSecondary">
                          Created:{" "}
                          {new Date(mission.created).toLocaleDateString()}{" "}
                          {/* Format the date as needed */}
                        </Typography>
                      )}
                    </div>
                  }
                />
              </ListItem>
            ))}
          </List>
        )}
      </div>
    </main>
  );
}

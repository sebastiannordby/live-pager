// Home.js
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { GetMissionsResponseMissionDto } from "./data/models/mission/get-missions-response";
import { API } from "./data/api";
import {
  List,
  ListItem,
  ListItemText,
  Typography,
  Divider,
} from "@mui/material";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";

export default function Home() {
  const [missions, setMissions] = useState<GetMissionsResponseMissionDto[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    (async () => {
      setMissions((await API.mission.getMissions()).missions);
    })();
  }, []);

  const gotoMission = (missionId: string) => {
    navigate(`/mission/enter/${missionId}`);
  };

  return (
    <div className="w-full h-full flex-1 max-w-[500px] mx-auto">
      <div className="mt-4 flex flex-col items-center w-full overflow-hidden">
        <h3 className="text-2xl">Open Missions</h3>
        <p className="text-lg font-thin mb-4 text-center">
          Thanks for helping other people, you are a good person.
        </p>

        {missions.length == 0 ? (
          <p>No available missions</p>
        ) : (
          <List
            sx={{
              width: "100%",
              bgcolor: "background.paper",
              overflowY: "auto",
            }}
          >
            {missions.map((mission) => (
              <ListItem
                key={`${mission.name}_${mission.created}`}
                onClick={() => gotoMission(mission.id)}
                className="cursor-pointer hover:bg-gray-500 hover:text-white"
              >
                <ListItemText
                  primary={
                    <div className="flex justify-between items-center">
                      <div>
                        <Typography variant="h6">{mission.name}</Typography>
                        <Typography variant="subtitle1">
                          {mission.organization}
                        </Typography>

                        {!mission.updated ? (
                          ""
                        ) : (
                          <Typography variant="body2">
                            Updated:{" "}
                            {new Date(mission.updated).toLocaleDateString()}{" "}
                            {/* Format the date as needed */}
                          </Typography>
                        )}

                        {!mission.created ? (
                          ""
                        ) : (
                          <Typography variant="body2">
                            Created:{" "}
                            {new Date(mission.created).toLocaleDateString()}{" "}
                            {/* Format the date as needed */}
                          </Typography>
                        )}
                      </div>
                      <ArrowForwardIcon />
                    </div>
                  }
                />
              </ListItem>
            ))}
          </List>
        )}
      </div>
    </div>
  );
}

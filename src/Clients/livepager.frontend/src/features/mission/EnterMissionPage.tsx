import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { FindMissionResponse } from "../../data/models/mission";
import { API } from "../../data/api";
import { Button, CircularProgress } from "@mui/material";
import { Circle, MapContainer, Marker, TileLayer } from "react-leaflet";
import L, { LatLngExpression } from "leaflet";
import { useActiveMission } from "../../common/ActiveMissionProvider";

export default function EnterMissionPage() {
  const { id } = useParams();
  const [mission, setMission] = useState<FindMissionResponse>();
  const navigate = useNavigate();
  const activeMission = useActiveMission();

  useEffect(() => {
    if (!id) {
      navigate("/");
      return;
    }

    (async () => {
      setMission(await API.mission.findMission(id));
    })();
  }, [id]);

  if (!mission) {
    return <CircularProgress />;
  }

  const joinMission = async () => {
    activeMission.setMission(mission);
    await activeMission.connection!.send("JoinMissionGroup", mission.id);
    navigate("/mission/active");
  };

  const missionCenter = { lat: mission.latitude, lng: mission.longitude };

  const blueDotIcon = L.divIcon({
    className: "blue-marker-dot", // Add a CSS class for the dot
  });

  return (
    <div className="flex flex-col h-full text-center">
      <h1 className="text-xl my-4">Join Mission - {mission?.name}</h1>

      <MapContainer
        center={missionCenter as LatLngExpression}
        zoom={13}
        className="flex-1"
        style={{ width: "100%", flex: "1" }}
      >
        <TileLayer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        />

        <Marker position={missionCenter} icon={blueDotIcon} />
        <Circle
          center={missionCenter}
          radius={mission.searchRadius}
          fillColor="#2965ff"
        />
      </MapContainer>

      <Button
        variant="contained"
        className="mt-2"
        color={"primary"}
        onClick={async () => await joinMission()}
      >
        Join mission
      </Button>
    </div>
  );
}

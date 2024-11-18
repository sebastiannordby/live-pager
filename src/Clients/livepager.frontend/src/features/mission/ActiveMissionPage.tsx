import { useEffect, useState } from "react";
import { Circle, MapContainer, Marker, TileLayer } from "react-leaflet";
import L, { LatLngExpression } from "leaflet";
import { useActiveMission } from "../../common/ActiveMissionProvider";
import getAxiousInstance from "../../data/axios";

type ParticipantCurrentLocation = {
  displayName: string;
  longitude: number;
  latitude: number;
};

type MissionLocationDataPoint = {
  missionId: string; // Guid represented as a string
  participantId: string; // Guid represented as a string
  participantDisplayName: string; // Guid represented as a string, assuming this is intended to be a name or display text
  longitude: number;
  latitude: number;
};

const sendLocationData = async (data: {
  latitude: number;
  longitude: number;
}) => {
  try {
    await getAxiousInstance().post("/api/mission/location", {
      userIdentificator: "unique-user-id",
      latitude: data.latitude,
      longitude: data.longitude,
    });
  } catch (error) {
    console.error("Error sending location data:", error);
  }
};

export function ActiveMissionPage() {
  // const [participantLocations, setParticipantLocations] =
  //   useState<ParticipantCurrentLocation[]>();
  const [isTracking, setIsTracking] = useState<boolean>(false);
  const [gpsWatchId, setGpsWatchId] = useState<number | null>(null); // Move watchId to state
  const activeMission = useActiveMission();
  const mission = activeMission.mission!;

  const handleSuccess = (position: GeolocationPosition) => {
    const { latitude, longitude } = position.coords;
    // setLocation({ latitude, longitude });
    sendLocationData({ latitude, longitude });
  };

  const handleError = (error: GeolocationPositionError) => {
    console.error("Error getting location:", error, gpsWatchId, isTracking);
  };

  const startTracking = () => {
    console.log("Starting tracking");

    if (navigator.geolocation) {
      console.log("Starting tracking");

      const id = navigator.geolocation.watchPosition(
        handleSuccess,
        handleError
      );

      setGpsWatchId(id);
      setIsTracking(true);
      console.log("Started tracking");
    } else {
      console.error("Geolocation is not supported by this browser.");
    }
  };

  useEffect(() => {
    startTracking();
  }, []);

  useEffect(() => {
    if (!activeMission.connection) return;

    activeMission.connection.on(
      "ReceiveLocationUpdate",
      (dataPoint: MissionLocationDataPoint) => {
        console.log("Message received:", dataPoint);
      }
    );

    return () => {
      activeMission.connection?.off("ReceiveLocationUpdate");
    };
  }, [activeMission.connection]);

  const missionCenter = {
    lng: mission.longitude,
    lat: mission.latitude,
  } as LatLngExpression;

  const blueDotIcon = L.divIcon({
    className: "blue-marker-dot", // Add a CSS class for the dot
  });

  return (
    <MapContainer
      center={missionCenter}
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
  );
}

import React, { useEffect, useState } from "react";
import {
  MapContainer,
  TileLayer,
  Marker,
  Circle,
  useMapEvents
  // useMap,
} from "react-leaflet";
import L, { LatLngExpression } from "leaflet";
import { Button, Slider } from "@mui/material";
import "leaflet/dist/leaflet.css";

// Interface for LatLng position
interface LatLng {
  lat: number;
  lng: number;
}

const CreateMission: React.FC = () => {
  const [position, setPosition] = useState<LatLng | null>(null);
  const [userPosition, setUserPosition] = useState<LatLng | null>(null);
  const [radius, setRadius] = useState<number>(500);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          setUserPosition({
            lng: position.coords.longitude,
            lat: position.coords.latitude,
          });
          setIsLoading(false);
        },
        (error) => {
          console.error(error);
          setIsLoading(false);
        }
      );
    }
  }, []);

  const blueDotIcon = L.divIcon({
    className: "blue-marker-dot", // Add a CSS class for the dot
  });

  // Handle map click events
  const MapClickHandler: React.FC = () => {
    // const map = useMap();
    useMapEvents({
      click(e) {
        setPosition(e.latlng); // Set marker position on map click
      },
    });

    // useEffect(() => {
    //   if (position) {
    //     map.setView(position);
    //     const zoomLevel = Math.max(13 - (radius / 500) * 0.3, 8); // Adjust zoom formula
    //     map.zoomOut(zoomLevel);
    //   }
    // }, [position, radius, map]);

    return null;
  };

  if (isLoading) {
    return <label>Loading..</label>;
  }

  return (
    <div className="flex flex-col flex-1">
      {/* Map container */}
      <MapContainer
        center={userPosition as LatLngExpression}
        zoom={13}
        className="flex-1"
        style={{ width: "100%" }}
      >
        <TileLayer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        />
        {/* Map click handler */}
        <MapClickHandler />
        {/* Marker and Circle */}
        {position && (
          <>
            <Marker position={position} icon={blueDotIcon} />
            <Circle center={position} radius={radius} fillColor="#2965ff" />
          </>
        )}
      </MapContainer>

      {/* Radius slider */}
      <div className="mt-4 p-4">
        <label>Radius: {radius} meters</label>
        <Slider
          value={radius}
          min={100}
          max={5000}
          step={100}
          onChange={(_, newValue) => setRadius(newValue as number)}
          valueLabelDisplay="auto"
        />
      </div>

      <Button className="mt-2">Create Mission</Button>
    </div>
  );
};

export default CreateMission;

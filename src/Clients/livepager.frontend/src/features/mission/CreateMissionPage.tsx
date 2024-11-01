import React, { useEffect, useState } from "react";
import {
  MapContainer,
  TileLayer,
  Marker,
  Circle,
  useMapEvents,
  // useMap,
} from "react-leaflet";
import L, { LatLngExpression } from "leaflet";
import {
  Button,
  Input,
  MobileStepper,
  Slider,
  Step,
  TextField,
} from "@mui/material";
import "leaflet/dist/leaflet.css";
import { API } from "../../data/api";
import { CreateMissionRequest } from "../../data/models/mission/create-mission-request";
import { useNavigate } from "react-router-dom";

// Interface for LatLng position
interface LatLng {
  lat: number;
  lng: number;
}

enum Steps {
  SearchArea = 0,
  Info = 1,
}

const CreateMission: React.FC = () => {
  const navigate = useNavigate();
  const [position, setPosition] = useState<LatLng | null>(null);
  const [userPosition, setUserPosition] = useState<LatLng | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [currentStep, setCurrentStep] = useState<Steps>(Steps.SearchArea);

  const [createMissionRequest, setCreateMissionRequest] =
    useState<CreateMissionRequest>({
      name: "",
      description: "",
      latitude: 0,
      longitude: 0,
      searchRadius: 500,
      organization: "",
    });

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

  const MapClickHandler: React.FC = () => {
    useMapEvents({
      click(e) {
        setPosition(e.latlng); // Set marker position on map click
        setCreateMissionRequest({
          ...createMissionRequest,
          longitude: e.latlng.lng,
          latitude: e.latlng.lat,
        });
      },
    });

    return null;
  };

  const executeCreateMission = async () => {
    await API.mission.createMission(createMissionRequest);
  };

  const gotoNextStep = async () => {
    if (currentStep == Steps.Info) {
      await executeCreateMission();
      navigate("/");
    } else {
      setCurrentStep((currentStep as number) + 1);
    }
  };

  if (isLoading) {
    return <label>Loading..</label>;
  }

  if (currentStep == Steps.SearchArea) {
    return (
      <div className="flex flex-col flex-1">
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

          <MapClickHandler />

          {position && (
            <>
              <Marker position={position} icon={blueDotIcon} />
              <Circle
                center={position}
                radius={createMissionRequest.searchRadius}
                fillColor="#2965ff"
              />
            </>
          )}
        </MapContainer>
        <form className="p-4">
          <label className="block mb-1">
            Radius: {createMissionRequest.searchRadius} meters
          </label>
          <Slider
            value={createMissionRequest.searchRadius}
            min={100}
            max={5000}
            step={100}
            onChange={(_, newValue) =>
              setCreateMissionRequest({
                ...createMissionRequest,
                searchRadius: newValue as number,
              })
            }
            valueLabelDisplay="auto"
          />
        </form>
        <Button
          variant="contained"
          className="mt-2"
          color={"primary"}
          onClick={gotoNextStep}
        >
          Next
        </Button>
      </div>
    );
  }

  if (currentStep == Steps.Info) {
    return (
      <div className="flex flex-col flex-1">
        {/* Map container */}
        <MapContainer
          center={userPosition as LatLngExpression}
          zoom={13}
          className="flex-1"
          style={{ maxHeight: "200px", width: "100%" }}
        >
          <TileLayer
            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          />
          {position && (
            <>
              <Marker position={position} icon={blueDotIcon} />
              <Circle
                center={position}
                radius={createMissionRequest.searchRadius}
                fillColor="#2965ff"
              />
            </>
          )}
        </MapContainer>

        <form className="flex flex-col gap-2 p-2">
          <div>
            <TextField
              required
              id="outlined-required"
              fullWidth={true}
              label="Name"
              value={createMissionRequest.name}
              onChange={(e) =>
                setCreateMissionRequest({
                  ...createMissionRequest,
                  name: e.target.value,
                })
              }
            />
          </div>
          <div>
            <TextField
              required
              id="outlined"
              fullWidth={true}
              label="Organization"
              value={createMissionRequest.organization}
              onChange={(e) =>
                setCreateMissionRequest({
                  ...createMissionRequest,
                  organization: e.target.value,
                })
              }
            />
          </div>
          <div>
            <TextField
              value={createMissionRequest.description}
              id="outlined"
              label="Description"
              multiline={true}
              fullWidth={true}
              rows={5}
              onChange={(e) =>
                setCreateMissionRequest({
                  ...createMissionRequest,
                  description: e.target.value,
                })
              }
            />
          </div>
        </form>

        <Button
          variant="contained"
          className="mt-2"
          color={"primary"}
          onClick={gotoNextStep}
        >
          Create Mission
        </Button>
      </div>
    );
  }
};

export default CreateMission;

import { useState } from "react";
import getAxiousInstance from "../../data/axios";

export default function TrackingPage() {
  const [isTracking, setIsTracking] = useState(false);
  const [location, setLocation] = useState<{
    latitude: number;
    longitude: number;
  } | null>(null);
  const [watchId, setWatchId] = useState<number | null>(null); // Move watchId to state

  const handleSuccess = (position: GeolocationPosition) => {
    const { latitude, longitude } = position.coords;
    setLocation({ latitude, longitude });
    sendLocationData({ latitude, longitude });
  };

  const handleError = (error: GeolocationPositionError) => {
    console.error("Error getting location:", error);
  };

  const sendLocationData = async (data: {
    latitude: number;
    longitude: number;
  }) => {
    try {
      await getAxiousInstance().post("/location", {
        userIdentificator: "unique-user-id",
        latitude: data.latitude,
        longitude: data.longitude,
      });
    } catch (error) {
      console.error("Error sending location data:", error);
    }
  };

  const startTracking = () => {
    console.log("Starting tracking");

    if (navigator.geolocation) {
      console.log("Starting tracking");

      const id = navigator.geolocation.watchPosition(
        handleSuccess,
        handleError
      );
      setWatchId(id);
      setIsTracking(true);

      console.log("Started tracking");
    } else {
      console.error("Geolocation is not supported by this browser.");
    }
  };

  const stopTracking = () => {
    if (watchId) {
      navigator.geolocation.clearWatch(watchId);
      setWatchId(null);
      setIsTracking(false);
      console.log("Stopped tracking");
    }
  };

  const toggleTracking = () => {
    if (isTracking) {
      stopTracking();
    } else {
      startTracking();
    }
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
      <h1 className="text-4xl font-bold mb-4">Live Tracking</h1>
      {location && (
        <p className="text-lg">
          Current Location: Latitude {location.latitude.toFixed(6)}, Longitude{" "}
          {location.longitude.toFixed(6)}
        </p>
      )}
      <button
        className={`mt-4 px-4 py-2 text-white ${
          isTracking ? "bg-red-500" : "bg-green-500"
        } rounded`}
        onClick={toggleTracking}
      >
        {isTracking ? "Stop Tracking" : "Start Tracking"}
      </button>
    </div>
  );
}

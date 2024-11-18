import React, { createContext, useContext, useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { FindMissionResponse } from "../data/models/mission";

export type ActiveMission = {
  connection: signalR.HubConnection | null;
  mission: FindMissionResponse | null;
  setMission: React.Dispatch<React.SetStateAction<FindMissionResponse | null>>;
};

const ActiveMissionConnectionContext = createContext<ActiveMission | null>(
  null
);

export const ActiveMissionConnectionProvider: React.FC<{
  children: React.ReactNode;
}> = ({ children }) => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );
  const [mission, setMission] = useState<FindMissionResponse | null>(null);

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(import.meta.env.VITE_API_URI + "mission-hub", {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .build();

    newConnection
      .start()
      .then(() => {
        console.log("Connected to SignalR");
      })
      .catch((err) => console.error("SignalR Connection Error: ", err));

    setConnection(newConnection);

    return () => {
      newConnection.stop();
    };
  }, []);

  return (
    <ActiveMissionConnectionContext.Provider
      value={{
        connection,
        mission,
        setMission,
      }}
    >
      {children}
    </ActiveMissionConnectionContext.Provider>
  );
};

export const useActiveMission = () => {
  const context = useContext(ActiveMissionConnectionContext);
  if (!context) {
    throw new Error("useSignalR must be used within a SignalRProvider");
  }
  return context;
};

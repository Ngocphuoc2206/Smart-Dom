import { useEffect, useRef, useState } from "react";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
  HubConnectionState,
} from "@microsoft/signalr";

export const useChatConnection = (userId: string) => {
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const isMountedRef = useRef(true);
  const connectionRef = useRef<HubConnection | null>(null); // ✅ giữ tham chiếu connection

  useEffect(() => {
    if (!userId) return;

    const newConnection = new HubConnectionBuilder()
      .withUrl(`https://localhost:7257/chatHub?userId=${userId}`)
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    if (connectionRef.current?.state === HubConnectionState.Connected) {
      console.log("⚠️ Connection already established");
      return;
    }

    connectionRef.current = newConnection;
    setConnection(newConnection);

    const startConnection = async () => {
      try {
        await newConnection.start();
        console.log("✅ SignalR connected to chatHub");
      } catch (error) {
        console.error("❌ Failed to start connection:", error);
      }
    };

    startConnection();

    return () => {
      if (connectionRef.current?.state === HubConnectionState.Connected) {
        connectionRef.current.stop();
        console.log("🛑 SignalR chatHub stopped");
      }
    };
  }, [userId]);

  return connection;
};

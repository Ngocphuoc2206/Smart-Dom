import { useEffect, useState, useRef } from "react";
import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import { getNotification } from "./useNotification";

type Notification = {
  isRead: boolean;
  [key: string]: any;
};

export const useSignalRNotification = (userId?: string | number) => {
  const [notifications, setNotifications] = useState<string[]>([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const connectionRef = useRef<any>(null);
  useEffect(() => {
    if (!userId) return;

    const connection = new HubConnectionBuilder()
      .withUrl(`https://localhost:7257/hub/notifications?userId=${userId}`)
      .withAutomaticReconnect()
      .build();

    connectionRef.current = connection;

    const startConnection = async () => {
      try {
        if (connection.state === HubConnectionState.Disconnected) {
          await connection.start();
          console.log("SignalR Connected");

          connection.on("ReceiveNotification", (message: string) => {
            console.log("📨 Notification received:", message);
            setNotifications((prev) => [...prev, message]);
            setUnreadCount((prev) => prev + 1);
          });
        }
      } catch (error) {
        console.error(" SignalR connection failed:", error);
      }
    };

    startConnection();

    return () => {
      if (
        connectionRef.current &&
        connectionRef.current.state === HubConnectionState.Connected
      ) {
        connectionRef.current.stop();
      }
    };
  }, [userId]);

  return { notifications, unreadCount };
};

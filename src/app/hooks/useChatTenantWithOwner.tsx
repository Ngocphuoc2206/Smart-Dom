import { useEffect, useState } from "react";
import { createChatConnection } from "@/app/hooks/useChatConnection";

const userId = "tenant123"; // hoặc "owner456"
const receiverId = "owner456"; // hoặc "tenant123"

const [connection, setConnection] = useState<any>(null);
const [messages, setMessages] = useState<any[]>([]);
const [newMessage, setNewMessage] = useState("");

useEffect(() => {
  const conn = createChatConnection(userId);

  conn.start().then(() => {
    console.log("Connected SignalR");
    conn.on("ReceiveMessage", (msg: any) => {
      setMessages((prev) => [...prev, msg]);
    });
  });

  setConnection(conn);

  return () => {
    conn.stop();
  };
}, []);

const sendMessage = async () => {
  if (newMessage.trim()) {
    await connection.invoke("SendMessage", userId, receiverId, newMessage);
    setNewMessage("");
  }
};

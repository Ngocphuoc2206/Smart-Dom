// hooks/useChatConnection.ts
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

export const createChatConnection = (userId: string) => {
  return new HubConnectionBuilder()
    .withUrl(`http://localhost:5000/chathub?userId=${userId}`)
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
    .build();
};

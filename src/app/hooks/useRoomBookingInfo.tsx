export async function getRoomBookingInfo() {
  try {
    const res = await fetch("https://localhost:7257/api/RoomBooking/tenant");
    if (!res.ok) {
      throw new Error("Network response was not ok");
    }

    const data = await res.json();
    if (!Array.isArray(data)) {
      throw new Error("Expected an array of room booking info");
    }
    return data;
  } catch (error) {
    console.error("Error fetching room data:", error);
    return [];
  }
}

export async function getRoom() {
  try {
    const res = await fetch("https://localhost:7257/api/Room");
    if (!res.ok) {
      throw new Error("Network response was not ok");
    }
    const data = await res.json();
    return data;
  } catch (error) {
    console.error("Error fetching room data:", error);
    return []; // Return an empty array or handle the error as needed
  }
}

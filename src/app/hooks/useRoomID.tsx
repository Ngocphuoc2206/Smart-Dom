export async function getRoomID(id: number) {
  try {
    const res = await fetch(`https://localhost:7257/api/Room/${id}`);
    if (!res.ok) {
      throw new Error("Network response was not ok");
    }
    const data = await res.json();

    return data;
  } catch (error) {
    console.error("Error fetching room data:", error);
    return [];
  }
}

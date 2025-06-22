export async function getRoom() {
  try {
    const res = await fetch("https://localhost:7257/api/Room");
    if (!res.ok) {
      throw new Error("Network response was not ok");
    }

    const data = await res.json();
    console.log("Dữ liệu trả về:", data); // 👉 debug xem có đúng không

    return Array.isArray(data.result) ? data.result : [];
  } catch (error) {
    console.error("Error fetching room data:", error);
    return [];
  }
}

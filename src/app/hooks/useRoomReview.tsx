export async function getRoomReview() {
  try {
    const res = await fetch("https://localhost:7257/api/RoomReview");
    if (!res.ok) {
      throw new Error("Network response was not ok");
    }
    const data = await res.json();
    return data;
  } catch (error) {
    console.error("Lỗi khi lấy danh sách đăng ký:", error);
    return []; // fallback
  }
}

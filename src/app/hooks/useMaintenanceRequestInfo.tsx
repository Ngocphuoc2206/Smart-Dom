export async function getMaintenanceRequestInfo() {
  try {
    const res = await fetch(
      "https://localhost:7257/api/MaintenanceRequest/tenant"
    );
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

export async function AccountUser() {
  try {
    const res = await fetch("https://localhost:7257/api/Account");
    const data = await res.json();
    return data;
  } catch (error) {
    console.error("Lỗi khi lấy danh sách đăng ký:", error);
    return []; // fallback
  }
}

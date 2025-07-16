export async function getInvoiceByUserID(id: any) {
  try {
    const res = await fetch(`https://localhost:7257/api/Invoice/${id}`);

    if (!res.ok) {
      throw new Error(`HTTP error! Status: ${res.status}`);
    }

    // Kiểm tra nếu không có nội dung trả về
    const text = await res.text();
    if (!text) {
      console.warn("API trả về rỗng");
      return null;
    }

    const data = JSON.parse(text);
    return data;
  } catch (error) {
    console.error("Lỗi khi lấy hóa đơn theo ID:", error);
    return null;
  }
}

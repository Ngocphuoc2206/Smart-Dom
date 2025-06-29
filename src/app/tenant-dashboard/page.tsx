"use client";

import Link from "next/link";
import { useState, useEffect } from "react";
import { useAuth } from "@/contexts/AuthContext";
import { useRouter } from "next/navigation";
import { getRoom } from "@/app/hooks/useRoom";
import { getInvoice } from "../hooks/useInvoice";
import { getMaintenanceRequest } from "../hooks/useMaintenanceRequest";
import { getRoomReview } from "../hooks/useRoomReview";
import { getRoomBookingInfo } from "../hooks/useRoomBookingInfo";
import { getNotification } from "../hooks/useNotification";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { getContract } from "../hooks/useContract";
import { useSignalRNotification } from "../hooks/useSignalRNotification";

type Notification = {
  id: number;
  userId: number;
  title: string;
  message: string;
  createAt: Date;
  isRead: boolean;
  [key: string]: any;
};

export default function TenantDashboard() {
  const [activeTab, setActiveTab] = useState("overview");
  const { logout } = useAuth();
  const { user } = useAuth();
  const router = useRouter();
  const [rooms, setRooms] = useState<any[]>([]);
  const [invoiceTenant, setInvoiceTenant] = useState<any[]>([]);
  const [RoomReviews, setRoomReviews] = useState<any[]>([]);
  const [Contracts, setContracts] = useState<any[]>([]);
  const [maintenanceRequest, setMaintenanceRequest] = useState<any[]>([]);
  const [roomBookingInfo, setRoomBookingInfo] = useState<any[]>([]);
  const [getNotifications, setGetNotifications] = useState<Notification[]>([]);
  const [unreadCountNotify, setUnreadCountNotify] = useState(0);
  const { notifications, unreadCount } = useSignalRNotification(
    user?.idUser ? Number(user.idUser) : undefined
  );

  useEffect(() => {
    getRoom().then(setRooms);
    getInvoice().then(setInvoiceTenant);
    getMaintenanceRequest().then(setMaintenanceRequest);
    getRoomBookingInfo().then(setRoomBookingInfo);
    getRoomReview().then(setRoomReviews);
    getNotification().then(setGetNotifications);
    getContract().then(setContracts);
  }, []);

  useEffect(() => {
    const notify = getNotifications.filter((n) => n.isRead === false).length;
    const sumNotify = notify + unreadCount;
    setUnreadCountNotify(sumNotify);
  }, [getNotifications, unreadCount]); // 👈 chỉ khi dữ liệu này thay đổi mới chạy

  const handleLogout = () => {
    logout();
    router.push("/");
  };

  const [showReportModal, setShowReportModal] = useState(false);
  const [reportForm, setReportForm] = useState({
    issue: "",
    description: "",
    priority: "medium",
    location: "",
  });

  //Connection Notifycation

  // Room search filters
  const [roomFilters, setRoomFilters] = useState({
    status: "all", // all, available, occupied, maintenance
    minPrice: "",
    maxPrice: "",
    minArea: "",
    searchTerm: "",
  });

  function formatDate(dateString: string) {
    const date = new Date(dateString);
    const month = (date.getMonth() + 1).toString().padStart(2, "0"); // tháng bắt đầu từ 0
    const year = date.getFullYear();
    return `${month}/${year}`;
  }

  const formatDateTime = (dateString: any) => {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0"); // Tháng bắt đầu từ 0
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case "occupied":
        return "bg-green-100 text-green-800";
      case "available":
        return "bg-blue-100 text-blue-800";
      case "maintenance":
        return "bg-yellow-100 text-yellow-800";
      case "paid":
        return "bg-green-100 text-green-800";
      case "pending":
        return "bg-yellow-100 text-yellow-800";
      case "pending processing":
        return "bg-yellow-100 text-yellow-800"; // 👈 Thêm dòng này
      case "overdue":
        return "bg-red-100 text-red-800";
      case "in-progress":
        return "bg-blue-100 text-blue-800";
      case "completed":
        return "bg-green-100 text-green-800";
      default:
        return "bg-gray-100 text-gray-800";
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case "paid":
        return "Đã thanh toán";
      case "pending":
        return "Chờ thanh toán";
      case "overdue":
        return "Quá hạn";
      case "in-progress":
        return "Đang xử lý";
      case "completed":
        return "Hoàn thành";
      case "active":
        return "Đang hiệu lực";
      case "available":
        return "Phòng trống";
      case "occupied":
        return "Đã thuê";
      case "pending processing":
        return "Chờ xử lý";
      case "maintenance":
        return "Bảo trì";
      default:
        return status;
    }
  };

  const getRoomStatusColor = (status: string) => {
    switch (status) {
      case "available":
        return "bg-green-100 text-green-800";
      case "occupied":
        return "bg-blue-100 text-blue-800";
      case "maintenance":
        return "bg-yellow-100 text-yellow-800";
      default:
        return "bg-gray-100 text-gray-800";
    }
  };

  // Filter rooms based on criteria
  const filteredRooms = rooms.filter((room) => {
    const matchesStatus =
      roomFilters.status === "all" || room.status === roomFilters.status;
    const matchesMinPrice =
      !roomFilters.minPrice || room.price >= parseInt(roomFilters.minPrice);
    const matchesMaxPrice =
      !roomFilters.maxPrice || room.price <= parseInt(roomFilters.maxPrice);
    const matchesMinArea =
      !roomFilters.minArea || room.area >= parseInt(roomFilters.minArea);
    const matchesSearch =
      !roomFilters.searchTerm ||
      (Array.isArray(room.amenities) ? room.amenities : []).some(
        (amenity: string) =>
          amenity.toLowerCase().includes(roomFilters.searchTerm.toLowerCase())
      );

    return (
      matchesStatus &&
      matchesMinPrice &&
      matchesMaxPrice &&
      matchesMinArea &&
      matchesSearch
    );
  });

  const handleFilterChange = (field: string, value: string) => {
    setRoomFilters((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleSubmitReport = async (e: React.FormEvent) => {
    e.preventDefault();
    const IDRoom = rooms.find(
      (r) =>
        r.fullName.trim().toLowerCase() === user?.name?.trim().toLowerCase()
    )?.id;

    alert(`IDRoom: ${IDRoom} - UserName: ${user?.name}`);
    try {
      const response = await fetch(
        "https://localhost:7257/api/MaintenanceRequest/create",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            userId: user?.idUser,
            roomId: IDRoom,
            incidentType: reportForm.issue,
            location: reportForm.location,
            priorityLevel: reportForm.priority,
            description: reportForm.description,
          }),
        }
      );

      if (response.ok) {
        // Reset form + đóng modal
        setShowReportModal(false);
        setReportForm({
          issue: "",
          description: "",
          priority: "medium",
          location: "",
        });

        // Thông báo thành công
        alert("Báo cáo sự cố đã được gửi thành công!");
      } else {
        const error = await response.json();
        console.error("Lỗi gửi báo cáo:", error);
        alert("Không thể gửi báo cáo. Vui lòng thử lại.");
      }
    } catch (err) {
      console.error("Lỗi mạng:", err);
      alert("Lỗi kết nối đến máy chủ!");
    }
  };

  const translateInvoiceType = (type: string): string => {
    switch (type) {
      case "monthly":
        return "Tiền phòng";
      case "electric":
        return "Tiền điện";
      case "water":
        return "Tiền nước";
      case "service":
        return "Phí dịch vụ";
      case "other":
        return "Khác";
      default:
        return type; // fallback nếu không khớp
    }
  };

  const getDaysLeft = (endDate: string | Date): string => {
    const now = new Date();
    const end = new Date(endDate);
    const diff = Math.ceil(
      (end.getTime() - now.getTime()) / (1000 * 60 * 60 * 24)
    );

    if (diff <= 0) return "Hết hạn";

    if (diff > 30) {
      const months = Math.floor(diff / 30); // 30 ngày = 1 tháng (gần đúng)
      return `${months} tháng`;
    }

    return `${diff} ngày`;
  };

  const room = roomBookingInfo.find((r) => r.userId == user?.idUser);

  const roomNumber = room?.roomNumber;

  const contractByID = Contracts.find(
    (c) => c.idUser === user?.idUser && c.status === "paid"
  );

  // Mock data
  const mockContract =
    contractByID && room
      ? {
          roomNumber: room.roomNumber,
          startDate: contractByID.startDate.toString(),
          endDate: contractByID.endDate.toString(),
          monthlyRent: room.price,
          deposit: contractByID.depositAmount,
          status: "active",
        }
      : null; // hoặc bạn gán giá trị mặc định an toàn

  const daysLeft = getDaysLeft(room?.desiredEnd);

  const review = RoomReviews.find((r) => r.userID == user?.idUser);
  const overall = review?.overallRating || 0;

  const unpaid_invoice = invoiceTenant.filter(
    (i) => i.status == "pending"
  ).length;

  function calculateInvoiceTotal(bill: any): number {
    switch (bill.invoiceType) {
      case "monthly":
      case "full":
        return (
          (bill.rentRooms ?? 0) +
          (bill.electricUsage ?? 0) * 3500 +
          (bill.waterUsage ?? 0) * 25000 +
          (bill.serviceFees ?? 0)
        );
      case "electric":
        return (bill.electricUsage ?? 0) * 3500;
      case "water":
        return (bill.waterUsage ?? 0) * 25000;
      case "service":
        return bill.serviceFees ?? 0;
      default:
        return bill.invoiceAmount ?? 0;
    }
  }

  return (
    <div className="min-h-screen bg-gray-50 flex">
      {/* Sidebar */}
      <aside className="fixed left-0 top-0 h-full w-64 bg-white shadow-lg border-r border-gray-200 z-30">
        {/* Header */}
        <div className="p-6 border-b border-gray-200">
          <div className="flex items-center">
            <div className="h-10 w-10 bg-gradient-to-r from-green-600 to-emerald-600 rounded-xl mr-3 flex items-center justify-center shadow-lg">
              <span className="text-white font-bold text-lg">🏠</span>
            </div>
            <div>
              <h1 className="text-lg font-bold text-gray-900">SmartDorm</h1>
              <p className="text-sm text-gray-500">Dashboard Khách thuê</p>
            </div>
          </div>
        </div>

        {/* Navigation Menu */}
        <nav className="p-4">
          <div className="space-y-2">
            {[
              { id: "overview", label: "Tổng quan", icon: "📊" },
              { id: "search", label: "Tìm phòng", icon: "🔍" },
              { id: "bills", label: "Hóa đơn", icon: "💳" },
              { id: "reports", label: "Báo cáo sự cố", icon: "🔧" },
              { id: "contract", label: "Hợp đồng", icon: "📋" },
              { id: "profile", label: "Hồ sơ", icon: "👤" },
            ].map((tab) => (
              <button
                key={tab.id}
                onClick={() => setActiveTab(tab.id)}
                className={`w-full flex items-center px-4 py-3 text-left rounded-xl font-medium text-sm transition-all duration-200 group ${
                  activeTab === tab.id
                    ? "bg-gradient-to-r from-green-500 to-emerald-500 text-white shadow-lg transform scale-105"
                    : "text-gray-600 hover:bg-gray-50 hover:text-gray-900 hover:scale-105"
                }`}
              >
                <span
                  className={`mr-3 text-lg transition-transform duration-200 ${
                    activeTab === tab.id ? "scale-110" : "group-hover:scale-110"
                  }`}
                >
                  {tab.icon}
                </span>
                <span className="font-medium">{tab.label}</span>
                {activeTab === tab.id && (
                  <div className="ml-auto w-2 h-2 bg-white rounded-full"></div>
                )}
              </button>
            ))}
          </div>
        </nav>

        {/* User Info & Logout */}
        <div className="absolute bottom-0 left-0 right-0 p-4 border-t border-gray-200">
          <div className="mb-3 px-4 py-2 bg-gray-50 rounded-lg">
            <p className="text-sm font-medium text-gray-900">{user?.name}</p>
            <p className="text-xs text-gray-500">{user?.email}</p>
          </div>
          <button
            onClick={handleLogout}
            className="w-full flex items-center px-4 py-3 text-left rounded-xl font-medium text-sm text-red-600 hover:bg-red-50 hover:text-red-700 transition-all duration-200 group"
          >
            <span className="mr-3 text-lg group-hover:scale-110 transition-transform duration-200">
              🚪
            </span>
            <span>Đăng xuất</span>
          </button>
        </div>
      </aside>

      {/* Main Content */}
      <div className="flex-1 ml-64">
        {/* Top Header */}
        <header className="bg-white shadow-sm border-b sticky top-0 z-20">
          <div className="px-6 py-4">
            <div className="flex justify-between items-center">
              <div>
                <h2 className="text-2xl font-bold text-gray-900">
                  {
                    [
                      { id: "overview", label: "Tổng quan" },
                      { id: "search", label: "Tìm phòng" },
                      { id: "bills", label: "Hóa đơn" },
                      { id: "reports", label: "Báo cáo sự cố" },
                      { id: "contract", label: "Hợp đồng" },
                      { id: "profile", label: "Hồ sơ" },
                    ].find((tab) => tab.id === activeTab)?.label
                  }
                </h2>
                <p className="text-gray-600">
                  Quản lý thông tin cá nhân và hoạt động thuê trọ
                </p>
              </div>
              <div className="flex items-center space-x-4">
                <div className="text-sm text-gray-500">
                  {new Date().toLocaleDateString("vi-VN")}
                </div>
              </div>
            </div>
          </div>
        </header>

        {/* Content Area */}
        <main className="p-6">
          {/* Overview Tab */}
          {activeTab === "overview" && (
            <div className="space-y-6">
              {/* Stats Cards */}
              <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
                <div className="bg-white p-6 rounded-lg shadow">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center">
                      <div className="text-3xl mr-4">🏠</div>
                      <div>
                        <p className="text-sm font-medium text-gray-600">
                          Phòng hiện tại
                        </p>
                        <Link
                          href={`/room-details/${roomNumber}`}
                          className="text-2xl font-bold text-blue-600 hover:text-blue-800"
                        >
                          {roomNumber}
                        </Link>
                      </div>
                    </div>
                    <Link
                      href="/rate-room"
                      className="bg-yellow-500 text-white px-3 py-1 rounded text-sm hover:bg-yellow-600 transition-colors"
                    >
                      ⭐ Đánh giá
                    </Link>
                  </div>
                </div>
                <div className="bg-white p-6 rounded-lg shadow">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center">
                      <div className="text-3xl mr-4">💳</div>
                      <div>
                        <p className="text-sm font-medium text-gray-600">
                          Hóa đơn chưa thanh toán
                        </p>
                        <p className="text-2xl font-bold text-yellow-600">
                          {unpaid_invoice}
                        </p>
                      </div>
                    </div>
                    <Link
                      href="/payment-history"
                      className="bg-blue-500 text-white px-3 py-1 rounded text-sm hover:bg-blue-600 transition-colors"
                    >
                      Lịch sử
                    </Link>
                  </div>
                </div>
                <div className="bg-white p-6 rounded-lg shadow">
                  <div className="flex items-center">
                    <div className="text-3xl mr-4">📅</div>
                    <div>
                      <p className="text-sm font-medium text-gray-600">
                        Hợp đồng còn lại
                      </p>
                      <p className="text-2xl font-bold text-blue-600">
                        {daysLeft}
                      </p>
                    </div>
                  </div>
                </div>
                <div className="bg-white p-6 rounded-lg shadow">
                  <div className="flex items-center">
                    <div className="text-3xl mr-4">⭐</div>
                    <div>
                      <p className="text-sm font-medium text-gray-600">
                        Đánh giá phòng
                      </p>
                      <p className="text-2xl font-bold text-green-600">
                        {overall}/5
                      </p>
                    </div>
                  </div>
                </div>
              </div>

              {/* Quick Actions */}
              <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                <div className="bg-white p-6 rounded-lg shadow">
                  <h3 className="text-lg font-semibold text-gray-900 mb-4">
                    Hóa đơn cần thanh toán
                  </h3>
                  <div className="space-y-3">
                    {invoiceTenant
                      .filter((bill) => bill.status === "pending")
                      .map((bill) => (
                        <div
                          key={bill.id}
                          className="flex justify-between items-center p-3 bg-gray-50 rounded"
                        >
                          <div>
                            <p className="font-medium">
                              {translateInvoiceType(bill.invoiceType)} -{" "}
                              {formatDate(bill.invoiceDateLimit)}
                            </p>
                            <p className="text-sm text-gray-600">
                              {bill.invoiceAmount.toLocaleString()}đ
                            </p>
                          </div>
                          <Link
                            href="/payment"
                            className="bg-green-600 text-white px-3 py-1 rounded text-sm hover:bg-green-700 inline-block"
                          >
                            Thanh toán
                          </Link>
                        </div>
                      ))}
                  </div>
                </div>

                <div className="bg-white p-6 rounded-lg shadow">
                  <h3 className="text-lg font-semibold text-gray-900 mb-4">
                    Báo cáo sự cố gần đây
                  </h3>
                  <div className="space-y-3">
                    {maintenanceRequest.map((report) => (
                      <div key={report.id} className="p-3 bg-gray-50 rounded">
                        <div className="flex justify-between items-start mb-2">
                          <p className="font-medium">{report.incidentType}</p>
                          <span
                            className={`px-2 py-1 text-xs rounded-full ${getStatusColor(
                              report.status
                            )}`}
                          >
                            {getStatusText(report.status)}
                          </span>
                        </div>
                        <p className="text-sm text-gray-600">
                          {report.responeFromOwners}
                        </p>
                      </div>
                    ))}
                  </div>
                  <button
                    onClick={() => setShowReportModal(true)}
                    className="mt-4 w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
                  >
                    + Báo cáo sự cố mới
                  </button>
                </div>
              </div>

              {/* Quick Actions */}
              <div className="bg-white p-6 rounded-lg shadow">
                <h3 className="text-lg font-semibold text-gray-900 mb-4">
                  Thao tác nhanh
                </h3>
                <div className="grid grid-cols-2 md:grid-cols-5 gap-4">
                  <Link
                    href="/messages"
                    className="bg-blue-600 text-white p-4 rounded-lg text-center hover:bg-blue-700 transition-colors"
                  >
                    <div className="text-2xl mb-2">💬</div>
                    <div className="font-medium text-sm">Tin nhắn</div>
                  </Link>
                  <Link
                    href="/notifications"
                    className="relative bg-purple-600 text-white p-4 rounded-lg text-center hover:bg-purple-700 transition-colors"
                  >
                    <div>
                      <div className="text-2xl mb-2">🔔</div>
                      <div className="font-medium text-sm">Thông báo</div>
                      <ul>
                        {notifications.map((msg, i) => (
                          <li key={i} className="text-sm text-gray-700">
                            {msg}
                          </li>
                        ))}
                      </ul>
                    </div>

                    {/* Badge số lượng chưa đọc */}
                    {unreadCountNotify > 0 && (
                      <span className="absolute top-1 right-1 inline-flex items-center justify-center px-2 py-1 text-xs font-bold leading-none text-red-100 bg-red-600 rounded-full">
                        {unreadCountNotify}
                      </span>
                    )}
                  </Link>

                  <Link
                    href="/payment-history"
                    className="bg-green-600 text-white p-4 rounded-lg text-center hover:bg-green-700 transition-colors"
                  >
                    <div className="text-2xl mb-2">📊</div>
                    <div className="font-medium text-sm">Lịch sử</div>
                  </Link>
                  <Link
                    href="/rate-room"
                    className="bg-yellow-600 text-white p-4 rounded-lg text-center hover:bg-yellow-700 transition-colors"
                  >
                    <div className="text-2xl mb-2">⭐</div>
                    <div className="font-medium text-sm">Viết đánh giá</div>
                  </Link>
                  <Link
                    href="/tenant-dashboard/reviews"
                    className="bg-orange-600 text-white p-4 rounded-lg text-center hover:bg-orange-700 transition-colors"
                  >
                    <div className="text-2xl mb-2">📝</div>
                    <div className="font-medium text-sm">Đánh giá của tôi</div>
                  </Link>
                </div>
              </div>
            </div>
          )}

          {/* Search Rooms Tab */}
          {activeTab === "search" && (
            <div className="space-y-6">
              {/* Search Header & Stats */}
              <div className="bg-white p-6 rounded-lg shadow">
                <div className="flex justify-between items-center mb-4">
                  <h2 className="text-2xl font-bold text-gray-900">
                    Danh sách phòng trọ
                  </h2>
                  <div className="text-sm text-gray-600">
                    Hiển thị {filteredRooms.length} / {rooms.length} phòng
                  </div>
                </div>

                {/* Room Status Stats */}
                <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6">
                  <div className="bg-green-50 p-4 rounded-lg text-center">
                    <div className="text-2xl font-bold text-green-600">
                      {rooms.filter((r) => r.status === "available").length}
                    </div>
                    <div className="text-sm text-green-700">Phòng trống</div>
                  </div>
                  <div className="bg-blue-50 p-4 rounded-lg text-center">
                    <div className="text-2xl font-bold text-blue-600">
                      {rooms.filter((r) => r.status === "occupied").length}
                    </div>
                    <div className="text-sm text-blue-700">Đã thuê</div>
                  </div>
                  <div className="bg-yellow-50 p-4 rounded-lg text-center">
                    <div className="text-2xl font-bold text-yellow-600">
                      {rooms.filter((r) => r.status === "maintenance").length}
                    </div>
                    <div className="text-sm text-yellow-700">Bảo trì</div>
                  </div>
                  <div className="bg-gray-50 p-4 rounded-lg text-center">
                    <div className="text-2xl font-bold text-gray-600">
                      {rooms.length}
                    </div>
                    <div className="text-sm text-gray-700">Tổng phòng</div>
                  </div>
                </div>

                {/* Search Filters */}
                <div className="grid grid-cols-1 md:grid-cols-6 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Trạng thái
                    </label>
                    <select
                      value={roomFilters.status}
                      onChange={(e) =>
                        handleFilterChange("status", e.target.value)
                      }
                      className="form-input w-full border border-gray-300 rounded-md px-3 py-2 text-gray-900"
                    >
                      <option value="all">Tất cả</option>
                      <option value="available">Phòng trống</option>
                      <option value="occupied">Đã thuê</option>
                      <option value="maintenance">Bảo trì</option>
                    </select>
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Giá từ
                    </label>
                    <input
                      type="number"
                      value={roomFilters.minPrice}
                      onChange={(e) =>
                        handleFilterChange("minPrice", e.target.value)
                      }
                      className="form-input w-full border border-gray-300 rounded-md px-3 py-2 text-gray-900 placeholder-gray-500"
                      placeholder="2,000,000"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Giá đến
                    </label>
                    <input
                      type="number"
                      value={roomFilters.maxPrice}
                      onChange={(e) =>
                        handleFilterChange("maxPrice", e.target.value)
                      }
                      className="form-input w-full border border-gray-300 rounded-md px-3 py-2 text-gray-900 placeholder-gray-500"
                      placeholder="5,000,000"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Diện tích tối thiểu
                    </label>
                    <input
                      type="number"
                      value={roomFilters.minArea}
                      onChange={(e) =>
                        handleFilterChange("minArea", e.target.value)
                      }
                      className="form-input w-full border border-gray-300 rounded-md px-3 py-2 text-gray-900 placeholder-gray-500"
                      placeholder="25"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Tìm kiếm
                    </label>
                    <input
                      type="text"
                      value={roomFilters.searchTerm}
                      onChange={(e) =>
                        handleFilterChange("searchTerm", e.target.value)
                      }
                      className="form-input w-full border border-gray-300 rounded-md px-3 py-2 text-gray-900 placeholder-gray-500"
                      placeholder="Số phòng, tiện nghi..."
                    />
                  </div>
                  <div className="flex items-end">
                    <button
                      onClick={() =>
                        setRoomFilters({
                          status: "all",
                          minPrice: "",
                          maxPrice: "",
                          minArea: "",
                          searchTerm: "",
                        })
                      }
                      className="w-full bg-gray-600 text-white py-2 rounded-md hover:bg-gray-700"
                    >
                      Xóa bộ lọc
                    </button>
                  </div>
                </div>
              </div>

              {/* All Rooms */}
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {filteredRooms.map((room) => (
                  <div
                    key={room.id}
                    className="bg-white rounded-lg shadow overflow-hidden border border-gray-200 hover:shadow-lg transition-shadow"
                  >
                    <div className="h-48 bg-gray-200 flex items-center justify-center relative">
                      <span className="text-gray-500">
                        Hình ảnh phòng {room.number}
                      </span>
                      {/* Status Badge */}
                      <div className="absolute top-3 right-3">
                        <span
                          className={`px-3 py-1 text-xs font-medium rounded-full ${getRoomStatusColor(
                            room.status
                          )}`}
                        >
                          {getStatusText(room.status)}
                        </span>
                      </div>
                    </div>
                    <div className="p-4">
                      <div className="flex justify-between items-start mb-2">
                        <h3 className="text-lg font-semibold text-gray-900">
                          Phòng {room.roomNumber}
                        </h3>
                        <div className="text-right">
                          <p className="text-2xl font-bold text-green-600">
                            {room.price.toLocaleString()}đ
                          </p>
                          <p className="text-sm text-gray-500">/tháng</p>
                        </div>
                      </div>

                      <p className="text-gray-600 mb-3">
                        Diện tích: {room.area}m²
                      </p>

                      {/* Tenant Info for Occupied Rooms */}
                      {room.status === "occupied" && room.tenant && (
                        <div className="mb-3 p-2 bg-blue-50 rounded-lg">
                          <p className="text-sm text-blue-800">
                            <span className="font-medium">Khách thuê:</span>{" "}
                            {room.tenant}
                          </p>
                          <p className="text-xs text-blue-600">
                            Từ: {room.moveInDate}
                          </p>
                        </div>
                      )}

                      {/* Maintenance Info */}
                      {room.status === "maintenance" &&
                        room.maintenanceReason && (
                          <div className="mb-3 p-2 bg-yellow-50 rounded-lg">
                            <p className="text-sm text-yellow-800">
                              <span className="font-medium">
                                Lý do bảo trì:
                              </span>{" "}
                              {room.maintenanceReason}
                            </p>
                          </div>
                        )}

                      <div className="mb-4">
                        <p className="text-sm font-medium text-gray-700 mb-1">
                          Tiện nghi:
                        </p>
                        <div className="flex flex-wrap gap-1">
                          {room.amenities.map(
                            (amenity: string, index: number) => (
                              <span
                                key={index}
                                className="bg-blue-100 text-blue-800 text-xs px-2 py-1 rounded"
                              >
                                {amenity}
                              </span>
                            )
                          )}
                        </div>
                      </div>

                      <div className="flex space-x-2">
                        {room.status === "available" ? (
                          <>
                            <Link
                              href={`/book-room?roomID=${room.id}`}
                              className="flex-1 bg-green-600 text-white py-2 rounded hover:bg-green-700 text-center font-medium"
                            >
                              Đặt phòng
                            </Link>
                            <Link
                              href={`/room-details/${room.number}`}
                              className="flex-1 border border-green-600 text-green-600 py-2 rounded hover:bg-green-50 text-center font-medium"
                            >
                              Xem chi tiết
                            </Link>
                          </>
                        ) : room.status === "occupied" ? (
                          <>
                            <Link
                              href={`/room-details/${room.number}`}
                              className="flex-1 border border-blue-600 text-blue-600 py-2 rounded hover:bg-blue-50 text-center font-medium"
                            >
                              Xem chi tiết
                            </Link>
                            <button
                              disabled
                              className="flex-1 bg-gray-300 text-gray-500 py-2 rounded cursor-not-allowed font-medium"
                            >
                              Đã thuê
                            </button>
                          </>
                        ) : (
                          <>
                            <Link
                              href={`/room-details/${room.number}`}
                              className="flex-1 border border-yellow-600 text-yellow-600 py-2 rounded hover:bg-yellow-50 text-center font-medium"
                            >
                              Xem chi tiết
                            </Link>
                            <button
                              disabled
                              className="flex-1 bg-gray-300 text-gray-500 py-2 rounded cursor-not-allowed font-medium"
                            >
                              Bảo trì
                            </button>
                          </>
                        )}
                      </div>
                    </div>
                  </div>
                ))}
              </div>

              {/* Empty State */}
              {filteredRooms.length === 0 && (
                <div className="text-center py-12">
                  <div className="text-6xl mb-4">🔍</div>
                  <h3 className="text-lg font-medium text-gray-900 mb-2">
                    Không tìm thấy phòng nào
                  </h3>
                  <p className="text-gray-600 mb-4">
                    Thử điều chỉnh bộ lọc để tìm phòng phù hợp
                  </p>
                  <button
                    onClick={() =>
                      setRoomFilters({
                        status: "all",
                        minPrice: "",
                        maxPrice: "",
                        minArea: "",
                        searchTerm: "",
                      })
                    }
                    className="bg-green-600 text-white px-6 py-2 rounded-lg hover:bg-green-700"
                  >
                    Xóa tất cả bộ lọc
                  </button>
                </div>
              )}
            </div>
          )}

          {/* Bills Tab */}
          {activeTab === "bills" && (
            <div className="space-y-6">
              <h2 className="text-2xl font-bold text-gray-900">
                Hóa đơn của tôi
              </h2>

              <div className="bg-white shadow rounded-lg overflow-hidden">
                <table className="min-w-full divide-y divide-gray-200">
                  <thead className="bg-gray-50">
                    <tr>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                        Loại hóa đơn
                      </th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                        Tháng
                      </th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                        Số tiền
                      </th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                        Hạn thanh toán
                      </th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                        Trạng thái
                      </th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                        Thao tác
                      </th>
                    </tr>
                  </thead>
                  <tbody className="bg-white divide-y divide-gray-200">
                    {invoiceTenant.map((bill) => (
                      <tr key={bill.id}>
                        <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                          {translateInvoiceType(bill.invoiceType)}
                        </td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                          {formatDate(bill.invoiceDateLimit)}
                        </td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                          {calculateInvoiceTotal(bill).toLocaleString()}đ
                        </td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                          {formatDateTime(bill.invoiceDateLimit)}
                        </td>
                        <td className="px-6 py-4 whitespace-nowrap">
                          <span
                            className={`px-2 py-1 text-xs rounded-full ${getStatusColor(
                              bill.status
                            )}`}
                          >
                            {getStatusText(bill.status)}
                          </span>
                        </td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                          {bill.status === "pending" ? (
                            <Link
                              href={`/payment/${bill.id}`}
                              className="bg-green-600 text-white px-3 py-1 rounded text-sm hover:bg-green-700 inline-block"
                            >
                              Thanh toán
                            </Link>
                          ) : (
                            <Link
                              href={`/receipt/${bill.id}`}
                              className="text-blue-600 hover:text-blue-900"
                            >
                              Xem biên lai
                            </Link>
                          )}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Contract Tab */}
          {activeTab === "contract" && (
            <div className="space-y-6">
              <h2 className="text-2xl font-bold text-gray-900">
                Hợp đồng thuê phòng
              </h2>

              <div className="bg-white p-6 rounded-lg shadow">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div>
                    <h3 className="text-lg font-semibold text-gray-900 mb-4">
                      Thông tin hợp đồng
                    </h3>
                    <div className="space-y-3">
                      <div>
                        <span className="text-sm font-medium text-gray-600">
                          Phòng số:
                        </span>
                        <span className="ml-2 text-sm text-gray-900">
                          {mockContract ? mockContract.roomNumber : ""}
                        </span>
                      </div>
                      <div>
                        <span className="text-sm font-medium text-gray-600">
                          Ngày bắt đầu:
                        </span>
                        <span className="ml-2 text-sm text-gray-900">
                          {formatDateTime(mockContract?.startDate)}
                        </span>
                      </div>
                      <div>
                        <span className="text-sm font-medium text-gray-600">
                          Ngày kết thúc:
                        </span>
                        <span className="ml-2 text-sm text-gray-900">
                          {formatDateTime(mockContract?.endDate)}
                        </span>
                      </div>
                      <div>
                        <span className="text-sm font-medium text-gray-600">
                          Tiền thuê hàng tháng:
                        </span>
                        <span className="ml-2 text-sm text-gray-900">
                          {mockContract?.monthlyRent.toLocaleString()}đ
                        </span>
                      </div>
                      <div>
                        <span className="text-sm font-medium text-gray-600">
                          Tiền cọc:
                        </span>
                        <span className="ml-2 text-sm text-gray-900">
                          {mockContract?.deposit.toLocaleString()}đ
                        </span>
                      </div>
                      <div>
                        <span className="text-sm font-medium text-gray-600">
                          Trạng thái:
                        </span>
                        <span
                          className={`ml-2 px-2 py-1 text-xs rounded-full ${getStatusColor(
                            mockContract?.status ?? ""
                          )}`}
                        >
                          {getStatusText(mockContract?.status ?? "")}
                        </span>
                      </div>
                    </div>
                  </div>
                  <div>
                    <h3 className="text-lg font-semibold text-gray-900 mb-4">
                      Thao tác
                    </h3>
                    <div className="space-y-3">
                      <button className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700">
                        Tải hợp đồng PDF
                      </button>
                      <button className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700">
                        Yêu cầu gia hạn
                      </button>
                      <button className="w-full bg-red-600 text-white py-2 rounded hover:bg-red-700">
                        Yêu cầu trả phòng
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* Reports Tab */}
          {activeTab === "reports" && (
            <div className="space-y-6">
              <div className="flex justify-between items-center">
                <h2 className="text-2xl font-bold text-gray-900">
                  Báo cáo sự cố
                </h2>
                <button
                  onClick={() => setShowReportModal(true)}
                  className="bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700 flex items-center"
                >
                  <span className="mr-2">+</span>
                  Báo cáo sự cố mới
                </button>
              </div>

              <div className="bg-white shadow rounded-lg overflow-hidden">
                <table className="min-w-full divide-y divide-gray-200">
                  <thead className="bg-gray-50">
                    <tr>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                        Vấn đề
                      </th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                        Ngày báo cáo
                      </th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                        Mức độ
                      </th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                        Trạng thái
                      </th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                        Phản hồi
                      </th>
                    </tr>
                  </thead>
                  <tbody className="bg-white divide-y divide-gray-200">
                    {maintenanceRequest.map((report) => (
                      <tr key={report.id}>
                        <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                          {report.incidentType}
                        </td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                          {formatDateTime(report.requestDate)}
                        </td>
                        <td className="px-6 py-4 whitespace-nowrap">
                          <span
                            className={`px-2 py-1 text-xs rounded-full ${
                              report.priorityLevel === "high"
                                ? "bg-red-100 text-red-800"
                                : report.priorityLevel === "medium"
                                ? "bg-yellow-100 text-yellow-800"
                                : "bg-green-100 text-green-800"
                            }`}
                          >
                            {report.priorityLevel === "high"
                              ? "Cao"
                              : report.priorityLevel === "medium"
                              ? "Trung bình"
                              : "Thấp"}
                          </span>
                        </td>
                        <td className="px-6 py-4 whitespace-nowrap">
                          <span
                            className={`px-2 py-1 text-xs rounded-full ${getStatusColor(
                              report.status
                            )}`}
                          >
                            {getStatusText(report.status)}
                          </span>
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-900 max-w-xs truncate">
                          {report.responeFromOwners || "Chưa có phản hồi"}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {activeTab === "profile" && (
            <div className="space-y-6">
              <h2 className="text-2xl font-bold text-gray-900">
                Hồ sơ cá nhân
              </h2>

              <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                {/* Profile Card */}
                <div className="lg:col-span-1">
                  <div className="bg-white p-6 rounded-2xl shadow-lg border border-gray-100">
                    <div className="text-center">
                      <div className="w-24 h-24 bg-gradient-to-r from-green-400 to-blue-500 rounded-full mx-auto mb-4 flex items-center justify-center text-white text-3xl font-bold">
                        A
                      </div>
                      <h3 className="text-xl font-bold text-gray-900 mb-2">
                        {user?.name}
                      </h3>
                      <p className="text-gray-600 mb-4">Khách thuê</p>
                      <div className="flex items-center justify-center text-sm text-gray-500 mb-4">
                        <span className="w-2 h-2 bg-green-500 rounded-full mr-2"></span>
                        Đang thuê phòng 101
                      </div>
                      <button className="w-full bg-green-600 text-white py-2 rounded-lg hover:bg-green-700 transition-colors">
                        Chỉnh sửa ảnh đại diện
                      </button>
                    </div>
                  </div>
                </div>

                {/* Profile Information */}
                <div className="lg:col-span-2">
                  <div className="bg-white p-6 rounded-2xl shadow-lg border border-gray-100">
                    <div className="flex justify-between items-center mb-6">
                      <h3 className="text-lg font-semibold text-gray-900">
                        Thông tin cá nhân
                      </h3>
                      <button className="text-green-600 hover:text-green-700 font-medium">
                        Chỉnh sửa
                      </button>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                      <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2">
                          Họ và tên
                        </label>
                        <input
                          type="text"
                          value={user?.name}
                          className="form-input w-full border border-gray-300 rounded-lg px-3 py-2 bg-gray-50 text-gray-900"
                          readOnly
                        />
                      </div>
                      <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2">
                          Email
                        </label>
                        <input
                          type="email"
                          value={user?.email}
                          className="form-input w-full border border-gray-300 rounded-lg px-3 py-2 bg-gray-50 text-gray-900"
                          readOnly
                        />
                      </div>
                      <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2">
                          Số điện thoại
                        </label>
                        <input
                          type="tel"
                          value={user?.phone}
                          className="form-input w-full border border-gray-300 rounded-lg px-3 py-2 bg-gray-50 text-gray-900"
                          readOnly
                        />
                      </div>
                      <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2">
                          CCCD/CMND
                        </label>
                        <input
                          type="text"
                          value={user?.idNumber}
                          className="w-full border border-gray-300 rounded-lg px-3 py-2 bg-gray-50"
                          readOnly
                        />
                      </div>
                      <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2">
                          Ngày sinh
                        </label>
                        <input
                          type="date"
                          value={user?.dob}
                          className="w-full border border-gray-300 rounded-lg px-3 py-2 bg-gray-50"
                          readOnly
                        />
                      </div>
                      <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2">
                          Giới tính
                        </label>
                        <select
                          className="w-full border border-gray-300 rounded-lg px-3 py-2 bg-gray-50"
                          disabled
                        >
                          <option>{user?.gender}</option>
                        </select>
                      </div>
                      <div className="md:col-span-2">
                        <label className="block text-sm font-medium text-gray-700 mb-2">
                          Địa chỉ thường trú
                        </label>
                        <input
                          type="text"
                          value={user?.address}
                          className="w-full border border-gray-300 rounded-lg px-3 py-2 bg-gray-50"
                          readOnly
                        />
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              {/* Rental Information */}
              <div className="bg-white p-6 rounded-2xl shadow-lg border border-gray-100">
                <h3 className="text-lg font-semibold text-gray-900 mb-6">
                  Thông tin thuê trọ
                </h3>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                  <div className="bg-blue-50 p-4 rounded-xl text-center">
                    <div className="text-2xl mb-2">🏠</div>
                    <div className="text-sm text-gray-600 mb-1">
                      Phòng hiện tại
                    </div>
                    <div className="font-semibold text-gray-900">Phòng 101</div>
                  </div>
                  <div className="bg-green-50 p-4 rounded-xl text-center">
                    <div className="text-2xl mb-2">📅</div>
                    <div className="text-sm text-gray-600 mb-1">
                      Ngày bắt đầu thuê
                    </div>
                    <div className="font-semibold text-gray-900">
                      01/01/2024
                    </div>
                  </div>
                  <div className="bg-purple-50 p-4 rounded-xl text-center">
                    <div className="text-2xl mb-2">💰</div>
                    <div className="text-sm text-gray-600 mb-1">
                      Tiền thuê/tháng
                    </div>
                    <div className="font-semibold text-gray-900">
                      3,500,000đ
                    </div>
                  </div>
                </div>
              </div>

              {/* Emergency Contact */}
              <div className="bg-white p-6 rounded-2xl shadow-lg border border-gray-100">
                <div className="flex justify-between items-center mb-6">
                  <h3 className="text-lg font-semibold text-gray-900">
                    Liên hệ khẩn cấp
                  </h3>
                  <button className="text-green-600 hover:text-green-700 font-medium">
                    Chỉnh sửa
                  </button>
                </div>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Tên người liên hệ
                    </label>
                    <input
                      type="text"
                      value="Nguyễn Văn B (Anh trai)"
                      className="form-input w-full border border-gray-300 rounded-lg px-3 py-2 bg-gray-50 text-gray-900"
                      readOnly
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Số điện thoại
                    </label>
                    <input
                      type="tel"
                      value="0987654321"
                      className="form-input w-full border border-gray-300 rounded-lg px-3 py-2 bg-gray-50 text-gray-900"
                      readOnly
                    />
                  </div>
                </div>
              </div>

              {/* Account Settings */}
              <div className="bg-white p-6 rounded-2xl shadow-lg border border-gray-100">
                <h3 className="text-lg font-semibold text-gray-900 mb-6">
                  Cài đặt tài khoản
                </h3>
                <div className="space-y-4">
                  <div className="flex items-center justify-between p-4 bg-gray-50 rounded-xl">
                    <div>
                      <div className="font-medium text-gray-900">
                        Thông báo email
                      </div>
                      <div className="text-sm text-gray-600">
                        Nhận thông báo về hóa đơn và sự cố
                      </div>
                    </div>
                    <label className="relative inline-flex items-center cursor-pointer">
                      <input
                        type="checkbox"
                        className="sr-only peer"
                        defaultChecked
                      />
                      <div className="w-11 h-6 bg-gray-200 peer-focus:outline-none peer-focus:ring-4 peer-focus:ring-green-300 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-green-600"></div>
                    </label>
                  </div>
                  <div className="flex items-center justify-between p-4 bg-gray-50 rounded-xl">
                    <div>
                      <div className="font-medium text-gray-900">
                        Thông báo SMS
                      </div>
                      <div className="text-sm text-gray-600">
                        Nhận tin nhắn về thanh toán
                      </div>
                    </div>
                    <label className="relative inline-flex items-center cursor-pointer">
                      <input type="checkbox" className="sr-only peer" />
                      <div className="w-11 h-6 bg-gray-200 peer-focus:outline-none peer-focus:ring-4 peer-focus:ring-green-300 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-green-600"></div>
                    </label>
                  </div>
                  <div className="pt-4 border-t border-gray-200">
                    <button className="w-full bg-red-600 text-white py-2 rounded-lg hover:bg-red-700 transition-colors">
                      Đổi mật khẩu
                    </button>
                  </div>
                </div>
              </div>
            </div>
          )}
        </main>
      </div>

      {/* Report Modal */}
      {showReportModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
          <div className="bg-white rounded-2xl max-w-md w-full max-h-[90vh] overflow-y-auto">
            <div className="p-6 border-b border-gray-200">
              <h3 className="text-xl font-bold text-gray-900">
                Báo cáo sự cố mới
              </h3>
              <p className="text-gray-600 mt-1">
                Mô tả chi tiết vấn đề bạn gặp phải
              </p>
            </div>
            <form onSubmit={handleSubmitReport} className="p-6">
              <div className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Loại sự cố *
                  </label>
                  <select
                    value={reportForm.issue}
                    onChange={(e) =>
                      setReportForm({ ...reportForm, issue: e.target.value })
                    }
                    className="form-input w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-green-500 focus:border-green-500 text-gray-900"
                    required
                  >
                    <option value="">Chọn loại sự cố</option>
                    <option value="Máy lạnh không hoạt động">
                      Máy lạnh không hoạt động
                    </option>
                    <option value="Vòi nước bị rò rỉ">Vòi nước bị rò rỉ</option>
                    <option value="Bóng đèn hỏng">Bóng đèn hỏng</option>
                    <option value="Ổ cắm điện hỏng">Ổ cắm điện hỏng</option>
                    <option value="Cửa sổ/cửa ra vào hỏng">
                      Cửa sổ/cửa ra vào hỏng
                    </option>
                    <option value="Internet không hoạt động">
                      Internet không hoạt động
                    </option>
                    <option value="Vấn đề vệ sinh">Vấn đề vệ sinh</option>
                    <option value="Khác">Khác</option>
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Vị trí
                  </label>
                  <input
                    type="text"
                    value={reportForm.location}
                    onChange={(e) =>
                      setReportForm({ ...reportForm, location: e.target.value })
                    }
                    className="form-input w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-green-500 focus:border-green-500 text-gray-900 placeholder-gray-500"
                    placeholder="VD: Phòng ngủ, Phòng tắm, Ban công..."
                  />
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Mức độ ưu tiên
                  </label>
                  <select
                    value={reportForm.priority}
                    onChange={(e) =>
                      setReportForm({ ...reportForm, priority: e.target.value })
                    }
                    className="form-input w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-green-500 focus:border-green-500 text-gray-900"
                  >
                    <option value="low">Thấp - Không ảnh hưởng nhiều</option>
                    <option value="medium">Trung bình - Cần xử lý sớm</option>
                    <option value="high">Cao - Cần xử lý ngay</option>
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Mô tả chi tiết *
                  </label>
                  <textarea
                    value={reportForm.description}
                    onChange={(e) =>
                      setReportForm({
                        ...reportForm,
                        description: e.target.value,
                      })
                    }
                    className="form-input w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-green-500 focus:border-green-500 text-gray-900 placeholder-gray-500"
                    rows={4}
                    placeholder="Mô tả chi tiết về vấn đề, thời gian xảy ra, mức độ ảnh hưởng..."
                    required
                  />
                </div>
              </div>

              <div className="flex space-x-3 mt-6">
                <button
                  type="button"
                  onClick={() => setShowReportModal(false)}
                  className="flex-1 border border-gray-300 text-gray-700 py-2 rounded-lg hover:bg-gray-50 transition-colors"
                >
                  Hủy
                </button>
                <button
                  type="submit"
                  className="flex-1 bg-green-600 text-white py-2 rounded-lg hover:bg-green-700 transition-colors"
                >
                  Gửi báo cáo
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

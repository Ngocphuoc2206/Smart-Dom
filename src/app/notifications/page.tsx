"use client";

import Link from "next/link";
import { useEffect, useState } from "react";
import {
  ArrowLeftIcon,
  BellIcon,
  CheckIcon,
  ExclamationTriangleIcon,
  InformationCircleIcon,
  CurrencyDollarIcon,
  WrenchScrewdriverIcon,
  HomeIcon,
  TrashIcon,
} from "@heroicons/react/24/outline";
import { getNotificationByUserID } from "../hooks/useNotificationByID";
import { useAuth } from "@/contexts/AuthContext";

interface Notification {
  id: string;
  type: "bill" | "maintenance" | "announcement" | "reminder" | "system";
  title: string;
  message: string;
  time: string;
  read: boolean;
  priority: "low" | "medium" | "high";
  actionUrl?: string;
}

interface RawNotification {
  id: number;
  userId: number;
  typeNotify: string;
  title: string;
  message: string;
  createdAt: string;
  isRead: boolean;
  priority: "low" | "medium" | "high";
}

export default function NotificationsPage() {
  const [notifications, setNotifications] = useState<RawNotification[]>([]);
  const { user } = useAuth();
  const [filter, setFilter] = useState<
    "all" | "unread" | "bill" | "maintenance"
  >("all");

  const removeDuplicateNotifications = (data: RawNotification[]) => {
    const uniqueMap = new Map<number, RawNotification>();
    data.forEach((item) => {
      if (!uniqueMap.has(item.id)) {
        uniqueMap.set(item.id, item); // Giữ bản đầu tiên nếu trùng id
      }
    });
    return Array.from(uniqueMap.values());
  };

  const formatDate = (dateString: any) => {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0"); // Tháng bắt đầu từ 0
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
  };

  const getIcon = (type: string) => {
    switch (type) {
      case "bill":
        return <CurrencyDollarIcon className="h-6 w-6" />;
      case "maintenance":
        return <WrenchScrewdriverIcon className="h-6 w-6" />;
      case "announcement":
        return <InformationCircleIcon className="h-6 w-6" />;
      case "reminder":
        return <ExclamationTriangleIcon className="h-6 w-6" />;
      case "system":
        return <HomeIcon className="h-6 w-6" />;
      default:
        return <BellIcon className="h-6 w-6" />;
    }
  };

  useEffect(() => {
    getNotificationByUserID(user?.idUser).then((data) => {
      const cleaned = removeDuplicateNotifications(data);
      setNotifications(cleaned);
    });
  }, []);

  const getIconColor = (type: string, priority: string) => {
    if (priority === "high") return "text-red-600 bg-red-100";
    if (priority === "medium") return "text-yellow-600 bg-yellow-100";

    switch (type) {
      case "bill":
        return "text-green-600 bg-green-100";
      case "maintenance":
        return "text-blue-600 bg-blue-100";
      case "announcement":
        return "text-purple-600 bg-purple-100";
      case "reminder":
        return "text-orange-600 bg-orange-100";
      case "system":
        return "text-gray-600 bg-gray-100";
      default:
        return "text-gray-600 bg-gray-100";
    }
  };

  const markAsRead = async (id: string) => {
    setNotifications((prev) =>
      prev.map((notif) =>
        notif.id.toString() === id ? { ...notif, read: true } : notif
      )
    );
  };
  const fetchNoti = async () => {
    const data = await getNotificationByUserID(user?.idUser);
    setNotifications(data);
  };

  const markAllAsRead = async () => {
    try {
      const response = await fetch(
        `https://localhost:7257/api/Notification/update/${user?.idUser}`,
        {
          method: "PUT", // hoặc PATCH
        }
      );

      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
    } catch (error) {
      console.error("Error fetching expired status:", error);
    }
    fetchNoti();
    setNotifications((prev) => prev.map((notif) => ({ ...notif, read: true })));
  };

  const deleteNotification = (id: string) => {
    setNotifications((prev) =>
      prev.filter((notif) => notif.id.toString() !== id)
    );
  };

  const filteredNotifications = notifications.filter((notif) => {
    if (filter === "all") return true;
    if (filter === "unread") return !notif.isRead;
    return notif.typeNotify === filter;
  });

  const unreadCount = notifications.filter((n) => !n.isRead).length;

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white shadow-sm border-b">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center py-4">
            <div className="flex items-center">
              <Link
                href="/tenant-dashboard"
                className="flex items-center text-gray-600 hover:text-gray-900"
              >
                <ArrowLeftIcon className="h-5 w-5 mr-2" />
                <div className="h-8 w-8 bg-green-600 rounded mr-3 flex items-center justify-center">
                  <span className="text-white font-bold">🏠</span>
                </div>
                <span className="text-xl font-bold text-gray-900">
                  SmartDorm
                </span>
              </Link>
              <span className="mx-3 text-gray-400">/</span>
              <span className="text-gray-600">Thông báo</span>
            </div>
            <div className="flex items-center space-x-3">
              <div className="flex items-center text-sm text-gray-600">
                <BellIcon className="h-4 w-4 mr-1" />
                {unreadCount} chưa đọc
              </div>
              {unreadCount > 0 && (
                <button
                  onClick={markAllAsRead}
                  className="text-sm text-blue-600 hover:text-blue-800"
                >
                  Đánh dấu tất cả đã đọc
                </button>
              )}
            </div>
          </div>
        </div>
      </header>

      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Filters */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-4 mb-6">
          <div className="flex flex-wrap gap-2">
            {[
              { key: "all", label: "Tất cả", count: notifications.length },
              { key: "unread", label: "Chưa đọc", count: unreadCount },
              {
                key: "bill",
                label: "Hóa đơn",
                count: notifications.filter((n) => n.typeNotify === "bill")
                  .length,
              },
              {
                key: "maintenance",
                label: "Bảo trì",
                count: notifications.filter(
                  (n) => n.typeNotify === "maintenance"
                ).length,
              },
            ].map((filterOption) => (
              <button
                key={filterOption.key}
                onClick={() => setFilter(filterOption.key as any)}
                className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
                  filter === filterOption.key
                    ? "bg-green-600 text-white"
                    : "bg-gray-100 text-gray-700 hover:bg-gray-200"
                }`}
              >
                {filterOption.label} ({filterOption.count})
              </button>
            ))}
          </div>
        </div>

        {/* Notifications List */}
        <div className="space-y-4">
          {filteredNotifications.length === 0 ? (
            <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-8 text-center">
              <BellIcon className="h-12 w-12 text-gray-400 mx-auto mb-4" />
              <h3 className="text-lg font-medium text-gray-900 mb-2">
                Không có thông báo
              </h3>
              <p className="text-gray-600">
                {filter === "unread"
                  ? "Bạn đã đọc hết tất cả thông báo!"
                  : "Chưa có thông báo nào trong danh mục này."}
              </p>
            </div>
          ) : (
            filteredNotifications.map((notification, index) => (
              <div
                key={`${notification.id}-${notification.createdAt || index}`}
                className={`bg-white rounded-lg shadow-sm border border-gray-200 p-6 transition-all hover:shadow-md ${
                  !notification.isRead ? "border-l-4 border-l-green-500" : ""
                }`}
              >
                <div className="flex items-start space-x-4">
                  <div
                    className={`p-2 rounded-lg ${getIconColor(
                      notification.typeNotify,
                      notification.priority
                    )}`}
                  >
                    {getIcon(notification.typeNotify)}
                  </div>

                  <div className="flex-1 min-w-0">
                    <div className="flex items-start justify-between">
                      <div className="flex-1">
                        <h3
                          className={`text-lg font-medium ${
                            !notification.isRead
                              ? "text-gray-900"
                              : "text-gray-700"
                          }`}
                        >
                          {notification.title}
                          {!notification.isRead && (
                            <span className="ml-2 inline-block w-2 h-2 bg-green-500 rounded-full"></span>
                          )}
                        </h3>
                        <p className="text-gray-600 mt-1">
                          {notification.message}
                        </p>
                        <p className="text-sm text-gray-500 mt-2">
                          {formatDate(notification.createdAt)}
                        </p>
                      </div>

                      <div className="flex items-center space-x-2 ml-4">
                        {notification.priority === "high" && (
                          <span className="px-2 py-1 text-xs bg-red-100 text-red-800 rounded-full">
                            Quan trọng
                          </span>
                        )}
                        {!notification.isRead && (
                          <button
                            onClick={() =>
                              markAsRead(notification.id.toString())
                            }
                            className="p-1 text-gray-400 hover:text-green-600 transition-colors"
                            title="Đánh dấu đã đọc"
                          >
                            <CheckIcon className="h-4 w-4" />
                          </button>
                        )}
                        <button
                          onClick={() =>
                            deleteNotification(notification.id.toString())
                          }
                          className="p-1 text-gray-400 hover:text-red-600 transition-colors"
                          title="Xóa thông báo"
                        >
                          <TrashIcon className="h-4 w-4" />
                        </button>
                      </div>
                    </div>

                    {/* {notification.actionUrl && (
                      <div className="mt-4">
                        <Link
                          href={notification.actionUrl}
                          className="inline-flex items-center px-4 py-2 bg-green-600 text-white text-sm font-medium rounded-lg hover:bg-green-700 transition-colors"
                        >
                          Xem chi tiết
                        </Link>
                      </div>
                    )} */}
                  </div>
                </div>
              </div>
            ))
          )}
        </div>

        {/* Back to Dashboard */}
        <div className="mt-8 text-center">
          <Link
            href="/tenant-dashboard"
            className="inline-flex items-center px-6 py-3 bg-gray-600 text-white font-medium rounded-lg hover:bg-gray-700 transition-colors"
          >
            <ArrowLeftIcon className="h-5 w-5 mr-2" />
            Quay lại Dashboard
          </Link>
        </div>
      </div>
    </div>
  );
}

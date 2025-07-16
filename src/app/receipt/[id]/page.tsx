"use client";

import Link from "next/link";
import { useParams } from "next/navigation";
import { useState, useEffect } from "react";
import {
  ArrowLeftIcon,
  PrinterIcon,
  DocumentArrowDownIcon,
  CheckCircleIcon,
  CalendarIcon,
  HomeIcon,
  UserIcon,
  ClockIcon,
  CreditCardIcon,
} from "@heroicons/react/24/outline";

import { getRoomBookingInfo } from "@/app/hooks/useRoomBookingInfo";
import { getInvoiceByUserID } from "@/app/hooks/useInvoiceID";
import { useAuth } from "@/contexts/AuthContext";

export default function ReceiptPage() {
  const params = useParams();
  const receiptId = params.id as string;
  const [RoomBookings, setRoomBookings] = useState<any[]>([]);
  const [invoices, setInvoices] = useState<any[]>([]);
  const [invoice, setInvoice] = useState<any>(null);
  const { user } = useAuth();

  useEffect(() => {
    getRoomBookingInfo().then(setRoomBookings);
    if (user && user.idUser !== undefined) {
      getInvoiceByUserID(user.idUser.toString()).then(setInvoices);
    }
  }, []);

  useEffect(() => {
    if (user && invoices.length > 0) {
      const found = invoices.find(
        (i) => i.userId === user.idUser && i.id.toString() === receiptId
      );
      setInvoice(found);
    }
  }, [user, invoices, receiptId]);

  const formatDate = (dateString: any) => {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
  };

  const roomBookingInfo = RoomBookings.find((r) => r.userId === user?.idUser);

  const receiptDetails = [];

  if (invoice?.invoiceType === "monthly") {
    receiptDetails.push({
      description: `Tiền thuê phòng tháng ${formatDate(
        invoice.invoiceDateLimit
      )}`,
      amount: invoice.rentRooms,
    });
  }
  if (invoice?.invoiceType === "electric") {
    receiptDetails.push({
      description: `Tiền điện (${invoice.electricUsage} kWh x 3,500đ)`,
      amount: invoice.electricUsage * 3500,
    });
  }
  if (invoice?.invoiceType === "water") {
    receiptDetails.push({
      description: `Tiền nước (${invoice.waterUsage} m³ x 25,000đ)`,
      amount: invoice.waterUsage * 25000,
    });
  }
  if (invoice?.invoiceType === "service") {
    receiptDetails.push({
      description: `Phí dịch vụ`,
      amount: invoice.serviceFees,
    });
  }

  const totalAmount = receiptDetails.reduce(
    (sum, item) => sum + item.amount,
    0
  );

  if (!roomBookingInfo || !invoice) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <h1 className="text-2xl font-bold text-gray-900 mb-4">
            Không tìm thấy biên lai
          </h1>
          <Link
            href="/tenant-dashboard"
            className="text-green-600 hover:text-green-800"
          >
            ← Quay lại Dashboard
          </Link>
        </div>
      </div>
    );
  }

  const handlePrint = () => {
    window.print();
  };

  const handleDownload = () => {
    const element = document.createElement("a");
    element.href = "#";
    element.download = `bien-lai-${invoice.id}.pdf`;
    element.click();
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header - Hide when printing */}
      <header className="bg-white shadow-sm border-b print:hidden">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
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
              <span className="text-gray-600">Biên lai #{invoice.id}</span>
            </div>
            <div className="flex space-x-3">
              <button
                onClick={handlePrint}
                className="flex items-center px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
              >
                <PrinterIcon className="h-5 w-5 mr-2" />
                In biên lai
              </button>
              <button
                onClick={handleDownload}
                className="flex items-center px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors"
              >
                <DocumentArrowDownIcon className="h-5 w-5 mr-2" />
                Tải PDF
              </button>
            </div>
          </div>
        </div>
      </header>

      {/* Main content */}
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8 print:py-0 print:px-0">
        <div className="bg-white shadow-lg rounded-2xl border border-gray-100 overflow-hidden print:shadow-none print:rounded-none print:border-none">
          <div className="bg-gradient-to-r from-green-600 to-emerald-600 text-white p-8 print:bg-gray-800">
            <div className="flex justify-between items-start">
              <div>
                <h1 className="text-3xl font-bold mb-2">BIÊN LAI THANH TOÁN</h1>
                <p className="text-green-100">
                  SmartDorm - Hệ thống quản lý ký túc xá thông minh
                </p>
              </div>
              <div className="text-right">
                <div className="text-2xl font-bold">#{invoice.id}</div>
                <div className="flex items-center mt-2">
                  <CheckCircleIcon className="h-5 w-5 mr-2" />
                  <span className="bg-green-500 px-3 py-1 rounded-full text-sm font-medium">
                    Đã thanh toán
                  </span>
                </div>
              </div>
            </div>
          </div>

          <div className="p-8">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-8 mb-8">
              <div>
                <h3 className="text-lg font-semibold text-gray-900 mb-4 flex items-center">
                  <HomeIcon className="h-5 w-5 mr-2 text-blue-600" />
                  Thông tin chủ trọ
                </h3>
                <div className="bg-gray-50 p-4 rounded-xl">
                  <div className="space-y-2 text-sm">
                    <p>Tên: Chủ trọ ABC</p>
                    <p>Điện thoại: 0987654321</p>
                    <p>Email: owner@demo.com</p>
                    <p>Địa chỉ: 123 Đường ABC, Quận 1, TP.HCM</p>
                  </div>
                </div>
              </div>

              <div>
                <h3 className="text-lg font-semibold text-gray-900 mb-4 flex items-center">
                  <UserIcon className="h-5 w-5 mr-2 text-green-600" />
                  Thông tin khách thuê
                </h3>
                <div className="bg-gray-50 p-4 rounded-xl">
                  <div className="space-y-2 text-sm">
                    <p>Tên: {invoice.tenant}</p>
                    <p>Phòng: {invoice.roomNumber}</p>
                    <p>Điện thoại: {user?.phone}</p>
                    <p>Email: {user?.email}</p>
                  </div>
                </div>
              </div>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
              <div className="bg-blue-50 p-4 rounded-xl text-center">
                <CalendarIcon className="h-8 w-8 text-blue-600 mx-auto mb-2" />
                <div className="text-sm text-gray-600 mb-1">
                  Ngày thanh toán
                </div>
                <div className="font-semibold text-gray-900">
                  {formatDate(invoice.paidAt)}
                </div>
              </div>
              <div className="bg-green-50 p-4 rounded-xl text-center">
                <CreditCardIcon className="h-8 w-8 text-green-600 mx-auto mb-2" />
                <div className="text-sm text-gray-600 mb-1">Phương thức</div>
                <div className="font-semibold text-gray-900">
                  {invoice.method}
                </div>
              </div>
              <div className="bg-purple-50 p-4 rounded-xl text-center">
                <ClockIcon className="h-8 w-8 text-purple-600 mx-auto mb-2" />
                <div className="text-sm text-gray-600 mb-1">Mã giao dịch</div>
                <div className="font-semibold text-gray-900">
                  {invoice.transactionCode}
                </div>
              </div>
            </div>

            <div className="mb-8">
              <h3 className="text-lg font-semibold text-gray-900 mb-4">
                Chi tiết thanh toán
              </h3>
              <div className="bg-gray-50 rounded-xl overflow-hidden">
                <table className="w-full">
                  <thead className="bg-gray-100">
                    <tr>
                      <th className="px-6 py-4 text-left text-sm font-medium text-gray-900">
                        Mô tả
                      </th>
                      <th className="px-6 py-4 text-right text-sm font-medium text-gray-900">
                        Số tiền
                      </th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-200">
                    {receiptDetails.map((item, idx) => (
                      <tr key={idx}>
                        <td className="px-6 py-4 text-sm text-gray-900">
                          {item.description}
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-900 text-right font-medium">
                          {item.amount.toLocaleString()}đ
                        </td>
                      </tr>
                    ))}
                    <tr className="bg-green-50">
                      <td className="px-6 py-4 text-base font-bold text-gray-900">
                        TỔNG CỘNG
                      </td>
                      <td className="px-6 py-4 text-base font-bold text-green-600 text-right">
                        {totalAmount.toLocaleString()}đ
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>

            <div className="mb-8">
              <h3 className="text-lg font-semibold text-gray-900 mb-4">
                Ghi chú
              </h3>
              <div className="bg-yellow-50 border border-yellow-200 rounded-xl p-4">
                <p className="text-gray-700">
                  {invoice.note || "Không có ghi chú."}
                </p>
              </div>
            </div>

            <div className="border-t border-gray-200 pt-6 text-center text-gray-600">
              <p className="mb-2">
                Cảm ơn bạn đã sử dụng dịch vụ của SmartDorm!
              </p>
              <p className="text-sm">
                Biên lai này được tạo tự động vào ngày{" "}
                {new Date().toLocaleDateString("vi-VN")}
              </p>
              <p className="text-sm mt-2">
                Hotline hỗ trợ: 1900-1234 | Email: support@smartdorm.vn
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

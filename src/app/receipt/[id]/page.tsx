"use client";

import Link from "next/link";
import { useParams, useSearchParams } from "next/navigation";
import { useState, useEffect } from "react";
import {
  ArrowLeftIcon,
  PrinterIcon,
  DocumentArrowDownIcon,
  CheckCircleIcon,
  CalendarIcon,
  CurrencyDollarIcon,
  HomeIcon,
  UserIcon,
  ClockIcon,
  CreditCardIcon,
} from "@heroicons/react/24/outline";

import { getRoomBookingInfo } from "@/app/hooks/useRoomBookingInfo";
import { getInvoice } from "@/app/hooks/useInvoice";
import { useAuth } from "@/contexts/AuthContext";

// Mock data cho biên lai

export default function ReceiptPage() {
  const params = useParams();
  const receiptId = params.id as string;
  const [showPrintPreview, setShowPrintPreview] = useState(false);
  const [RoomBookings, setRoomBookings] = useState<any[]>([]);
  const [invoices, setInvoices] = useState<any[]>([]);

  const { user } = useAuth();
  useEffect(() => {
    getRoomBookingInfo().then(setRoomBookings);
    getInvoice().then(setInvoices);
  }, []); // thêm dependency array rỗng vào đây

  const formatDate = (dateString: any) => {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0"); // Tháng bắt đầu từ 0
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
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

  const roomBookingInfo = RoomBookings.find((r) => r.userId === user?.idUser);
  const invoice = invoices.find(
    (i) => i.userId === user?.idUser && i.id.toString() === receiptId
  );
  const mockReceipts = {
    "1": {
      id: "1",
      billNumber: "HD001",
      type: "Tiền phòng",
      month: "02/2024",
      amount: 3500000,
      paidDate: "2024-02-25",
      dueDate: "2024-02-25",
      status: "paid",
      paymentMethod: "Chuyển khoản",
      transactionId: "TXN123456789",
      tenant: {
        name: "Nguyễn Văn A",
        room: "101",
        phone: "0123456789",
        email: "tenant@demo.com",
      },
      landlord: {
        name: "Chủ trọ ABC",
        phone: "0987654321",
        email: "owner@demo.com",
        address: "123 Đường ABC, Quận 1, TP.HCM",
      },
      details: [
        {
          description: "Tiền thuê phòng tháng 02/2024",
          amount: invoice?.rentRooms,
        },
        {
          description: "Tiền điện (150 kWh x 3,500đ)",
          amount: invoice?.electricUsage * 3500,
        },
        {
          description: "Tiền nước (15m³ x 25,000đ)",
          amount: invoice?.waterUsage * 25000,
        },
        { description: "Phí dịch vụ", amount: invoice?.serviceFees * 25000 },
      ],
      notes: "Cảm ơn bạn đã thanh toán đúng hạn. Chúc bạn có một tháng vui vẻ!",
    },
  };
  const receipt = mockReceipts[receiptId as keyof typeof mockReceipts];

  if (!receipt || !roomBookingInfo || !invoice) {
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
    // Simulate PDF download
    const element = document.createElement("a");
    element.href = "#";
    element.download = `bien-lai-${receipt.billNumber}.pdf`;
    element.click();
  };

  const totalAmount = receipt.details.reduce(
    (sum, item) => sum + item.amount,
    0
  );

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
              <span className="text-gray-600">
                Biên lai {receipt.billNumber}
              </span>
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

      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8 print:py-0 print:px-0">
        {/* Receipt */}
        <div className="bg-white shadow-lg rounded-2xl border border-gray-100 overflow-hidden print:shadow-none print:rounded-none print:border-none">
          {/* Receipt Header */}
          <div className="bg-gradient-to-r from-green-600 to-emerald-600 text-white p-8 print:bg-gray-800">
            <div className="flex justify-between items-start">
              <div>
                <h1 className="text-3xl font-bold mb-2">BIÊN LAI THANH TOÁN</h1>
                <p className="text-green-100">
                  SmartDorm - Hệ thống quản lý ký túc xá thông minh
                </p>
              </div>
              <div className="text-right">
                <div className="text-2xl font-bold">#{receipt.billNumber}</div>
                <div className="flex items-center mt-2">
                  <CheckCircleIcon className="h-5 w-5 mr-2" />
                  <span className="bg-green-500 px-3 py-1 rounded-full text-sm font-medium">
                    Đã thanh toán
                  </span>
                </div>
              </div>
            </div>
          </div>

          {/* Receipt Body */}
          <div className="p-8">
            {/* Parties Information */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-8 mb-8">
              {/* Landlord Info */}
              <div>
                <h3 className="text-lg font-semibold text-gray-900 mb-4 flex items-center">
                  <HomeIcon className="h-5 w-5 mr-2 text-blue-600" />
                  Thông tin chủ trọ
                </h3>
                <div className="bg-gray-50 p-4 rounded-xl">
                  <div className="space-y-2">
                    <div>
                      <span className="font-medium">Tên:</span>{" "}
                      {receipt.landlord.name}
                    </div>
                    <div>
                      <span className="font-medium">Điện thoại:</span>{" "}
                      {receipt.landlord.phone}
                    </div>
                    <div>
                      <span className="font-medium">Email:</span>{" "}
                      {receipt.landlord.email}
                    </div>
                    <div>
                      <span className="font-medium">Địa chỉ:</span>{" "}
                      {receipt.landlord.address}
                    </div>
                  </div>
                </div>
              </div>

              {/* Tenant Info */}
              <div>
                <h3 className="text-lg font-semibold text-gray-900 mb-4 flex items-center">
                  <UserIcon className="h-5 w-5 mr-2 text-green-600" />
                  Thông tin khách thuê
                </h3>
                <div className="bg-gray-50 p-4 rounded-xl">
                  <div className="space-y-2">
                    <div>
                      <span className="font-medium">Tên:</span>{" "}
                      {roomBookingInfo.fullName ? roomBookingInfo.fullName : ""}
                    </div>
                    <div>
                      <span className="font-medium">Phòng:</span>{" "}
                      {roomBookingInfo.roomNumber}
                    </div>
                    <div>
                      <span className="font-medium">Điện thoại:</span>{" "}
                      {roomBookingInfo.phone}
                    </div>
                    <div>
                      <span className="font-medium">Email:</span>{" "}
                      {roomBookingInfo.email}
                    </div>
                  </div>
                </div>
              </div>
            </div>

            {/* Payment Information */}
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

            {/* Bill Details */}
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
                    {invoice.invoiceType === "monthly" && (
                      <tr>
                        <td className="px-6 py-4 text-sm text-gray-900">
                          Tiền thuê phòng tháng{" "}
                          {formatDate(invoice.invoiceDateLimit)}
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-900 text-right font-medium">
                          {invoice.rentRooms.toLocaleString()}đ
                        </td>
                      </tr>
                    )}

                    {invoice.invoiceType === "electric" && (
                      <tr>
                        <td className="px-6 py-4 text-sm text-gray-900">
                          Tiền điện ({invoice.electricUsage} kWh x 3,500đ)
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-900 text-right font-medium">
                          {(invoice.electricUsage * 3500).toLocaleString()}đ
                        </td>
                      </tr>
                    )}

                    {invoice.invoiceType === "water" && (
                      <tr>
                        <td className="px-6 py-4 text-sm text-gray-900">
                          Tiền nước ({invoice.waterUsage} m³ x 25,000đ)
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-900 text-right font-medium">
                          {(invoice.waterUsage * 25000).toLocaleString()}đ
                        </td>
                      </tr>
                    )}

                    {invoice.invoiceType === "service" && (
                      <tr>
                        <td className="px-6 py-4 text-sm text-gray-900">
                          Phí dịch vụ
                        </td>
                        <td className="px-6 py-4 text-sm text-gray-900 text-right font-medium">
                          {invoice.serviceFees.toLocaleString()}đ
                        </td>
                      </tr>
                    )}

                    <tr className="bg-green-50">
                      <td className="px-6 py-4 text-base font-bold text-gray-900">
                        TỔNG CỘNG
                      </td>
                      <td className="px-6 py-4 text-base font-bold text-green-600 text-right">
                        {(() => {
                          switch (invoice.invoiceType) {
                            case "monthly":
                              return invoice.rentRooms.toLocaleString();
                            case "electric":
                              return (
                                invoice.electricUsage * 3500
                              ).toLocaleString();
                            case "water":
                              return (
                                invoice.waterUsage * 25000
                              ).toLocaleString();
                            case "service":
                              return invoice.serviceFees.toLocaleString();
                            default:
                              return "0";
                          }
                        })()}
                        đ
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>

            {/* Notes */}
            {receipt.notes && (
              <div className="mb-8">
                <h3 className="text-lg font-semibold text-gray-900 mb-4">
                  Ghi chú
                </h3>
                <div className="bg-yellow-50 border border-yellow-200 rounded-xl p-4">
                  <p className="text-gray-700">{receipt.notes}</p>
                </div>
              </div>
            )}

            {/* Footer */}
            <div className="border-t border-gray-200 pt-6">
              <div className="text-center text-gray-600">
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

        {/* Action Buttons - Hide when printing */}
        <div className="mt-8 flex justify-center space-x-4 print:hidden">
          <Link
            href="/tenant-dashboard"
            className="px-6 py-3 bg-gray-600 text-white rounded-lg hover:bg-gray-700 transition-colors"
          >
            Quay lại Dashboard
          </Link>
          <button
            onClick={handlePrint}
            className="px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors flex items-center"
          >
            <PrinterIcon className="h-5 w-5 mr-2" />
            In biên lai
          </button>
          <button
            onClick={handleDownload}
            className="px-6 py-3 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors flex items-center"
          >
            <DocumentArrowDownIcon className="h-5 w-5 mr-2" />
            Tải PDF
          </button>
        </div>
      </div>

      {/* Print Styles */}
      <style jsx global>{`
        @media print {
          body {
            background: white !important;
          }
          .print\\:hidden {
            display: none !important;
          }
          .print\\:shadow-none {
            box-shadow: none !important;
          }
          .print\\:rounded-none {
            border-radius: 0 !important;
          }
          .print\\:border-none {
            border: none !important;
          }
          .print\\:py-0 {
            padding-top: 0 !important;
            padding-bottom: 0 !important;
          }
          .print\\:px-0 {
            padding-left: 0 !important;
            padding-right: 0 !important;
          }
          .print\\:bg-gray-800 {
            background-color: #1f2937 !important;
          }
        }
      `}</style>
    </div>
  );
}

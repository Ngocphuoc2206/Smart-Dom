"use client";

import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";
import { useMemo } from "react";

type Invoice = {
  invoiceDateLimit: string;
  invoiceAmount: number;
};

export default function RevenueLineChart({
  invoices,
}: {
  invoices: Invoice[];
}) {
  // Nhóm và tính tổng doanh thu theo tháng
  const chartData = useMemo(() => {
    const monthlyRevenue: { [key: string]: number } = {};

    invoices.forEach((inv) => {
      const date = new Date(inv.invoiceDateLimit);
      const key = `${date.getMonth() + 1}/${date.getFullYear()}`; // ví dụ "6/2025"

      if (!monthlyRevenue[key]) {
        monthlyRevenue[key] = 0;
      }
      monthlyRevenue[key] += inv.invoiceAmount;
    });

    // Chuyển sang dạng mảng
    return Object.entries(monthlyRevenue).map(([month, revenue]) => ({
      month,
      revenue,
    }));
  }, [invoices]);

  return (
    <ResponsiveContainer width="100%" height="100%">
      <LineChart data={chartData}>
        <CartesianGrid strokeDasharray="3 3" />
        <XAxis dataKey="month" />
        <YAxis tickFormatter={(value) => value / 1_000_000 + "M"} />
        <Tooltip
          formatter={(value: number) => value.toLocaleString("vi-VN") + "₫"}
        />
        <Line
          type="monotone"
          dataKey="revenue"
          stroke="#34d399"
          strokeWidth={3}
          dot={{ r: 4 }}
        />
      </LineChart>
    </ResponsiveContainer>
  );
}

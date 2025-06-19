"use client";

import {
  BuildingOfficeIcon,
  EyeIcon,
  EyeSlashIcon,
  LockClosedIcon,
  UserIcon,
} from "@heroicons/react/24/outline";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { useAuth } from "../../../contexts/AuthContext";

export default function LoginPage() {
  const router = useRouter();
  const { login } = useAuth();
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    userType: "tenant" as "owner" | "tenant",
  });
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
    setError("");
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError("");

    // Xử lý login demo cho chủ trọ
    if (formData.userType === "owner") {
      setTimeout(() => {
        if (
          formData.email === "owner@demo.com" &&
          formData.password === "123456"
        ) {
          const userData = {
            email: formData.email,
            name: "Chủ trọ",
            userType: "owner" as const,
            isAuthenticated: true,
            phone: "0393329634",
            idNumber: "052204015567",
            dob: "06-22-2004",
            address: "38D Tam Bình, Thủ Đức, TP.HCM",
            gender: "male",
          };
          login(userData);
          router.push("/owner-dashboard");
        } else {
          setError("Tài khoản demo chủ trọ là: owner@demo.com / 123456");
        }
        setIsLoading(false);
      }, 1500);
      return;
    }

    // Gọi API thật cho khách thuê
    try {
      const response = await fetch(`https://localhost:7257/api/Account/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          email: formData.email,
          password: formData.password,
        }),
      });

      if (!response.ok) {
        const json = await response.json().catch(() => null);
        throw new Error(json?.message || "Đăng nhập thất bại");
      }

      const data = await response.json();

      setTimeout(() => {
        if (data.email && formData.password) {
          const userData = {
            email: data.email,
            userType: "tenant" as const,
            name: data.role === 0 ? "Chủ trọ" : data.fullName,
            isAuthenticated: true,
            phone: data.phone,
            idUser: data.idUser,
            idNumber: data.idNumber,
            dob: data.dateOfBirth,
            address: data.address,
            gender: data.gender,
          };
          login(userData);
          router.push("/tenant-dashboard");
        } else {
          setError("Vui lòng nhập đầy đủ thông tin");
        }
        setIsLoading(false);
      }, 1500);
    } catch (err: any) {
      setError(err.message);
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-indigo-50 to-purple-50 flex items-center justify-center p-4">
      {/* Background Pattern */}
      <div className="absolute inset-0 bg-grid-pattern opacity-5"></div>

      <div className="max-w-md w-full space-y-8 relative">
        {/* Logo and Header */}
        <div className="text-center">
          <div className="mx-auto h-16 w-16 bg-gradient-to-r from-blue-600 to-indigo-600 rounded-2xl flex items-center justify-center shadow-lg">
            <BuildingOfficeIcon className="h-8 w-8 text-white" />
          </div>
          <h2 className="mt-6 text-3xl font-bold text-gray-900">
            Đăng nhập SmartDorm
          </h2>
          <p className="mt-2 text-sm text-gray-600">
            Quản lý nhà trọ thông minh và hiệu quả
          </p>
        </div>

        {/* Login Form */}
        <div className="bg-white/80 backdrop-blur-lg rounded-2xl shadow-xl p-8 border border-white/20">
          <form className="space-y-6" onSubmit={handleSubmit}>
            {/* User Type Selection */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Loại tài khoản
              </label>
              <div className="grid grid-cols-2 gap-3">
                <button
                  type="button"
                  onClick={() =>
                    setFormData((prev) => ({ ...prev, userType: "owner" }))
                  }
                  className={`p-3 rounded-xl border-2 transition-all ${
                    formData.userType === "owner"
                      ? "border-blue-500 bg-blue-50 text-blue-700"
                      : "border-gray-200 hover:border-gray-300"
                  }`}
                >
                  <div className="text-center">
                    <div className="text-2xl mb-1">👤</div>
                    <div className="text-sm font-medium">Chủ trọ</div>
                  </div>
                </button>
                <button
                  type="button"
                  onClick={() =>
                    setFormData((prev) => ({ ...prev, userType: "tenant" }))
                  }
                  className={`p-3 rounded-xl border-2 transition-all ${
                    formData.userType === "tenant"
                      ? "border-green-500 bg-green-50 text-green-700"
                      : "border-gray-200 hover:border-gray-300"
                  }`}
                >
                  <div className="text-center">
                    <div className="text-2xl mb-1">🧑‍💼</div>
                    <div className="text-sm font-medium">Khách thuê</div>
                  </div>
                </button>
              </div>
            </div>

            {/* Email Input */}
            <div>
              <label
                htmlFor="email"
                className="block text-sm font-medium text-gray-700 mb-2"
              >
                Email
              </label>
              <div className="relative">
                <UserIcon className="h-5 w-5 text-gray-400 absolute left-3 top-3.5" />
                <input
                  id="email"
                  name="email"
                  type="email"
                  value={formData.email}
                  onChange={handleInputChange}
                  className="form-input w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all text-gray-900 placeholder-gray-500"
                  placeholder="Nhập email của bạn"
                  required
                />
              </div>
            </div>

            {/* Password Input */}
            <div>
              <label
                htmlFor="password"
                className="block text-sm font-medium text-gray-700 mb-2"
              >
                Mật khẩu
              </label>
              <div className="relative">
                <LockClosedIcon className="h-5 w-5 text-gray-400 absolute left-3 top-3.5" />
                <input
                  id="password"
                  name="password"
                  type={showPassword ? "text" : "password"}
                  value={formData.password}
                  onChange={handleInputChange}
                  className="form-input w-full pl-10 pr-12 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all text-gray-900 placeholder-gray-500"
                  placeholder="Nhập mật khẩu"
                  required
                />
                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                  className="absolute right-3 top-3.5 text-gray-400 hover:text-gray-600"
                >
                  {showPassword ? (
                    <EyeSlashIcon className="h-5 w-5" />
                  ) : (
                    <EyeIcon className="h-5 w-5" />
                  )}
                </button>
              </div>
            </div>

            {/* Error Message */}
            {error && (
              <div className="bg-red-50 border border-red-200 rounded-xl p-3">
                <p className="text-sm text-red-600">{error}</p>
              </div>
            )}

            {/* Demo Accounts */}
            <div className="bg-blue-50 border border-blue-200 rounded-xl p-4">
              <h4 className="text-sm font-medium text-blue-900 mb-2">
                Tài khoản demo:
              </h4>
              <div className="text-xs text-blue-700 space-y-1">
                <p>
                  <strong>Chủ trọ:</strong> owner@demo.com / 123456
                </p>
              </div>
              <div className="mt-3 pt-3 border-t border-blue-200">
                <p className="text-xs text-blue-600">
                  <strong>Chủ trọ mới:</strong> Liên hệ{" "}
                  <strong>1900 1234</strong> để được cấp tài khoản quản lý miễn
                  phí.
                </p>
              </div>
            </div>

            {/* Submit Button */}
            <button
              type="submit"
              disabled={isLoading}
              className={`w-full py-3 px-4 rounded-xl font-semibold text-white transition-all ${
                isLoading
                  ? "bg-gray-400 cursor-not-allowed"
                  : formData.userType === "owner"
                  ? "bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 shadow-lg hover:shadow-xl"
                  : "bg-gradient-to-r from-green-600 to-emerald-600 hover:from-green-700 hover:to-emerald-700 shadow-lg hover:shadow-xl"
              }`}
            >
              {isLoading ? (
                <div className="flex items-center justify-center">
                  <div className="animate-spin rounded-full h-5 w-5 border-b-2 border-white mr-2"></div>
                  Đang đăng nhập...
                </div>
              ) : (
                "Đăng nhập"
              )}
            </button>

            {/* Links */}
            <div className="text-center space-y-2">
              <Link
                href="/auth/forgot-password"
                className="text-sm text-blue-600 hover:text-blue-800"
              >
                Quên mật khẩu?
              </Link>
              <div className="text-sm text-gray-600">
                Chưa có tài khoản?{" "}
                <Link
                  href="/auth/register"
                  className="text-blue-600 hover:text-blue-800 font-medium"
                >
                  Đăng ký ngay
                </Link>
              </div>
            </div>
          </form>
        </div>

        {/* Back to Home */}
        <div className="text-center">
          <Link href="/" className="text-sm text-gray-600 hover:text-gray-800">
            ← Quay về trang chủ
          </Link>
        </div>
      </div>

      <style jsx>{`
        .bg-grid-pattern {
          background-image: radial-gradient(
            circle,
            #e5e7eb 1px,
            transparent 1px
          );
          background-size: 20px 20px;
        }
      `}</style>
    </div>
  );
}

"use client";

import Link from "next/link";
import { useState } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "../../../contexts/AuthContext";
import {
  EyeIcon,
  EyeSlashIcon,
  UserIcon,
  LockClosedIcon,
  BuildingOfficeIcon,
  EnvelopeIcon,
  PhoneIcon,
  IdentificationIcon,
  HomeIcon,
} from "@heroicons/react/24/outline";

interface RegisterForm {
  fullName: string;
  email: string;
  phone: string;
  idNumber: string;
  gender: "male" | "female" | "other";
  password: string;
  confirmPassword: string;
  address: string;
  userType: "owner" | "tenant";
  agreeTerms: boolean;
  dateOfBirth: string; // <-- thêm dòng này
}

export default function RegisterPage() {
  const router = useRouter();
  const { login } = useAuth();
  const [formData, setFormData] = useState<RegisterForm>({
    fullName: "",
    email: "",
    phone: "",
    idNumber: "",
    password: "",
    gender: "male",
    confirmPassword: "",
    address: "",
    userType: "tenant",
    agreeTerms: false,
    dateOfBirth: "", // <-- thêm dòng này
  });

  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState<
    Partial<Record<keyof RegisterForm, string>>
  >({});
  const [globalError, setGlobalError] = useState("");

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
    if (errors[name as keyof RegisterForm]) {
      setErrors((prev) => ({ ...prev, [name]: "" }));
    }
    setGlobalError("");
  };

  const validateForm = () => {
    const newErrors: Partial<Record<keyof RegisterForm, string>> = {};

    if (!formData.fullName.trim()) newErrors.fullName = "Vui lòng nhập họ tên";
    if (!formData.email.trim()) newErrors.email = "Vui lòng nhập email";
    else if (!/\S+@\S+\.\S+/.test(formData.email))
      newErrors.email = "Email không hợp lệ";
    if (!formData.dateOfBirth)
      newErrors.dateOfBirth = "Vui lòng chọn ngày sinh";
    if (!formData.address.trim()) newErrors.address = "Vui lòng nhập địa chỉ";
    if (!formData.phone.trim()) newErrors.phone = "Vui lòng nhập số điện thoại";
    else if (!/^[0-9]{10,11}$/.test(formData.phone))
      newErrors.phone = "Số điện thoại không hợp lệ";
    if (!formData.idNumber.trim())
      newErrors.idNumber = "Vui lòng nhập số CMND/CCCD";
    else if (!/^[0-9]{9,12}$/.test(formData.idNumber))
      newErrors.idNumber = "Số CMND/CCCD không hợp lệ";
    if (!formData.password) newErrors.password = "Vui lòng nhập mật khẩu";
    else if (formData.password.length < 6)
      newErrors.password = "Mật khẩu phải có ít nhất 6 ký tự";
    if (!formData.confirmPassword)
      newErrors.confirmPassword = "Vui lòng xác nhận mật khẩu";
    else if (formData.password !== formData.confirmPassword)
      newErrors.confirmPassword = "Mật khẩu xác nhận không khớp";
    if (!formData.agreeTerms)
      newErrors.agreeTerms = "Bạn cần đồng ý với điều khoản";

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateForm()) return;
    setIsLoading(true);

    //Call Api
    try {
      const response = await fetch(
        `https://localhost:7257/api/Account/register`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            FullName: formData.fullName,
            Email: formData.email,
            Phone: formData.phone,
            DOB: new Date(formData.dateOfBirth).toISOString(),
            Address: formData.address,
            Gender: formData.gender,
            Password: formData.password,
            IDCard: formData.idNumber,
            UserType: formData.userType,
          }),
        }
      );

      if (!response.ok) {
        const json = await response.json().catch(() => null);
        throw new Error(json?.message || "Đăng ký thất bại");
      }
      router.push("/auth/login");
    } catch (err: any) {
      setGlobalError(err.message || "Có lỗi xảy ra, vui lòng thử lại.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-purple-50 via-blue-50 to-indigo-50 flex items-center justify-center p-4">
      {/* Background Pattern */}
      <div className="absolute inset-0 bg-grid-pattern opacity-5"></div>

      <div className="max-w-lg w-full space-y-8 relative">
        {/* Logo and Header */}
        <div className="text-center">
          <div className="mx-auto h-16 w-16 bg-gradient-to-r from-purple-600 to-blue-600 rounded-2xl flex items-center justify-center shadow-lg">
            <BuildingOfficeIcon className="h-8 w-8 text-white" />
          </div>
          <h2 className="mt-6 text-3xl font-bold text-gray-900">
            Đăng ký SmartDorm
          </h2>
          <p className="mt-2 text-sm text-gray-600">
            Tạo tài khoản để bắt đầu sử dụng
          </p>
        </div>

        {/* Register Form */}
        <div className="bg-white/80 backdrop-blur-lg rounded-2xl shadow-xl p-8 border border-white/20">
          <form className="space-y-6" onSubmit={handleSubmit}>
            {/* Info Notice */}
            <div className="bg-green-50 border border-green-200 rounded-xl p-4">
              <div className="flex items-start">
                <div className="text-2xl mr-3">🧑‍💼</div>
                <div>
                  <h4 className="text-green-900 font-semibold mb-1">
                    Đăng ký dành cho khách thuê
                  </h4>
                  <p className="text-green-700 text-sm mb-2">
                    Trang này dành cho khách thuê muốn tìm và thuê phòng trọ.
                  </p>
                  <p className="text-green-600 text-xs">
                    <strong>Chủ trọ:</strong> Vui lòng liên hệ hotline{" "}
                    <strong>1900 1234</strong> để được cấp tài khoản quản lý.
                  </p>
                </div>
              </div>
            </div>

            {/* Full Name */}
            <div>
              <label
                htmlFor="fullName"
                className="block text-sm font-medium text-gray-700 mb-2"
              >
                Họ và tên *
              </label>
              <div className="relative">
                <UserIcon className="h-5 w-5 text-gray-400 absolute left-3 top-3.5" />
                <input
                  id="fullName"
                  name="fullName"
                  type="text"
                  value={formData.fullName}
                  onChange={handleInputChange}
                  className={`form-input w-full pl-10 pr-4 py-3 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all text-gray-900 placeholder-gray-500 ${
                    errors.fullName ? "border-red-300" : "border-gray-300"
                  }`}
                  placeholder="Nhập họ và tên"
                />
              </div>
              {errors.fullName && (
                <p className="mt-1 text-sm text-red-600">{errors.fullName}</p>
              )}
            </div>

            {/* Email */}
            <div>
              <label
                htmlFor="email"
                className="block text-sm font-medium text-gray-700 mb-2"
              >
                Email *
              </label>
              <div className="relative">
                <EnvelopeIcon className="h-5 w-5 text-gray-400 absolute left-3 top-3.5" />
                <input
                  id="email"
                  name="email"
                  type="email"
                  value={formData.email}
                  onChange={handleInputChange}
                  className={`form-input w-full pl-10 pr-4 py-3 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all text-gray-900 placeholder-gray-500 ${
                    errors.email ? "border-red-300" : "border-gray-300"
                  }`}
                  placeholder="Nhập email"
                />
              </div>
              {errors.email && (
                <p className="mt-1 text-sm text-red-600">{errors.email}</p>
              )}
            </div>

            {/* DOB */}
            <div className="mb-4">
              <label htmlFor="dateOfBirth" className="block text-gray-700">
                Ngày sinh
              </label>
              <input
                type="date"
                name="dateOfBirth"
                id="dateOfBirth"
                value={formData.dateOfBirth}
                onChange={handleInputChange}
                className={`w-full p-2 border rounded ${
                  errors.dateOfBirth ? "border-red-500" : "border-gray-300"
                }`}
              />
              {errors.dateOfBirth && (
                <p className="text-red-500 text-sm">{errors.dateOfBirth}</p>
              )}
            </div>

            {/* Address */}
            <div>
              <label
                htmlFor="address"
                className="block text-sm font-medium text-gray-700 mb-2"
              >
                Địa chỉ *
              </label>
              <div className="relative">
                <HomeIcon className="h-5 w-5 text-gray-400 absolute left-3 top-3.5" />
                <input
                  id="address"
                  name="address"
                  type="text"
                  value={formData.address}
                  onChange={handleInputChange}
                  className={`form-input w-full pl-10 pr-4 py-3 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all text-gray-900 placeholder-gray-500 ${
                    errors.address ? "border-red-300" : "border-gray-300"
                  }`}
                  placeholder="Nhập địa chỉ"
                />
              </div>
              {errors.address && (
                <p className="mt-1 text-sm text-red-600">{errors.address}</p>
              )}
            </div>

            {/* Phone */}
            <div>
              <label
                htmlFor="phone"
                className="block text-sm font-medium text-gray-700 mb-2"
              >
                Số điện thoại *
              </label>
              <div className="relative">
                <PhoneIcon className="h-5 w-5 text-gray-400 absolute left-3 top-3.5" />
                <input
                  id="phone"
                  name="phone"
                  type="tel"
                  value={formData.phone}
                  onChange={handleInputChange}
                  className={`form-input w-full pl-10 pr-4 py-3 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all text-gray-900 placeholder-gray-500 ${
                    errors.phone ? "border-red-300" : "border-gray-300"
                  }`}
                  placeholder="0123456789"
                />
              </div>
              {errors.phone && (
                <p className="mt-1 text-sm text-red-600">{errors.phone}</p>
              )}
            </div>

            {/* ID Number */}
            <div>
              <label
                htmlFor="idNumber"
                className="block text-sm font-medium text-gray-700 mb-2"
              >
                Số CMND/CCCD *
              </label>
              <div className="relative">
                <IdentificationIcon className="h-5 w-5 text-gray-400 absolute left-3 top-3.5" />
                <input
                  id="idNumber"
                  name="idNumber"
                  type="text"
                  value={formData.idNumber}
                  onChange={handleInputChange}
                  className={`form-input w-full pl-10 pr-4 py-3 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all text-gray-900 placeholder-gray-500 ${
                    errors.idNumber ? "border-red-300" : "border-gray-300"
                  }`}
                  placeholder="123456789012"
                />
              </div>
              {errors.idNumber && (
                <p className="mt-1 text-sm text-red-600">{errors.idNumber}</p>
              )}
            </div>

            {/* Gender */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Giới tính *
              </label>
              <div className="flex gap-4">
                <label className="flex items-center">
                  <input
                    type="radio"
                    name="gender"
                    value="male"
                    checked={formData.gender === "male"}
                    onChange={handleInputChange}
                    className="text-blue-600 focus:ring-blue-500 border-gray-300"
                  />
                  <span className="ml-2 text-gray-700 text-sm">Nam</span>
                </label>
                <label className="flex items-center">
                  <input
                    type="radio"
                    name="gender"
                    value="female"
                    checked={formData.gender === "female"}
                    onChange={handleInputChange}
                    className="text-blue-600 focus:ring-blue-500 border-gray-300"
                  />
                  <span className="ml-2 text-gray-700 text-sm">Nữ</span>
                </label>
                <label className="flex items-center">
                  <input
                    type="radio"
                    name="gender"
                    value="other"
                    checked={formData.gender === "other"}
                    onChange={handleInputChange}
                    className="text-blue-600 focus:ring-blue-500 border-gray-300"
                  />
                  <span className="ml-2 text-gray-700 text-sm">Khác</span>
                </label>
              </div>
              {errors.gender && (
                <p className="mt-1 text-sm text-red-600">{errors.gender}</p>
              )}
            </div>

            {/* Password */}
            <div>
              <label
                htmlFor="password"
                className="block text-sm font-medium text-gray-700 mb-2"
              >
                Mật khẩu *
              </label>
              <div className="relative">
                <LockClosedIcon className="h-5 w-5 text-gray-400 absolute left-3 top-3.5" />
                <input
                  id="password"
                  name="password"
                  type={showPassword ? "text" : "password"}
                  value={formData.password}
                  onChange={handleInputChange}
                  className={`form-input w-full pl-10 pr-12 py-3 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all text-gray-900 placeholder-gray-500 ${
                    errors.password ? "border-red-300" : "border-gray-300"
                  }`}
                  placeholder="Nhập mật khẩu"
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
              {errors.password && (
                <p className="mt-1 text-sm text-red-600">{errors.password}</p>
              )}
            </div>

            {/* Confirm Password */}
            <div>
              <label
                htmlFor="confirmPassword"
                className="block text-sm font-medium text-gray-700 mb-2"
              >
                Xác nhận mật khẩu *
              </label>
              <div className="relative">
                <LockClosedIcon className="h-5 w-5 text-gray-400 absolute left-3 top-3.5" />
                <input
                  id="confirmPassword"
                  name="confirmPassword"
                  type={showConfirmPassword ? "text" : "password"}
                  value={formData.confirmPassword}
                  onChange={handleInputChange}
                  className={`form-input w-full pl-10 pr-12 py-3 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all text-gray-900 placeholder-gray-500 ${
                    errors.confirmPassword
                      ? "border-red-300"
                      : "border-gray-300"
                  }`}
                  placeholder="Nhập lại mật khẩu"
                />
                <button
                  type="button"
                  onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                  className="absolute right-3 top-3.5 text-gray-400 hover:text-gray-600"
                >
                  {showConfirmPassword ? (
                    <EyeSlashIcon className="h-5 w-5" />
                  ) : (
                    <EyeIcon className="h-5 w-5" />
                  )}
                </button>
              </div>
              {errors.confirmPassword && (
                <p className="mt-1 text-sm text-red-600">
                  {errors.confirmPassword}
                </p>
              )}
            </div>

            {/* Terms Agreement */}
            <div>
              <label className="flex items-start">
                <input
                  type="checkbox"
                  name="agreeTerms"
                  checked={formData.agreeTerms}
                  onChange={handleInputChange}
                  className="mt-1 h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
                />
                <span className="ml-2 text-sm text-gray-600">
                  Tôi đồng ý với{" "}
                  <Link
                    href="/terms"
                    className="text-blue-600 hover:text-blue-800"
                  >
                    Điều khoản sử dụng
                  </Link>{" "}
                  và{" "}
                  <Link
                    href="/privacy"
                    className="text-blue-600 hover:text-blue-800"
                  >
                    Chính sách bảo mật
                  </Link>
                </span>
              </label>
              {errors.agreeTerms && (
                <p className="mt-1 text-sm text-red-600">{errors.agreeTerms}</p>
              )}
            </div>

            {/* Submit Button */}
            <button
              type="submit"
              disabled={isLoading}
              className={`w-full py-3 px-4 rounded-xl font-semibold text-white transition-all ${
                isLoading
                  ? "bg-gray-400 cursor-not-allowed"
                  : "bg-gradient-to-r from-green-600 to-emerald-600 hover:from-green-700 hover:to-emerald-700 shadow-lg hover:shadow-xl"
              }`}
            >
              {isLoading ? (
                <div className="flex items-center justify-center">
                  <div className="animate-spin rounded-full h-5 w-5 border-b-2 border-white mr-2"></div>
                  Đang tạo tài khoản...
                </div>
              ) : (
                "Đăng ký"
              )}
            </button>

            {/* Links */}
            <div className="text-center">
              <div className="text-sm text-gray-600">
                Đã có tài khoản?{" "}
                <Link
                  href="/auth/login"
                  className="text-blue-600 hover:text-blue-800 font-medium"
                >
                  Đăng nhập ngay
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

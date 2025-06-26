module.exports = {

"[externals]/next/dist/server/app-render/action-async-storage.external.js [external] (next/dist/server/app-render/action-async-storage.external.js, cjs)": (function(__turbopack_context__) {

var { g: global, __dirname, m: module, e: exports } = __turbopack_context__;
{
const mod = __turbopack_context__.x("next/dist/server/app-render/action-async-storage.external.js", () => require("next/dist/server/app-render/action-async-storage.external.js"));

module.exports = mod;
}}),
"[externals]/next/dist/server/app-render/work-unit-async-storage.external.js [external] (next/dist/server/app-render/work-unit-async-storage.external.js, cjs)": (function(__turbopack_context__) {

var { g: global, __dirname, m: module, e: exports } = __turbopack_context__;
{
const mod = __turbopack_context__.x("next/dist/server/app-render/work-unit-async-storage.external.js", () => require("next/dist/server/app-render/work-unit-async-storage.external.js"));

module.exports = mod;
}}),
"[externals]/next/dist/server/app-render/work-async-storage.external.js [external] (next/dist/server/app-render/work-async-storage.external.js, cjs)": (function(__turbopack_context__) {

var { g: global, __dirname, m: module, e: exports } = __turbopack_context__;
{
const mod = __turbopack_context__.x("next/dist/server/app-render/work-async-storage.external.js", () => require("next/dist/server/app-render/work-async-storage.external.js"));

module.exports = mod;
}}),
"[project]/src/app/hooks/useRoom.tsx [app-ssr] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname } = __turbopack_context__;
{
__turbopack_context__.s({
    "getRoom": (()=>getRoom)
});
async function getRoom() {
    try {
        const res = await fetch("https://localhost:7257/api/Room");
        if (!res.ok) {
            throw new Error("Network response was not ok");
        }
        const data = await res.json();
        console.log("Dữ liệu trả về:", data); // 👉 debug xem có đúng không
        return Array.isArray(data.result) ? data.result : [];
    } catch (error) {
        console.error("Error fetching room data:", error);
        return [];
    }
}
}}),
"[project]/src/app/hooks/useRoomBookingInfo.tsx [app-ssr] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname } = __turbopack_context__;
{
__turbopack_context__.s({
    "getRoomBookingInfo": (()=>getRoomBookingInfo)
});
async function getRoomBookingInfo() {
    try {
        const res = await fetch("https://localhost:7257/api/RoomBooking/tenant");
        if (!res.ok) {
            throw new Error("Network response was not ok");
        }
        const data = await res.json();
        if (!Array.isArray(data)) {
            throw new Error("Expected an array of room booking info");
        }
        return data;
    } catch (error) {
        console.error("Error fetching room data:", error);
        return [];
    }
}
}}),
"[project]/src/app/hooks/useDurationContract.tsx [app-ssr] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname } = __turbopack_context__;
{
__turbopack_context__.s({
    "getDurationContract": (()=>getDurationContract)
});
async function getDurationContract() {
    try {
        const res = await fetch("https://localhost:7257/api/DurationContract");
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
}}),
"[project]/src/app/hooks/useInvoice.tsx [app-ssr] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname } = __turbopack_context__;
{
__turbopack_context__.s({
    "getInvoice": (()=>getInvoice)
});
async function getInvoice() {
    try {
        const res = await fetch("https://localhost:7257/api/Invoice");
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
}}),
"[externals]/module [external] (module, cjs)": (function(__turbopack_context__) {

var { g: global, __dirname, m: module, e: exports } = __turbopack_context__;
{
const mod = __turbopack_context__.x("module", () => require("module"));

module.exports = mod;
}}),
"[project]/src/app/owner-dashboard/page.tsx [app-ssr] (ecmascript)": (function(__turbopack_context__) {

var { g: global, __dirname, m: module, e: exports } = __turbopack_context__;
{
const e = new Error(`Could not parse module '[project]/src/app/owner-dashboard/page.tsx'

Expression expected`);
e.code = 'MODULE_UNPARSEABLE';
throw e;}}),

};

//# sourceMappingURL=%5Broot-of-the-server%5D__6f55ff2b._.js.map
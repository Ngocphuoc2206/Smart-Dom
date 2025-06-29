(globalThis.TURBOPACK = globalThis.TURBOPACK || []).push([typeof document === "object" ? document.currentScript : undefined, {

"[project]/src/app/messages/page.tsx [app-client] (ecmascript)": ((__turbopack_context__) => {
"use strict";

var { g: global, __dirname, k: __turbopack_refresh__, m: module } = __turbopack_context__;
{
// pages/tenant/chat/[ownerId].tsx
__turbopack_context__.s({
    "default": (()=>TenantChat)
});
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$jsx$2d$dev$2d$runtime$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/next/dist/compiled/react/jsx-dev-runtime.js [app-client] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$index$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/next/dist/compiled/react/index.js [app-client] (ecmascript)");
var __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$microsoft$2f$signalr$2f$dist$2f$esm$2f$HubConnectionBuilder$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__ = __turbopack_context__.i("[project]/node_modules/@microsoft/signalr/dist/esm/HubConnectionBuilder.js [app-client] (ecmascript)");
;
var _s = __turbopack_context__.k.signature();
"use client";
;
;
function TenantChat({ ownerId }) {
    _s();
    const [connection, setConnection] = (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$index$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useState"])(null);
    const [messages, setMessages] = (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$index$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useState"])([]);
    const [message, setMessage] = (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$index$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useState"])("");
    (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$index$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useEffect"])({
        "TenantChat.useEffect": ()=>{
            const newConnection = new __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f40$microsoft$2f$signalr$2f$dist$2f$esm$2f$HubConnectionBuilder$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["HubConnectionBuilder"]().withUrl("https://your-api/chatHub").withAutomaticReconnect().build();
            setConnection(newConnection);
        }
    }["TenantChat.useEffect"], []);
    (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$index$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["useEffect"])({
        "TenantChat.useEffect": ()=>{
            if (connection) {
                connection.start().then({
                    "TenantChat.useEffect": ()=>{
                        connection.on("ReceiveMessage", {
                            "TenantChat.useEffect": (senderId, message)=>{
                                setMessages({
                                    "TenantChat.useEffect": (prev)=>[
                                            ...prev,
                                            {
                                                senderId,
                                                message
                                            }
                                        ]
                                }["TenantChat.useEffect"]);
                            }
                        }["TenantChat.useEffect"]);
                    }
                }["TenantChat.useEffect"]);
            }
        }
    }["TenantChat.useEffect"], [
        connection
    ]);
    const sendMessage = async ()=>{
        if (connection) {
            await connection.invoke("SendMessage", "tenantId", ownerId, message);
            setMessages([
                ...messages,
                {
                    senderId: "tenantId",
                    message
                }
            ]);
            setMessage("");
        }
    };
    return /*#__PURE__*/ (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$jsx$2d$dev$2d$runtime$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["jsxDEV"])("div", {
        className: "chat",
        children: [
            messages.map((msg, index)=>/*#__PURE__*/ (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$jsx$2d$dev$2d$runtime$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["jsxDEV"])("div", {
                    children: [
                        msg.senderId,
                        ": ",
                        msg.message
                    ]
                }, index, true, {
                    fileName: "[project]/src/app/messages/page.tsx",
                    lineNumber: 45,
                    columnNumber: 9
                }, this)),
            /*#__PURE__*/ (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$jsx$2d$dev$2d$runtime$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["jsxDEV"])("input", {
                value: message,
                onChange: (e)=>setMessage(e.target.value)
            }, void 0, false, {
                fileName: "[project]/src/app/messages/page.tsx",
                lineNumber: 49,
                columnNumber: 7
            }, this),
            /*#__PURE__*/ (0, __TURBOPACK__imported__module__$5b$project$5d2f$node_modules$2f$next$2f$dist$2f$compiled$2f$react$2f$jsx$2d$dev$2d$runtime$2e$js__$5b$app$2d$client$5d$__$28$ecmascript$29$__["jsxDEV"])("button", {
                onClick: sendMessage,
                children: "Gửi"
            }, void 0, false, {
                fileName: "[project]/src/app/messages/page.tsx",
                lineNumber: 50,
                columnNumber: 7
            }, this)
        ]
    }, void 0, true, {
        fileName: "[project]/src/app/messages/page.tsx",
        lineNumber: 43,
        columnNumber: 5
    }, this);
}
_s(TenantChat, "CFtrTx8sw0SMJ+iW/Y0Nw5g951M=");
_c = TenantChat;
var _c;
__turbopack_context__.k.register(_c, "TenantChat");
if (typeof globalThis.$RefreshHelpers$ === 'object' && globalThis.$RefreshHelpers !== null) {
    __turbopack_context__.k.registerExports(module, globalThis.$RefreshHelpers$);
}
}}),
}]);

//# sourceMappingURL=src_app_messages_page_tsx_5ce31cec._.js.map
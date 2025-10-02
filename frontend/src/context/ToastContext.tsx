"use client";

import type { Toast, ToastPosition, ToastType } from "@/types/toast";
import React, { createContext, useContext, useState, useCallback } from "react";

interface ToastContextProps {
  showToast: (
    message: string,
    type?: ToastType,
    duration?: number,
    position?: ToastPosition
  ) => void;
}

const ToastContext = createContext<ToastContextProps | undefined>(undefined);

// eslint-disable-next-line react-refresh/only-export-components
export const useToast = () => {
  const ctx = useContext(ToastContext);
  if (!ctx) {
    throw new Error("useToast must be used within a ToastProvider");
  }
  return ctx;
};

export const ToastProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [toasts, setToasts] = useState<Toast[]>([]);
  const [defaultPosition] = useState<ToastPosition>("top-right");

  const removeToast = useCallback((id: string) => {
    setToasts((prev) => prev.filter((t) => t.id !== id));
  }, []);

  const showToast = useCallback(
    (
      message: string,
      type: ToastType = "info",
      duration = 3000,
      position?: ToastPosition
    ) => {
      const id = `${Date.now()}-${Math.random()}`;
      const newToast: Toast = {
        id,
        message,
        type,
        duration,
        position: position || defaultPosition,
      };
      setToasts((prev) => [...prev, newToast]);

      setTimeout(() => {
        removeToast(id);
      }, duration);
    },
    [defaultPosition, removeToast]
  );

  return (
    <ToastContext.Provider value={{ showToast }}>
      {children}
      <div className="fixed z-[9999] pointer-events-none">
        {toasts.map((toast) => (
          <div
            key={toast.id}
            className={`m-2 rounded-xl shadow-lg px-4 py-3 text-white flex justify-between items-center
              ${toast.type === "success" && "bg-green-500"}
              ${toast.type === "error" && "bg-red-500"}
              ${toast.type === "warning" && "bg-yellow-500"}
              ${toast.type === "info" && "bg-blue-500"}
            `}
            style={{
              position: "fixed",
              ...(toast.position?.includes("top")
                ? { top: "20px" }
                : { bottom: "20px" }),
              ...(toast.position?.includes("left")
                ? { left: "20px" }
                : { right: "20px" }),
            }}
          >
            <span>{toast.message}</span>
            <button
              onClick={() => removeToast(toast.id)}
              className="ml-4 text-lg font-bold cursor-pointer pointer-events-auto"
            >
              ✖
            </button>
          </div>
        ))}
      </div>
    </ToastContext.Provider>
  );
};

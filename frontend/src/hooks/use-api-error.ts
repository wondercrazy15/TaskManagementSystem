import { useState } from "react";
import axios from "axios";
import { useToast } from "@/context/ToastContext";

const getErrorMessage = (error: unknown): string => {
  if (axios.isAxiosError(error)) {
    return (
      error.response?.data?.message ||
      error.message ||
      "An unknown Axios error occurred."
    );
  } else if (error instanceof Error) {
    return error.message;
  } else {
    return "An unexpected error occurred.";
  }
};

export const useApiError = () => {
  const { showToast } = useToast();

  const [error, setError] = useState<string | null>(null);

  const handleError = (err: unknown, showUserError: boolean = true) => {
    const message = getErrorMessage(err);
    setError(message);
    console.error("API Error:", message);
    if (showUserError) {
      showToast(message, "error");
    }
  };

  const clearError = () => setError(null);

  return {
    error,
    handleError,
    clearError,
  };
};

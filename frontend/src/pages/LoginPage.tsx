import { LoginForm } from "../components/forms/LoginForm";
import { API_ENDPOINTS } from "@/api/api";
import { post } from "@/api/ApiClient";
import { useAppContext } from "@/context/AppContext";
import { useToast } from "@/context/ToastContext";
import { useApiError } from "@/hooks/use-api-error";
import type { LoginRequest, LoginResponse } from "@/types/auth";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

export const LoginPage = () => {
  const navigate = useNavigate();
  const { showToast } = useToast();
  const { handleError } = useApiError();
  const { setUserInfo } = useAppContext();
  const [isLoading, setIsLoading] = useState(false);

  const handleLogin = async (data: { email: string; password: string }) => {
    try {
      setIsLoading(true);
      const response = await post<LoginRequest, LoginResponse>(
        API_ENDPOINTS.AUTH.LOGIN,
        {
          email: data.email,
          password: data.password,
        }
      );

      localStorage.setItem("token", response.data.token);
      localStorage.setItem("userid", response.data.id);
      setUserInfo(response.data);
      showToast("User logged in successfully", "success");
      navigate("/dashboard");
    } catch (err) {
      handleError(err);
    } finally {
      setIsLoading(false); 
    }
  };
  return (
    <div className="flex justify-center items-center h-screen">
      <div className="bg-white w-[400px]  shadow-[0_0_3px_1px_rgba(0,0,0,0.12)] p-5 rounded-[6px] ">
        <h3 className="text-center font-semibold mb-3 text-xl">Login</h3>
        <LoginForm isLoading={isLoading} onSubmit={handleLogin} />
      </div>
    </div>
  );
};

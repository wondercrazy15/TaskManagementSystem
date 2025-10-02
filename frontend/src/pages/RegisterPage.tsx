import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse,
} from "@/types/auth";
import { API_ENDPOINTS } from "@/api/api";
import { post } from "@/api/ApiClient";
import { useNavigate } from "react-router-dom";
import { useToast } from "@/context/ToastContext";
import { useApiError } from "@/hooks/use-api-error";
import { useAppContext } from "@/context/AppContext";
import { RegisterForm } from "@/components/forms/Registration";

export const RegisterPage = () => {
  const navigate = useNavigate();
  const { showToast } = useToast();
  const { handleError } = useApiError();
  const { setUserInfo } = useAppContext();

  const registerUser = async (data: {
    username: string;
    email: string;
    password: string;
    confirmPassword: string;
    role?: string;
  }) => {
    try {
      const response = await post<RegisterRequest, RegisterResponse>(
        API_ENDPOINTS.AUTH.REGISTER,
        {
          userName: data.username,
          email: data.email,
          password: data.password,
          role: "user",
        }
      );

      if (response.data) {
        showToast("User registered successfully", "success");
        const response = await post<LoginRequest, LoginResponse>(
          API_ENDPOINTS.AUTH.LOGIN,
          {
            email: data.email,
            password: data.password,
          }
        );
        setUserInfo(response.data);
        localStorage.setItem("token", response.data.token);
        localStorage.setItem("userid", response.data.id);

        showToast("User logged in successfully", "success");
        navigate("/dashboard");
      }
    } catch (error) {
      handleError(error);
    }
  };

  const handleRegister = async (data: {
    username: string;
    email: string;
    password: string;
    confirmPassword: string;
    role?: string;
  }) => {
    await registerUser(data);
  };
  return (
    <div className="flex justify-center items-center h-screen">
      <div className="bg-white w-[400px]  shadow-[0_0_3px_1px_rgba(0,0,0,0.12)] p-5 rounded-[6px] ">
        <h3 className="text-center font-semibold mb-3 text-xl">Register</h3>
        <RegisterForm onSubmit={handleRegister} />
      </div>
    </div>
  );
};

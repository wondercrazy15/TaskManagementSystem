import { createBrowserRouter } from "react-router-dom";
import { RegisterPage } from "./pages/RegisterPage";
import { LoginPage } from "./pages/LoginPage";
import { DashboardPage } from "./pages/DashboardPage";
import { AuthRedirect } from "./components/common/AuthRedirect";
import { ProtectedRoute } from "./components/common/ProtectedRoute";

export const router = createBrowserRouter([
  {
    path: "/login",
    element: (
      <AuthRedirect>
        <LoginPage />
      </AuthRedirect>
    ),
  },
  {
    path: "/register",
    element: (
      <AuthRedirect>
        <RegisterPage />
      </AuthRedirect>
    ),
  },
  {
    path: "/dashboard",
    element: (
      <ProtectedRoute>
        <DashboardPage />
      </ProtectedRoute>
    ),
  },
  {
    path: "*",
    element: (
      <AuthRedirect>
        <LoginPage />
      </AuthRedirect>
    ),
  },
]);

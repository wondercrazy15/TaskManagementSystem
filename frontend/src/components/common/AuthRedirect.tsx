import { Navigate } from "react-router-dom";

export const AuthRedirect = ({ children }: { children: React.ReactNode }) => {
  const token = localStorage.getItem("token");
  if (token) {
    return <Navigate to="/dashboard" replace />;
  }
  return <>{children}</>;
};

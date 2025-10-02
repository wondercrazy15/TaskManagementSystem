import { Navigate } from "react-router-dom";
import Header from "./Header";

export const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
  const token = localStorage.getItem("token");
  if (!token) {
    return <Navigate to="/login" replace />;
  }
  return<>
  <Header/>
  {children}</>;
};

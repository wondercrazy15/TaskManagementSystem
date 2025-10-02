import { LogOut } from "lucide-react";
import { useNavigate } from "react-router-dom";

const Header = () => {
  const navigate = useNavigate();
  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("userid");
    navigate("/login");
  };
  return (
    <header className="bg-slate-800 min-h-14 px-4 flex items-center justify-between">
      <a href="/" className="text-white text-lg">
        Task Management System
      </a>
      <button
        className="btn btn-secondary text-white flex items-center gap-2"
        onClick={handleLogout}
      >
        <LogOut />
      </button>
    </header>
  );
};
export default Header;

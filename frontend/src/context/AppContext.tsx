import type { LoginResponse } from "@/types/auth";
import type { TaskListResponse } from "@/types/task";
import type { UserListResponse } from "@/types/user";
import { createContext, useContext, useState, type ReactNode } from "react";

type AppContextType = {
  userInfo: LoginResponse | null;
  setUserInfo: (user: LoginResponse | null) => void;

  userList: UserListResponse[];
  setUserList: (users: UserListResponse[]) => void;

  taskList: TaskListResponse[];
  setTaskList: (tasks: TaskListResponse[]) => void;
};

const AppContext = createContext<AppContextType | undefined>(undefined);

export const AppProvider = ({ children }: { children: ReactNode }) => {
  const [userInfo, setUserInfo] = useState<LoginResponse | null>(null);
  const [userList, setUserList] = useState<UserListResponse[]>([]);
  const [taskList, setTaskList] = useState<TaskListResponse[]>([]);

  return (
    <AppContext.Provider
      value={{ userInfo, setUserInfo, userList, setUserList, taskList, setTaskList }}
    >
      {children}
    </AppContext.Provider>
  );
};

// eslint-disable-next-line react-refresh/only-export-components
export const useAppContext = () => {
  const context = useContext(AppContext);
  if (!context) throw new Error("useAppContext must be used inside AppProvider");
  return context;
};

export type UserInfo = {
  id: string;
  name: string;
  email: string;
} | null;

export type Task = {
  id: string;
  title: string;
  status: "TODO" | "IN_PROGRESS" | "DONE";
};

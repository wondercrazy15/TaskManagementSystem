/* eslint-disable react-refresh/only-export-components */
import React, { useEffect, useRef, useState } from "react";
import { Pencil, Check, X } from "lucide-react";
import { Input } from "../components/ui/input";
import { Textarea } from "../components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../components/ui/select";
import { get, post, put, remove } from "@/api/ApiClient";
import { API_ENDPOINTS } from "@/api/api";
import type { Task, TaskAddRequest, TaskListResponse } from "@/types/task";
import type { UserListResponse } from "@/types/user";
import { useAppContext } from "@/context/AppContext";
import AppDialog from "@/components/common/Dialog";
import TaskForm, { type TaskFormRef } from "@/components/forms/TaskForm";
import type { TaskFormData } from "@/util/validators";
import { useApiError } from "@/hooks/use-api-error";
import { useToast } from "@/context/ToastContext";
import { Button } from "@/components/common/Button";
import { MultiSelect } from "@/components/common/MultiSelect";
import Loader from "@/components/common/Loader";
import { Trash } from "lucide-react";

type ColumnKey = "TODO" | "IN_PROGRESS" | "DONE";

export enum TaskStatus {
  TODO = "TODO",
  IN_PROGRESS = "IN_PROGRESS",
  DONE = "DONE",
}

export enum Priority {
  LOW = "LOW",
  MEDIUM = "MEDIUM",
  HIGH = "HIGH",
}

export const DashboardPage: React.FC = () => {
  const formRef = useRef<TaskFormRef>(null);
  const { setTaskList, setUserList, userList, userInfo } = useAppContext();
  const [editingId, setEditingId] = useState<string | null>(null);
  const [formData, setFormData] = useState<Task | null>(null);
  const [open, setOpen] = useState(false);
  const [tasksByColumn, setTasksByColumn] = useState<Record<ColumnKey, Task[]>>(
    {
      TODO: [],
      IN_PROGRESS: [],
      DONE: [],
    }
  );
  const { handleError } = useApiError();
  const { showToast } = useToast();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [filterUser, setFilterUser] = useState<string[]>([]);
  const userId = localStorage.getItem("userid");

  const handleDelete = async (taskId: string) => {
    try {
      await remove(`${API_ENDPOINTS.TASKS.UPDATE(taskId)}`);
      showToast("Task deleted successfully", "success");

      setTasksByColumn((prev) => {
        const next = { ...prev };
        (Object.keys(next) as ColumnKey[]).forEach((col) => {
          next[col] = next[col].filter((t) => t.id !== taskId);
        });
        return next;
      });
    } catch (err) {
      handleError(err);
    }
  };

  const handleSubmit = async (data: TaskFormData) => {
    try {
      const response = await post<TaskAddRequest, Task>(
        API_ENDPOINTS.TASKS.ADD_NEW,
        { ...data, creatorId: userInfo?.id ? userInfo?.id : userId ? userId : "" }
      );
      if (response.data) {
        showToast("Task added successfully", "success");
        await fetchTasksAndUsers();
      }
      setOpen(false);
    } catch (err) {
      handleError(err);
    }
  };

  const handleEdit = async (data: TaskFormData, editingId: string) => {
    try {
      const response = await put<TaskAddRequest, Task>(
        API_ENDPOINTS.TASKS.ADD_NEW,
        { ...data, creatorId: userInfo?.id ? userInfo?.id : userId ? userId : "", id: editingId }
      );
      if (response.data) {
        showToast("Task updated successfully", "success");
        await fetchTasksAndUsers();
      }
      setOpen(false);
    } catch (err) {
      handleError(err);
    }
  };

  const startEditing = (task: Task) => {
    setEditingId(task.id);
    setFormData({ ...task });
  };

  const cancelEditing = () => {
    setEditingId(null);
    setFormData(null);
  };

  const saveTask = () => {
    if (!editingId || !formData) return;

    const data: TaskFormData = {
      title: formData.title,
      description: formData.description,
      taskStatusId: formData.taskStatusId.toString(),
      priority: formData.priority as "Low" | "Medium" | "High",
      assigneeId: formData.taskAssignments,
    };

    handleEdit(data, editingId);
    setTasksByColumn((prev) => {
      const next = { ...prev };
      (Object.keys(next) as ColumnKey[]).forEach((col) => {
        const idx = next[col].findIndex((t) => t.id === editingId);
        if (idx !== -1) {
          next[col] = [
            ...next[col].slice(0, idx),
            { ...formData },
            ...next[col].slice(idx + 1),
          ];
        }
      });
      return next;
    });
    setEditingId(null);
    setFormData(null);
  };

  const fetchTasksAndUsers = async () => {
    try {
      setIsLoading(true);
      const [tasksResponse, usersResponse] = await Promise.all([
        get<TaskListResponse[]>(API_ENDPOINTS.TASKS.LIST),
        get<UserListResponse[]>(API_ENDPOINTS.USERS.LIST),
      ]);

      setTaskList(tasksResponse.data);
      setUserList(usersResponse.data);

      const mapStatus = (statusId: number): TaskStatus =>
        statusId === 1
          ? TaskStatus.TODO
          : statusId === 2
          ? TaskStatus.IN_PROGRESS
          : TaskStatus.DONE;

      const formattedTasks: Task[] = tasksResponse.data.map((t) => ({
        id: t.id,
        title: t.title,
        description: t.description,
        status: mapStatus(t.taskStatusId),
        priority: t.priority.toUpperCase() as Task["priority"],
        taskStatusId: t.taskStatusId,
        taskAssignments: t.taskAssignments,
        creatorId: t.creatorId,
        createdAt: t.createdAt,
        updatedAt: t.updatedAt,
      }));
      setTasksByColumn({
        TODO: formattedTasks.filter((t) => t.taskStatusId === 1 || t.taskStatusId === 0),
        IN_PROGRESS: formattedTasks.filter((t) => t.taskStatusId === 2),
        DONE: formattedTasks.filter((t) => t.taskStatusId === 3),
      });
    } catch (error) {
      handleError(error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchTasksAndUsers();
  }, []);

  useEffect(() => {
    if (filterUser.length > 0) {
      let isMounted = true;

      const fetchFilteredTasks = async () => {
        try {
          const response = await post<string[], TaskListResponse[]>(
            API_ENDPOINTS.TASKS.LIST_BY_USERS,
            filterUser
          );

          if (isMounted && response.data) {
            setTaskList(response.data);

            const mapStatus = (statusId: number): TaskStatus =>
              statusId === 1
                ? TaskStatus.TODO
                : statusId === 2
                ? TaskStatus.IN_PROGRESS
                : TaskStatus.DONE;

            const formattedTasks: Task[] = response.data.map((t) => ({
              id: t.id,
              title: t.title,
              description: t.description,
              status: mapStatus(t.taskStatusId),
              priority: t.priority.toUpperCase() as Task["priority"],
              taskStatusId: t.taskStatusId,
              taskAssignments: t.taskAssignments,
              creatorId: t.creatorId,
              createdAt: t.createdAt,
              updatedAt: t.updatedAt,
            }));

            setTasksByColumn({
              TODO: formattedTasks.filter((t) => t.taskStatusId === 1 || t.taskStatusId === 0),
              IN_PROGRESS: formattedTasks.filter((t) => t.taskStatusId === 2),
              DONE: formattedTasks.filter((t) => t.taskStatusId === 3),
            });
          }
        } catch (error) {
          handleError(error);
        }
      };

      fetchFilteredTasks();

      return () => {
        isMounted = false;
      };
    } else {
      fetchTasksAndUsers();
    }
  }, [filterUser]);

  if (isLoading) return <Loader />;

  return (
    <div className="h-full p-6">
      <div className="flex justify-between py-4">
        <MultiSelect
          options={
            userList?.map((u) => ({
              value: u.id,
              label: u.username,
            })) ?? []
          }
          placeholder="Search By User"
          values={filterUser}
          className="w-[300px] bg-grey-800"
          onChange={(vals: (string | number)[]) =>
            setFilterUser(vals.map(String))
          }
        />

        <AppDialog
          open={open}
          onOpenChange={setOpen}
          trigger={<Button onClick={() => setOpen(true)}>Add Task</Button>}
          title="Add Task"
          footer={
            <div className="flex gap-4 justify-center">
              <Button onClick={() => formRef.current?.submit()}>Save</Button>
              <Button className="dark-btn" onClick={() => setOpen(false)}>Cancel</Button>
            </div>
          }
        >
          <TaskForm
            ref={formRef}
            onSubmit={handleSubmit}
            assignees={userList.map((u) => ({
              value: u.id,
              label: u.username,
            }))}
          />
        </AppDialog>
      </div>

      <div className="grid grid-cols-3 gap-6">
        {(Object.keys(tasksByColumn) as ColumnKey[]).map((column) => (
          <div key={column} className="bg-gray-50 rounded-lg p-4 shadow-sm">
            <h3 className="text-sm font-semibold text-gray-700 mb-3">
              {column.replace("_", " ")}
            </h3>
            <div className="space-y-3 h-[calc(100vh-250px)] overflow-auto">
              {tasksByColumn[column].map((task) => (
                <div key={task.id} className="bg-white p-3 rounded border">
                  {editingId === task.id && formData ? (
                    <div className="space-y-2">
                      <Input
                        value={formData.title}
                        onChange={(e) =>
                          setFormData(
                            (p) => p && { ...p, title: e.target.value }
                          )
                        }
                        placeholder="Title"
                      />
                      <Textarea
                        value={formData.description}
                        onChange={(e) =>
                          setFormData(
                            (p) => p && { ...p, description: e.target.value }
                          )
                        }
                        placeholder="Description"
                      />
                      <Select
                        value={formData.taskStatusId.toString()}
                        onValueChange={(val) =>
                          setFormData((p) =>
                            p ? { ...p, taskStatusId: parseInt(val, 10) } : p
                          )
                        }
                      >
                        <SelectTrigger>
                          <SelectValue placeholder="Status" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="1">TODO</SelectItem>
                          <SelectItem value="2">IN PROGRESS</SelectItem>
                          <SelectItem value="3">DONE</SelectItem>
                        </SelectContent>
                      </Select>

                      <Select
                        value={formData.priority}
                        onValueChange={(val) =>
                          setFormData((p) => p && { ...p, priority: val })
                        }
                      >
                        <SelectTrigger>
                          <SelectValue placeholder="Priority" />
                        </SelectTrigger>
                        <SelectContent>
                          {Object.values(Priority).map((priority) => (
                            <SelectItem key={priority} value={priority}>
                              {priority}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>

                      <MultiSelect
                        options={
                          userList?.map((u) => ({
                            value: u.id,
                            label: u.username,
                          })) ?? []
                        }
                        values={formData?.taskAssignments ?? []}
                        onChange={(vals: (string | number)[]) =>
                          setFormData((p) =>
                            p ? { ...p, taskAssignments: vals.map(String) } : p
                          )
                        }
                      />

                      <div className="flex justify-end gap-2">
                        <button
                          onClick={saveTask}
                          className="p-2 rounded bg-green-500 text-white"
                        >
                          <Check size={16} />
                        </button>
                        <button
                          onClick={cancelEditing}
                          className="p-2 rounded bg-gray-300 text-black"
                        >
                          <X size={16} />
                        </button>
                      </div>
                    </div>
                  ) : (
                    <div className="flex justify-between items-center">
                      <div>
                        <p className="font-medium">{task.title}</p>
                        <p className="text-xs text-gray-500">
                          {task.description}
                        </p>
                      </div>
                      <div className="flex gap-2">
                        <button
                          onClick={() => startEditing(task)}
                          className="text-gray-600 hover:text-gray-900"
                        >
                          <Pencil size={16} />
                        </button>
                        <button
                          onClick={() => handleDelete(task.id)}
                          className="text-red-600 hover:text-red-800"
                        >
                          <Trash size={16} />
                        </button>
                      </div>
                    </div>
                  )}
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

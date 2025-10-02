export interface TaskListResponse {
  id: string;
  title: string;
  description: string;
  taskStatusId: number;
  priority: string;
  creatorId: string;
  createdAt: string;
  updatedAt: string;
  taskAssignments: string[];
}

export interface TaskAddRequest {
  id?: string;
  title: string;
  description: string;
  taskStatusId: number | string;
  priority: string;
  assigneeId: string[];
  creatorId: string;
}

export interface Task {
  id: string;
  title: string;
  description: string;
  taskStatusId: number;
  priority: string;
  creatorId: string;
  createdAt: string;
  updatedAt: string;
  taskAssignments: string[];
}

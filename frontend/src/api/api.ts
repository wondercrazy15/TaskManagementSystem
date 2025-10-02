export const API_ENDPOINTS = {
  AUTH: {
    LOGIN: '/api/User/login',
    REGISTER: '/api/User/register',
    USER_INFO: '/connect/userinfo',
  },

  USERS: {
    LIST: '/api/User/getAllUsers',
    DETAIL: (id: string) => `/api/user/detail/${id}`,
  },

  TASKS: {
    LIST: '/api/Tasks',
    LIST_BY_USERS: '/api/Tasks/getTasksByAssignees',
    ADD_NEW: '/api/Tasks',
    UPDATE: (id: string) => `/api/Tasks/${id}`,
    ASSIGN: (taskId: string, userId: string) => `/api/Tasks/${taskId}/assign/${userId}`,
  },
};
